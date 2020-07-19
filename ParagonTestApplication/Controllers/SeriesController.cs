namespace ParagonTestApplication.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Models.ApiModels.Common;
    using ParagonTestApplication.Models.ApiModels.Series;

    /// <summary>
    /// Series controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAllSeries allSeries;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="allSeries">Series.</param>
        public SeriesController(IMapper mapper, IAllSeries allSeries)
        {
            this.mapper = mapper;
            this.allSeries = allSeries;
        }

        /// <summary>
        ///     Get all webinar's series.
        /// </summary>
        /// <returns>Webinar's series.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<SeriesDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IEnumerable<SeriesDto>>>> GetSeriesList()
        {
            var series = await this.allSeries.GetAll();
            var seriesResult = this.mapper.Map<IEnumerable<SeriesDto>>(series);

            return this.Ok(new Response<IEnumerable<SeriesDto>>(HttpStatusCode.OK, seriesResult));
        }
    }
}