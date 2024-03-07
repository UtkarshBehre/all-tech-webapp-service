using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace all_tech_webapp_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("ping")]
        public IActionResult Ping()
        {
            return this.Ok("pong");
        }

        [HttpGet]
        [Route("getConflict")]
        public IActionResult GetConflict()
        {
            return this.Conflict("tis conflict");
        }

        [HttpGet]
        [Route("getBadRequest")]
        public IActionResult GetBadRequest()
        {
            return this.BadRequest("400 bad request");
        }

        [HttpPost]
        [Route("getUnauthorized")]
        public IActionResult GetUnauthorized()
        {
            return this.Unauthorized("401 unauthorized");
        }

        [HttpGet]
        [Route("getForbidden")]
        public IActionResult getForbidden()
        {
            return StatusCode((int) HttpStatusCode.Forbidden);
        }

        [HttpGet]
        [Route("getInternalServerError")]
        public IActionResult GetInternalServerError()
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "500 internal server error");
        }

    }
}
