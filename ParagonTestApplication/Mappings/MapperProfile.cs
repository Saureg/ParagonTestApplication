namespace ParagonTestApplication.Mappings
{
    using AutoMapper;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Mapper profile.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfile"/> class.
        /// </summary>
        public MapperProfile()
        {
            this.CreateMap<Series, SeriesDto>();
            this.CreateMap<SeriesDto, Series>();
            this.CreateMap<Webinar, WebinarDto>();
            this.CreateMap<PagedList<Webinar>, PagedList<WebinarDto>>();
            this.CreateMap<SeriesDto, Series>();
            this.CreateMap<WebinarDto, Webinar>();
            this.CreateMap<CreateOrUpdateSeriesRequest, Series>();
            this.CreateMap<CreateOrUpdateWebinarRequest, Webinar>();
            this.CreateMap<WebinarFilter, WebinarParameters>();
        }
    }
}