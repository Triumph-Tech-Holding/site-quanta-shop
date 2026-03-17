using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Dominio.ViewModel;
using MMN.Util.Translation;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MMN.Util.Model;
using MMN.Util.Extensions;
using MMN.Dominio.Excecao;
using System.Globalization;

namespace MMN.Api.Helpers
{
    public class TokenUtil : ITokenUtil
    {
        private readonly AppSettings _appSettings;
        private readonly ILocation _location;
        public TokenUtil(IOptions<AppSettings> appSettings, ILocation location)
        {
            _appSettings = appSettings.Value;
            _location = location;
        }
        public string ConstruirToken(UsuarioViewModel userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Exp", DateTime.UtcNow.HorarioBrasilia().AddYears(1).ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                signingCredentials: creds);

            var webToken = System.Net.WebUtility.UrlEncode(new JwtSecurityTokenHandler().WriteToken(token));
            //AbrirToken(webToken);
            return webToken;
        }

        public TokenViewModel ValidarToken(string token)
        {
            try
            {
                var webToken = System.Net.WebUtility.UrlDecode(token);
                var stream = webToken;
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

                var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;
                var idUsuario = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;

                var dataExpira = tokenS.Claims.FirstOrDefault(claim => claim.Type == "exp" || claim.Type == "Exp").Value;

                if (long.TryParse(dataExpira, out long timestamp))
                {
                    DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                    DateTime dateTime = unixEpoch.AddSeconds(timestamp);

                    if (DateTime.UtcNow.HorarioBrasilia() > dateTime)
                    {
                        throw new UnauthorizedException("senha_alterar_token_expirado");
                    }

                    return new TokenViewModel()
                    {
                        IdUsuario = idUsuario,
                        Jti = jti,
                        DataExpira = dateTime
                    };
                }
                else if (DateTime.TryParseExact(dataExpira, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    if (DateTime.UtcNow.HorarioBrasilia() > dateTime)
                    {
                        throw new UnauthorizedException("senha_alterar_token_expirado");
                    }

                    return new TokenViewModel()
                    {
                        IdUsuario = idUsuario,
                        Jti = jti,
                        DataExpira = dateTime
                    };
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new PadraoException("token_erro");
            }

        }
    }
}
