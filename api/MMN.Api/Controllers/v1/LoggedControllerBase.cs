using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace MMN.Api.Controllers.v1
{
    public class LoggedControllerBase : ControllerBase
    {
        protected Guid IdUsuarioLogado
        {
            get
            {
                Guid.TryParse(User.Identity.Name, out var id);

                return id;
            }
        }

        protected int IdGrupoLogado
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.PrimarySid)).Value);
            }
        }

        protected int IdGraduacaoLogado
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                return Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.PrimaryGroupSid)).Value);
            }
        }

        protected string ZanoxConnectId
        {
            get
            {
                return "60FCF3F4B33B72C89C73";
            }
        }
    }
}
