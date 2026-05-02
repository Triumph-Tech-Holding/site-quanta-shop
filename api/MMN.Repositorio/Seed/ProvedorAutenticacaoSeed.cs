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
                        // Client ID e Secret são placeholder — o Startup os substitui a partir
                        // das variáveis de ambiente GOOGLE_CLIENT_ID e GOOGLE_CLIENT_SECRET.
                        // Este registro é para o fluxo OAuth2 legado (autenticacaoGoogle).
                        // O fluxo principal é autenticacaoGoogleCredential (One Tap/GSI).
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
