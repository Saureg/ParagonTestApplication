using System.Collections.Generic;
using System.Threading.Tasks;
using ParagonTestApplication.Models.Common;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.Data.Contracts
{
    public interface IAllWebinars
    {
        Task<IEnumerable<Webinar>> GetAll();

        Task<PagedList<Webinar>> GetFilteredList(WebinarParameters webinarFilter, PaginationFilter paginationFilter);

        Task<Webinar> Get(int id);

        Task<Webinar> Update(int id, Webinar webinar);

        Task<Webinar> Create(Webinar webinar, string seriesName);

        Task<Webinar> Delete(int id);
    }
}