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

namespace ParagonTestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebinarController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAllWebinars _allWebinars;

        public WebinarController(IMapper mapper, IAllWebinars allWebinars)
        {
            _mapper = mapper;
            _allWebinars = allWebinars;
        }

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
                return BadRequest(
                    new Response<PagedList<WebinarDto>>(HttpStatusCode.BadRequest, null, validationResultString));

            var webinarParameters = _mapper.Map<WebinarParameters>(webinarFilter);
            var webinars = await _allWebinars.GetFilteredList(webinarParameters, paginationFilter);
            var webinarResult = _mapper.Map<PagedList<Webinar>, PagedList<WebinarDto>>(webinars);

            return Ok(new Response<PagedList<WebinarDto>>(HttpStatusCode.OK, webinarResult));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<WebinarDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<WebinarDto>>> GetWebinar(int id)
        {
            var webinar = await _allWebinars.Get(id);

            if (webinar == null)
                return NotFound(new Response<WebinarDto>(HttpStatusCode.NotFound,
                    null, $"Webinar with id={id} not found"));

            var webinarDto = _mapper.Map<WebinarDto>(webinar);
            return Ok(new Response<WebinarDto>(HttpStatusCode.OK, webinarDto));
        }

        /// <summary>
        ///     Create new webinar
        /// </summary>
        /// <param name="createOrUpdateWebinarRequest">Request model for new webinar</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<WebinarDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WebinarDto>> CreateWebinar(
            CreateOrUpdateWebinarRequest createOrUpdateWebinarRequest)
        {
            var validator = new CreateOrUpdateWebinarRequestValidator(await _allWebinars.GetAll());
            var results = validator.ValidateAsync(createOrUpdateWebinarRequest).Result;

            if (!results.IsValid)
                return BadRequest(new Response<WebinarDto>(HttpStatusCode.BadRequest, null, results.ToString()));

            var webinar = _mapper.Map<Webinar>(createOrUpdateWebinarRequest);
            var createdWebinar = await _allWebinars.Create(webinar, createOrUpdateWebinarRequest.Series.Name);
            var createdWebinarDto = _mapper.Map<WebinarDto>(createdWebinar);

            return CreatedAtAction(
                nameof(GetWebinar),
                new {id = createdWebinarDto.Id},
                new Response<WebinarDto>(HttpStatusCode.Created, createdWebinarDto)
            );
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<WebinarDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<WebinarDto>>> UpdateWebinar(int id,
            [FromBody] CreateOrUpdateWebinarRequest createOrUpdateWebinarRequest)
        {
            var validator = new CreateOrUpdateWebinarRequestValidator(await _allWebinars.GetAll(), id);
            var results = await validator.ValidateAsync(createOrUpdateWebinarRequest);

            if (!results.IsValid)
                return BadRequest(new Response<WebinarDto>(HttpStatusCode.BadRequest, null, results.ToString()));

            var webinar = await _allWebinars.Get(id);
            if (webinar == null)
                return NotFound(new Response<WebinarDto>(HttpStatusCode.NotFound,
                    null, $"Webinar with id={id} not found"));

            var webinarForUpdate = _mapper.Map<Webinar>(createOrUpdateWebinarRequest);
            var updatedWebinar = await _allWebinars.Update(id, webinarForUpdate);
            var updatedWebinarDto = _mapper.Map<WebinarDto>(updatedWebinar);

            return CreatedAtAction(
                nameof(GetWebinar),
                new {id = updatedWebinarDto.Id},
                new Response<WebinarDto>(HttpStatusCode.Created, updatedWebinarDto)
            );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response<>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWebinar(int id)
        {
            var webinar = await _allWebinars.Get(id);

            if (webinar == null)
                return NotFound(new Response<WebinarDto>(HttpStatusCode.NotFound,
                    null, $"Webinar with id={id} not found"));
            await _allWebinars.Delete(id);
            return NoContent();
        }
    }
}