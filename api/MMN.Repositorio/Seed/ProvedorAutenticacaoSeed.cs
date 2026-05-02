using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Seed
{
    public static class ProvedorAutenticacaoSeed
    {
        public static void SeedProvedorAutenticacao(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ProvedorAutenticacao>()
                .HasData(
                    new ProvedorAutenticacao
                    {
                        IdProvedorAutenticacao = -1,
                        // UrlApi do tokeninfo do Google (One Tap/GSI — não usa OAuth2 token exchange).
                        // O Client Secret não é necessário no fluxo de credential (ID token).
                        UrlApi = "https://oauth2.googleapis.com/tokeninfo",
                        EndpointCadastro = "api/user/registrarGoogleCredential",
                        EndpointLogin = "api/UsuarioLogin/autenticacaoGoogleCredential",
                        // GOOGLE_CLIENT_ID é injetado via IOptions<AppSettings> (PostConfigure no Startup).
                        // Client Secret não é usado — fluxo credential valida via tokeninfo, sem troca de código.
                        Login = "",
                        ParametrosLogin = "{" +
                            "\"scope\":\"https://www.googleapis.com/auth/userinfo.email\"," +
                            "\"grant_type\":\"authorization_code\"" +
                            "}",
                        Protocolo = (int)IdentityProviderProtocol.Oauth2,
                        Provedor = (int)IdentityProvider.Google,
                        Senha = ""
                    }
                );
        }
    }
}
