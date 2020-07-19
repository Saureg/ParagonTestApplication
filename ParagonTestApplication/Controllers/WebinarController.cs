namespace ParagonTestApplication.Controllers
{
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Models.ApiModels.Common;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;
    using ParagonTestApplication.Models.DataModels;
    using ParagonTestApplication.Models.Validators;

    /// <summary>
    /// Webinar controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WebinarController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAllWebinars allWebinars;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="allWebinars">Webinars.</param>
        public WebinarController(IMapper mapper, IAllWebinars allWebinars)
        {
            this.mapper = mapper;
            this.allWebinars = allWebinars;
        }

        /// <summary>
        ///     Get a list of webinars.
        /// </summary>
        /// <param name="webinarFilter">Webinar filtering options.</param>
        /// <param name="paginationFilter">Pagination options.</param>
        /// <returns>List of webinars.</returns>
        /// <response code="200">Returns filtered items.</response>
        /// <response code="400">If filters is invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(Response<PagedList<WebinarDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<PagedList<WebinarDto>>>> GetWebinarList(
            [FromQuery] WebinarFilter webinarFilter,
            [FromQuery] PaginationFilter paginationFilter)
        {
            var webinarFilterValidator = new WebinarFilterValidator();
            var webinarFiltersValidationResults = webinarFilterValidator.ValidateAsync(webinarFilter).Result;
            var paginationFilterValidator = new PaginationFilterValidator();
            var paginationFiltersValidationResults = paginationFilterValidator.ValidateAsync(paginationFilter).Result;
            var validationResultString = $"{webinarFiltersValidationResults}{paginationFiltersValidationResults}";
            if (!webinarFiltersValidationResults.IsValid | !paginationFiltersValidationResults.IsValid)
            {
                return this.BadRequest(
                    new Response<PagedList<WebinarDto>>(HttpStatusCode.BadRequest, null, validationResultString));
            }

            var webinarParameters = this.mapper.Map<WebinarParameters>(webinarFilter);
            var webinars = await this.allWebinars.GetFilteredList(webinarParameters, paginationFilter);
            var webinarResult = this.mapper.Map<PagedList<Webinar>, PagedList<WebinarDto>>(webinars);

            return this.Ok(new Response<PagedList<WebinarDto>>(HttpStatusCode.OK, webinarResult));
        }

        /// <summary>
        ///     Get a specific webinar.
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <returns>Webinar.</returns>
        /// <response code="200">Returns webinar.</response>
        /// <response code="404">Webinar not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<WebinarDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<WebinarDto>>> GetWebinar(int id)
        {
            var webinar = await this.allWebinars.Get(id);

            if (webinar == null)
            {
                return this.NotFound(new Response<WebinarDto>(
                    HttpStatusCode.NotFound,
                    null,
                    $"Webinar with id={id} not found"));
            }

            var webinarDto = this.mapper.Map<WebinarDto>(webinar);
            return this.Ok(new Response<WebinarDto>(HttpStatusCode.OK, webinarDto));
        }

        /// <summary>
        ///     Create a new webinar.
        /// </summary>
        /// <param name="createOrUpdateWebinarRequest">Parameters for creating a new webinar.</param>
        /// <returns>New webinar.</returns>
        /// <response code="201">Returns a newly created webinar.</response>
        /// <response code="400">If parameters is invalid.</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<WebinarDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WebinarDto>> CreateWebinar(
            CreateOrUpdateWebinarRequest createOrUpdateWebinarRequest)
        {
            var validator = new CreateOrUpdateWebinarRequestValidator(await this.allWebinars.GetAll());
            var results = validator.ValidateAsync(createOrUpdateWebinarRequest).Result;

            if (!results.IsValid)
            {
                return this.BadRequest(new Response<WebinarDto>(HttpStatusCode.BadRequest, null, results.ToString()));
            }

            var webinar = this.mapper.Map<Webinar>(createOrUpdateWebinarRequest);
            var createdWebinar = await this.allWebinars.Create(webinar, createOrUpdateWebinarRequest.Series.Name);
            var createdWebinarDto = this.mapper.Map<WebinarDto>(createdWebinar);

            return this.CreatedAtAction(
                nameof(this.GetWebinar),
                new { id = createdWebinarDto.Id },
                new Response<WebinarDto>(HttpStatusCode.Created, createdWebinarDto));
        }

        /// <summary>
        ///     Update a specific webinar.
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <param name="createOrUpdateWebinarRequest">Parameters for updating a specific webinar.</param>
        /// <returns>Updated webinar.</returns>
        /// <response code="201">Returns a updated webinar.</response>
        /// <response code="400">If parameters is invalid.</response>
        /// <response code="404">Webinar not found.</response>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<WebinarDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<WebinarDto>>> UpdateWebinar(
            int id,
            [FromBody] CreateOrUpdateWebinarRequest createOrUpdateWebinarRequest)
        {
            var validator = new CreateOrUpdateWebinarRequestValidator(await this.allWebinars.GetAll(), id);
            var results = await validator.ValidateAsync(createOrUpdateWebinarRequest);

            if (!results.IsValid)
            {
                return this.BadRequest(new Response<WebinarDto>(HttpStatusCode.BadRequest, null, results.ToString()));
            }

            var webinar = await this.allWebinars.Get(id);
            if (webinar == null)
            {
                return this.NotFound(new Response<WebinarDto>(
                    HttpStatusCode.NotFound,
                    null,
                    $"Webinar with id={id} not found"));
            }

            var webinarForUpdate = this.mapper.Map<Webinar>(createOrUpdateWebinarRequest);
            var updatedWebinar = await this.allWebinars.Update(id, webinarForUpdate);
            var updatedWebinarDto = this.mapper.Map<WebinarDto>(updatedWebinar);

            return this.CreatedAtAction(
                nameof(this.GetWebinar),
                new { id = updatedWebinarDto.Id },
                new Response<WebinarDto>(HttpStatusCode.Created, updatedWebinarDto));
        }

        /// <summary>
        ///     Delete a specific webinar.
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <response code="204">Webinar deleted.</response>
        /// <response code="404">Webinar not found.</response>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWebinar(int id)
        {
            var webinar = await this.allWebinars.Get(id);

            if (webinar == null)
            {
                return this.NotFound(new Response<WebinarDto>(
                    HttpStatusCode.NotFound,
                    null,
                    $"Webinar with id={id} not found"));
            }

            await this.allWebinars.Delete(id);
            return this.NoContent();
        }
    }
}