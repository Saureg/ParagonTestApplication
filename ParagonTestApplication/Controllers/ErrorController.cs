using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParagonTestApplication.Models.ApiModels.Common;

namespace ParagonTestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<object>> Get()
        {
            return StatusCode(500,
                new Response<object>(HttpStatusCode.InternalServerError, null, "Internal server error"));
        }
    }
}