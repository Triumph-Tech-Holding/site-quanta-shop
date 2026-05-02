using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;
using MMN.Api.Controllers.v1;
using MMN.Api.Helpers;
using MMN.Api.Hubs;
using MMN.Api.Service;
using MMN.Api.Services;
using MMN.Dominio.Api.Configuration;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Negocio;
using MMN.Repositorio.Contexto;
using MMN.Repositorio.Repositorio;
using MMN.Util.Cache;
using MMN.Util.Jwt;
using MMN.Util.Model;
using MMN.Util.Translation;
using SimplesmenteSou.Configuration;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Api
{
    public class Startup
    {
        public bool IsDev { get; set; }

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            IsDev = env.IsDevelopment();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHealthChecks();

            var allowedOrigins = (Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (allowedOrigins.Length == 0)
            {
                allowedOrigins = new[]
                {
                    "https://quantashop.com.br",
                    "https://www.quantashop.com.br",
                    "https://escritorio.quantashop.com.br",
                    "https://app.quantashop.com.br"
                };
            }

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder =>
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(allowedOrigins)
                    .AllowCredentials()
                    .WithExposedHeaders("token-expired"));
            });

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("auth-limit", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromSeconds(60);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });
                options.RejectionStatusCode = 429;
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSignalR();

            // configure strongly typed settings objects
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // Override GoogleClientId from env var so no DB mutation is needed at startup
            services.PostConfigure<AppSettings>(opts =>
            {
                var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                if (!string.IsNullOrEmpty(clientId))
                    opts.GoogleClientId = clientId;
            });
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret)),
                ValidateIssuer = !string.IsNullOrEmpty(token.Issuer),
                ValidIssuer = token.Issuer,
                ValidateAudience = !string.IsNullOrEmpty(token.Audience),
                ValidAudience = token.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("token-expired", "true");

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddHttpContextAccessor();

            string connection = ConexaoDataBase.Connection();

            // Check if we're in test mode (USE_TEST_DATABASE=true skips real database)
            bool useTestDatabase = Environment.GetEnvironmentVariable("USE_TEST_DATABASE")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
            bool hasValidConnection = !useTestDatabase && !string.IsNullOrEmpty(connection);
            
            if (hasValidConnection)
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(connection);
                });

                //Dapper
                services.AddScoped<IDbConnection>(sp => new SqlConnection(connection));
            }
            else
            {
                // Development mode without database - register null/mock services
                Console.WriteLine("WARNING: Running without database connection (SQL_CONNECTION_STRING is empty or DISABLED)");
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
                services.AddScoped<IDbConnection>(sp => null);
            }

            //Auto Mapper Configurations
            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));

            MappingInterfaces(services);

            AutoMapperConfig.RegisterMappings();

            // Configurar versionamento da API
            //services.AddApiVersioning(o =>
            //{
            //    o.ReportApiVersions = true;
            //    o.AssumeDefaultVersionWhenUnspecified = true;
            //    o.DefaultApiVersion = new ApiVersion(1, 0);
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MMN API", Version = "v1" });
                //c.SwaggerDoc("v2", new OpenApiInfo { Title = "MMN API", Version = "v2" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath, true);
            });

            if (!IsDev)
            {
                services.AddHostedService<AwinService>();
                services.AddHostedService<ExtratoPagarMe>();
                services.AddHostedService<FaturaService>();
                services.AddHostedService<ExtratoAsaas>();
            }
        }

        private void MappingInterfaces(IServiceCollection services)
        {
            //Mapeamenteo de negócios
            services.AddScoped<IBancoNegocio, BancoNegocio>();
            services.AddScoped<ICategoriaNegocio, CategoriaNegocio>();
            services.AddScoped<ICarrosselNegocio, CarrosselNegocio>();
            services.AddScoped<ICidadeNegocio, CidadeNegocio>();
            services.AddScoped<IConfiguracaoNegocio, ConfiguracaoNegocio>();
            services.AddScoped<IEstadoNegocio, EstadoNegocio>();
            services.AddScoped<IGraduacaoNegocio, GraduacaoNegocio>();
            services.AddScoped<IGrupoNegocio, GrupoNegocio>();
            services.AddScoped<ILancamentoNegocio, LancamentoNegocio>();
            services.AddScoped<IMenuNegocio, MenuNegocio>();
            services.AddScoped<IPedidoNegocio, PedidoNegocio>();
            services.AddScoped<IProdutoNegocio, ProdutoNegocio>();
            services.AddScoped<ITipoNegocio, TipoNegocio>();
            services.AddScoped<IUsuarioBancoNegocio, UsuarioBancoNegocio>();
            services.AddScoped<IUsuarioEnderecoNegocio, UsuarioEnderecoNegocio>();
            services.AddScoped<IUsuarioNegocio, UsuarioNegocio>();
            services.AddScoped<IUsuarioProdutoNegocio, UsuarioProdutoNegocio>();
            services.AddScoped<IStatusNegocio, StatusNegocio>();
            services.AddScoped<IUsuarioConfiguracaoNegocio, UsuarioConfiguracaoNegocio>();
            services.AddScoped<IProdutoNivelNegocio, ProdutoNivelNegocio>();
            services.AddScoped<IGraduacaoNegocio, GraduacaoNegocio>();
            services.AddScoped<IMensagemNegocio, MensagemNegocio>();
            services.AddScoped<IMensagemGraduacaoNegocio, MensagemGraduacaoNegocio>();
            services.AddScoped<IAnuncianteNegocio, AnuncianteNegocio>();
            services.AddScoped<IPremiacaoDownlineNegocio, PremiacaoDownlineNegocio>();
            services.AddScoped<IAnuncianteCashBackNegocio, AnuncianteCashBackNegocio>();
            services.AddScoped<ISaqueNegocio, SaqueNegocio>();
            services.AddScoped<ICategoriaAnuncianteNegocio, CategoriaAnuncianteNegocio>();
            services.AddScoped<ICredenciamentoNegocio, CredenciamentoNegocio>();
            services.AddScoped<ITransacaoNegocio, TransacaoNegocio>();
            services.AddScoped<IFaqNegocio, FaqNegocio>();
            services.AddScoped<IGrupoMenuNegocio, GrupoMenuNegocio>();
            services.AddScoped<IUsuarioPremiacaoNegocio, UsuarioPremiacaoNegocio>();
            services.AddScoped<ISuporteNegocio, SuporteNegocio>();
            services.AddScoped<IAlteracaoPerfilNegocio, AlteracaoPerfilNegocio>();
            services.AddScoped<IRefreshTokenNegocio, RefreshTokenNegocio>();
            services.AddScoped<IPagamentoNegocio, PagamentoNegocio>();
            services.AddScoped<IParceiroNegocio, ParceiroNegocio>();
            services.AddScoped<ITutorialNegocio, TutorialNegocio>();
            services.AddScoped<IMaterialApoioNegocio, MaterialApoioNegocio>();
            services.AddScoped<ICupomCashbackNegocio, CupomCashbackNegocio>();
            services.AddScoped<ITipoPagamentoNegocio, TipoPagamentoNegocio>();
            services.AddScoped<IProvedorAutenticacaoNegocio, ProvedorAutenticacaoNegocio>();
            services.AddScoped<IFaturaNegocioNovo, FaturaNegocioNovo>();
            services.AddScoped<IPagamentoPedidoRepositorio, PagamentoPedidoRepositorio>();
            services.AddScoped<ICashbackNegocio, CashbackNegocio>();
            services.AddScoped<IPromocaoNegocio, PromocaoNegocio>();
            services.AddScoped<IPreCadastroNegocio, PreCadastroNegocio>();
            services.AddScoped<IEcossistemaNegocio, EcossistemaNegocio>();

            //Mapeamentos de Repositório
            services.AddScoped<IBancoRepositorio, BancoRepositorio>();
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<ICidadeRepositorio, CidadeRepositorio>();
            services.AddScoped<ICarrosselRepositorio, CarrosselRepositorio>();
            services.AddScoped<IConfiguracaoRepositorio, ConfiguracaoRepositorio>();
            services.AddScoped<IEstadoRepositorio, EstadoRepositorio>();
            services.AddScoped<IFaqRepositorio, FaqRepositorio>();
            services.AddScoped<IGraduacaoRepositorio, GraduacaoRepositorio>();
            services.AddScoped<IGrupoRepositorio, GrupoRepositorio>();
            services.AddScoped<IHistoricoGraduacaoRepositorio, HistoricoGraduacaoRepositorio>();
            services.AddScoped<ILancamentoRepositorio, LancamentoRepositorio>();
            services.AddScoped<IMensagemRepositorio, MensagemRepositorio>();
            services.AddScoped<IMenuRepositorio, MenuRepositorio>();
            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
            services.AddScoped<IPedidoDetalheRepositorio, PedidoDetalheRepositorio>();
            services.AddScoped<IProceduresRepositorio, ProceduresRepositorio>();
            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            services.AddScoped<ITipoRepositorio, TipoRepositorio>();
            services.AddScoped<ICuponCashbackRepositorio, CuponCashbackRepositorio>();
            services.AddScoped<ITransacaoRepositorio, TransacaoRepositorio>();
            services.AddScoped<IUsuarioBancoRepositorio, UsuarioBancoRepositorio>();
            services.AddScoped<IUsuarioEnderecoRepositorio, UsuarioEnderecoRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IUsuarioProdutoRepositorio, UsuarioProdutoRepositorio>();
            services.AddScoped<IStatusRepositorio, StatusRepositorio>();
            services.AddScoped<IUsuarioConfiguracaoRepositorio, UsuarioConfiguracaoRepositorio>();
            services.AddScoped<IProdutoNivelRepositorio, ProdutoNivelRepositorio>();
            services.AddScoped<IGraduacaoRepositorio, GraduacaoRepositorio>();
            services.AddScoped<IMensagemRepositorio, MensagemRepositorio>();
            services.AddScoped<IMensagemGraduacaoRepositorio, MensagemGraduacaoRepositorio>();
            services.AddScoped<IAnuncianteRepositorio, AnuncianteRepositorio>();
            services.AddScoped<IOrdenacaoAnuncioRepositorio, OrdenacaoAnuncioRepositorio>();
            services.AddScoped<IPremiacaoDownlineRepositorio, PremiacaoDownlineRepositorio>();
            services.AddScoped<IAnuncianteCashBackRepositorio, AnuncianteCashBackRepositorio>();
            services.AddScoped<ISaqueRepositorio, SaqueRepositorio>();
            services.AddScoped<ICategoriaAnuncianteRepositorio, CategoriaAnuncianteRepositorio>();
            services.AddScoped<ICredenciamentoRepositorio, CredenciamentoRepositorio>();
            services.AddScoped<IGrupoMenuRepositorio, GrupoMenuRepositorio>();
            services.AddScoped<IUsuarioPremiacaoRepositorio, UsuarioPremiacaoRepositorio>();
            services.AddScoped<ISuporteRepositorio, SuporteRepositorio>();
            services.AddScoped<IAlteracaoPerfilRepositorio, AlteracaoPerfilRepositorio>();
            services.AddScoped<IRefreshTokenRepositorio, RefreshTokenRepositorio>();
            services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
            services.AddScoped<IParceiroRepositorio, ParceiroRepositorio>();
            services.AddScoped<ITutorialRepositorio, TutorialRepositorio>();
            services.AddScoped<IMaterialApoioRepositorio, MaterialApoioRepositorio>();
            services.AddScoped<ICuponCashbackRepositorio, CuponCashbackRepositorio>();
            services.AddScoped<ICuponCashbackPedidoRepositorio, CuponCashbackPedidoRepositorio>();
            services.AddScoped<IAutenticacaoExternaRepositorio, AutenticacaoExternaRepositorio>();
            services.AddScoped<IProvedorAutenticacaoRepositorio, ProvedorAutenticacaoRepositorio>();
            services.AddScoped<IPromocaoRepositorio, PromocaoRepositorio>();
            services.AddScoped<IEcossistemaRepositorio, EcossistemaRepositorio>();
            services.AddScoped<IQuantaAmizadeNegocio, QuantaAmizadeNegocio>();
            services.AddScoped<IQuantaAmizadeRepositorio, QuantaAmizadeRepositorio>();

            //Mapeamentos genéricos
            services.AddScoped<ILocation, Location>();
            services.AddScoped<ICache, Cache>();
            services.AddScoped<ITokenUtil, TokenUtil>();
            services.AddScoped<IRelatorios, Relatorios>();

            //Mapeamentos services
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<ICarouselsService, CarouselsService>();
            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IPartnersService, PartnersService>();
            services.AddScoped<IQuantaFriendshipService, QuantaFriendshipService>();
            services.AddScoped<ITotalizersUserService, TotalizersUserService>();
            services.AddScoped<IAwinFeedService, AwinFeedService>();
            services.AddScoped<IAwinWebhookService, AwinWebhookService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();

            services.AddHttpClient<IBotConversaService, BotConversaService>();
            services.AddHttpClient<WhatsAppService>();
            services.AddScoped<IAsaasService, AsaasService>();
            services.AddScoped<IPagarmeBilletService, PagarmeBilletService>();
                }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // Seed test data when using in-memory database
            bool useTestDatabase = Environment.GetEnvironmentVariable("USE_TEST_DATABASE")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
            if (useTestDatabase)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    db.Database.EnsureCreated();
                    SeedTestData(db);
                }
            }

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(ExceptionHandler.GlobalExceptionHandler);

            app.UseAuthentication();

            app.UseRateLimiter();

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "MMN API V1");
                //opt.SwaggerEndpoint("/swagger/v2/swagger.json", "MMN API V2");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();
            app.UseCors("AllowMyOrigin");
            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapHub<BuyConfirmationHub>("/buy-confirmation");
                endpoints.MapWebHookSubscriptionEndpoints();
            });
        }

        private void SeedTestData(DatabaseContext db)
        {
            if (db.Usuario.Any()) return;

            Console.WriteLine("Seeding test data...");

            var salt = MMN.Util.Util.Hash.Get_SALT();
            var testUserId = Guid.NewGuid();
            var email = "admin@quantashop.com.br";
            var senha = "Quanta2026!";
            var senhaHash = MMN.Util.Util.Hash.Get_HASH_SHA512(senha, email, salt);

            var grupo = new MMN.Dominio.Model.Grupo
            {
                IdGrupo = 1,
                Descricao = "Administrador",
                Ativo = true
            };
            db.Grupo.Add(grupo);

            var graduacao = new MMN.Dominio.Model.Graduacao
            {
                IdGraduacao = 1,
                Nome = "Bronze",
                Nivel = 1
            };
            db.Graduacao.Add(graduacao);

            var usuario = new MMN.Dominio.Model.Usuario
            {
                IdUsuario = testUserId,
                IdGrupo = 1,
                Nome = "Admin Quanta",
                Email = email,
                Login = email,
                Senha = senhaHash,
                SaltKey = salt,
                Celular = "11999999999",
                Ativo = true,
                Bloqueado = false,
                EmailConfirmado = true,
                Cultura = "pt-BR",
                DataCadastro = DateTime.UtcNow,
                Perfil = 'A',
                TermosDeAceite = true,
                IdGraduacao = 1,
                TentativasIncorretas = 0
            };
            db.Usuario.Add(usuario);

            var categoria = new MMN.Dominio.Model.Categoria
            {
                IdCategoria = 1,
                Nome = "Eletronicos",
                Ativo = true
            };
            db.Categoria.Add(categoria);

            for (int i = 1; i <= 5; i++)
            {
                db.Produto.Add(new MMN.Dominio.Model.Produto
                {
                    IdProduto = i,
                    IdCategoria = 1,
                    Nome = $"Produto Teste {i}",
                    Descricao = $"Descricao do produto de teste numero {i}",
                    ImagemUrl = "https://via.placeholder.com/300",
                    Valor = 100m * i,
                    Pontos = 10 * i,
                    Ativo = true,
                    Visivel = true,
                    DataCriacao = DateTime.UtcNow,
                    Parcelas = 12,
                    ReaisPorPonto = 1.0m,
                    TetoBinario = 0
                });
            }

            db.Tipo.Add(new MMN.Dominio.Model.Tipo { IdTipo = 1, Chave = "CB", Descricao = "Cashback", Ativo = true });
            db.Status.Add(new MMN.Dominio.Model.Status { IdStatus = 1, Nome = "Confirmado" });

            for (int i = 1; i <= 3; i++)
            {
                var transacao = new MMN.Dominio.Model.Transacao
                {
                    IdTransacao = i,
                    IdUsuario = testUserId,
                    IdTipo = 1,
                    IdStatus = 1,
                    ValorPrincipal = 50m * i,
                    DataTransacao = DateTime.UtcNow.AddDays(-i * 7),
                    Descricao = $"Cashback da compra #{i}",
                    Ativo = true
                };
                db.Transacao.Add(transacao);

                db.Lancamento.Add(new MMN.Dominio.Model.Lancamento
                {
                    IdLancamento = i,
                    IdUsuario = testUserId,
                    IdTransacao = i,
                    IdTipo = 1,
                    IdStatus = 1,
                    Valor = 50m * i,
                    DataLancamento = DateTime.UtcNow.AddDays(-i * 7),
                    Descricao = $"Lancamento cashback #{i}",
                    Ativo = true,
                    Bloqueado = false,
                    OrdemExibicao = i
                });
            }

            db.UsuarioProduto.Add(new MMN.Dominio.Model.UsuarioProduto
            {
                IdUsuarioProduto = 1,
                IdUsuario = testUserId,
                IdProduto = 1,
                IdPedido = 1,
                DataVinculo = DateTime.UtcNow,
                Ativo = true
            });

            db.SaveChanges();
            Console.WriteLine("Test data seeded successfully!");
        }
    }
}
