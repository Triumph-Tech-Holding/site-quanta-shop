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
                        UrlApi = "https://oauth2.googleapis.com/token",
                        EndpointCadastro = "api/user/registrarGoogle",
                        EndpointLogin = "api/UsuarioLogin/autenticacaoGoogle",
                        Login = "123493812146-gdjfhkeguuon50kjhhd6i3hgf4v172el.apps.googleusercontent.com",
                        ParametrosLogin = "{" +
                            "\"scope\":\"https://www.googleapis.com/auth/userinfo.email\"," +
                            "\"grant_type\":\"authorization_code\"" +
                            "}",
                        Protocolo = (int)IdentityProviderProtocol.Oauth2,
                        Provedor = (int)IdentityProvider.Google,
                        Senha = "69fWzUHFrkSE1HIfS8smM-Z-"
                    }
                );
        }
    }
}
