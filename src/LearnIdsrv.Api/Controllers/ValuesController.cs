using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnIdsrv.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("GetData")]
        public ActionResult<IEnumerable<string>> Get()
        {
            var claims = HttpContext.User.Claims.Select(x => $"{x.Type}:{x.Value}");
            return Ok(new { Name = "API", Claims = claims.ToArray()});
        }
    }
}
