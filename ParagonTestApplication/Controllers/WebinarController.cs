using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<WebinarController> _logger;

        public WebinarController(IMapper mapper, ILogger<WebinarController> logger, IAllWebinars allWebinars)
        {
            _mapper = mapper;
            _logger = logger;
            _allWebinars = allWebinars;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<WebinarDto>>> Get([FromQuery] WebinarFilter webinarFilter,
            [FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                var validator = new WebinarFilterValidator();
                var results = validator.ValidateAsync(webinarFilter).Result;

                if (!results.IsValid)
                    return BadRequest(new Response<WebinarDto>(HttpStatusCode.BadRequest, null, results.ToString()));

                var webinarParameters = _mapper.Map<WebinarParameters>(webinarFilter);
                var webinars = await _allWebinars.GetFilteredList(webinarParameters, paginationFilter);
                var webinarResult = _mapper.Map<PagedList<Webinar>, PagedList<WebinarDto>>(webinars);

                return Ok(new Response<PagedList<WebinarDto>>(HttpStatusCode.OK, webinarResult));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WebinarDto>> Get(int id)
        {
            var webinar = await _allWebinars.Get(id);

            if (webinar == null)
                return NotFound(new Response<WebinarDto>(HttpStatusCode.NotFound,
                    null, $"Webinar with id={id} not found"));

            var webinarDto = _mapper.Map<WebinarDto>(webinar);
            return Ok(new Response<WebinarDto>(HttpStatusCode.OK, webinarDto, null));
        }

        /// <summary>
        ///     Create new webinar
        /// </summary>
        /// <param name="createOrUpdateWebinarRequest">Request model for new webinar</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public async Task<ActionResult<WebinarDto>> Create(CreateOrUpdateWebinarRequest createOrUpdateWebinarRequest)
        {
            var validator = new CreateOrUpdateWebinarRequestValidator(await _allWebinars.GetAll());
            var results = validator.ValidateAsync(createOrUpdateWebinarRequest).Result;

            if (!results.IsValid)
                return BadRequest(new Response<WebinarDto>(HttpStatusCode.BadRequest, null, results.ToString()));

            var webinar = _mapper.Map<Webinar>(createOrUpdateWebinarRequest);
            webinar.EndDateTime = webinar.StartDateTime.AddMinutes(5);
            var createdWebinar = await _allWebinars.Create(webinar, createOrUpdateWebinarRequest.Series.Name);
            var createdWebinarDto = _mapper.Map<WebinarDto>(createdWebinar);

            return CreatedAtAction(
                nameof(Get),
                new {id = createdWebinarDto.Id},
                new Response<WebinarDto>(HttpStatusCode.Created, createdWebinarDto, null)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
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

            var updatedWebinar = _mapper.Map<Webinar>(createOrUpdateWebinarRequest);
            var updateWebinar = await _allWebinars.Update(id, updatedWebinar);

            return CreatedAtAction(
                nameof(Get),
                new {id = updateWebinar.Id},
                new Response<Webinar>(HttpStatusCode.Created, updateWebinar, null)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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