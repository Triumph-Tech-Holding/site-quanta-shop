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
                        // ATENÇÃO: Este registro é para o fluxo OAuth2 Authorization Code (legado).
                        // O fluxo principal é autenticacaoGoogleCredential (One Tap/GSI) que valida
                        // o ID token via Google tokeninfo API — não usa Login/Senha deste registro.
                        // O Client ID abaixo deve coincidir com GOOGLE_CLIENT_ID no ambiente.
                        // Para atualizar em produção: UPDATE "ProvedorAutenticacao" SET "Login" = '<client_id>', "Senha" = '<client_secret>' WHERE "IdProvedorAutenticacao" = -1;
                        Login = "372294010028-ff1frn14fg81mn0ujhv215lk9rd5t80r.apps.googleusercontent.com",
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
