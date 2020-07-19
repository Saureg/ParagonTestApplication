namespace ParagonTestApplication.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ParagonTestApplication.Models.ApiModels.Common;

    /// <summary>
    /// Controller for error handling.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Get error response.
        /// </summary>
        /// <returns>Error message.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status500InternalServerError)]
        public ActionResult<Response<object>> Get()
        {
            return this.StatusCode(
                500,
                new Response<object>(HttpStatusCode.InternalServerError, null, "Internal server error"));
        }
    }
}