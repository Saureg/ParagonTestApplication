using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParagonTestApplication.Data.Contracts;
using ParagonTestApplication.Models.ApiModels.Common;
using ParagonTestApplication.Models.ApiModels.Series;

namespace ParagonTestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAllSeries _allSeries;

        public SeriesController(IMapper mapper, IAllSeries allSeries)
        {
            _mapper = mapper;
            _allSeries = allSeries;
        }

        /// <summary>
        ///     Get all webinar's series
        /// </summary>
        /// <returns>Webinar's series</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<SeriesDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IEnumerable<SeriesDto>>>> GetSeriesList()
        {
            var series = await _allSeries.GetAll();
            var seriesResult = _mapper.Map<IEnumerable<SeriesDto>>(series);

            return Ok(new Response<IEnumerable<SeriesDto>>(HttpStatusCode.OK, seriesResult));
        }
    }
}