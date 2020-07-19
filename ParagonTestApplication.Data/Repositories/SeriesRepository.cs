namespace ParagonTestApplication.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Series repository.
    /// </summary>
    public class SeriesRepository : IAllSeries
    {
        private readonly MainDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Db context.</param>
        public SeriesRepository(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns>Series.</returns>
        public async Task<IEnumerable<Series>> GetAll()
        {
            return await this.dbContext.Series.ToListAsync();
        }
    }
}