using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParagonTestApplication.Data.Contracts;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.Data.Repositories
{
    public class SeriesRepository : IAllSeries
    {
        private readonly MainDbContext _dbContext;

        public SeriesRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Series>> GetAll()
        {
            return await _dbContext.Series.ToListAsync();
        }
    }
}