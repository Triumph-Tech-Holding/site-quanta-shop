using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Util.Translation;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LancamentoController : ControllerBase
    {
        private readonly ILocation _location;
        public LancamentoController(ILocation location)
        {
            _location = location;
        }

    }
}