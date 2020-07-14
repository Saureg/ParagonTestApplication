using AutoMapper;
using ParagonTestApplication.Models.ApiModels.Series;
using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.Common;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Series, SeriesDto>();
            CreateMap<SeriesDto, Series>();
            CreateMap<Webinar, WebinarDto>();
            CreateMap<PagedList<Webinar>, PagedList<WebinarDto>>();
            CreateMap<SeriesDto, Series>();
            CreateMap<WebinarDto, Webinar>();
            CreateMap<CreateOrUpdateSeriesRequest, Series>();
            CreateMap<CreateOrUpdateWebinarRequest, Webinar>();
            CreateMap<WebinarFilter, WebinarParameters>();
        }
    }
}