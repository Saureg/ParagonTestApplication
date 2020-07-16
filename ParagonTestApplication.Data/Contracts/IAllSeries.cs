using System.Collections.Generic;
using System.Threading.Tasks;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.Data.Contracts
{
    public interface IAllSeries
    {
        Task<IEnumerable<Series>> GetAll();
    }
}