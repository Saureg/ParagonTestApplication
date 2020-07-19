namespace ParagonTestApplication.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ParagonTestApplication.Data.Contracts;
    using ParagonTestApplication.Models.Common;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Webinar repository.
    /// </summary>
    public class WebinarRepository : IAllWebinars
    {
        private readonly MainDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Db context.</param>
        public WebinarRepository(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get all webinars.
        /// </summary>
        /// <returns>Webinars.</returns>
        public async Task<IEnumerable<Webinar>> GetAll()
        {
            return await this.dbContext.Webinars.Include(x => x.Series).ToListAsync();
        }

        /// <summary>
        /// Get filtered webinar list.
        /// </summary>
        /// <param name="webinarFilter">Webinar filter.</param>
        /// <param name="paginationFilter">Pafination filter.</param>
        /// <returns>Filtered webinar list.</returns>
        public async Task<PagedList<Webinar>> GetFilteredList(
            WebinarParameters webinarFilter,
            PaginationFilter paginationFilter)
        {
            var queryable = this.dbContext.Webinars.Include(x => x.Series).AsQueryable();

            if (webinarFilter.MinDateTime != null)
            {
                queryable = queryable.Where(x => x.EndDateTime >= webinarFilter.MinDateTime.Value);
            }

            if (webinarFilter.MaxDateTime != null)
            {
                queryable = queryable.Where(x => x.StartDateTime <= webinarFilter.MaxDateTime.Value);
            }

            if (webinarFilter.MinDuration != null)
            {
                queryable = queryable.Where(x => x.Duration >= webinarFilter.MinDuration.Value);
            }

            if (webinarFilter.MaxDuration != null)
            {
                queryable = queryable.Where(x => x.Duration <= webinarFilter.MaxDuration.Value);
            }

            if (webinarFilter.SeriesId != null)
            {
                queryable = queryable.Where(x => x.SeriesId == webinarFilter.SeriesId);
            }

            var webinars = await queryable
                .OrderByDescending(x => x.StartDateTime)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
            return new PagedList<Webinar>(
                webinars,
                queryable.Count(),
                paginationFilter.PageNumber,
                paginationFilter.PageSize);
        }

        /// <summary>
        /// Get webinar.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Webinar.</returns>
        public async Task<Webinar> Get(int id)
        {
            return await this.dbContext.Webinars
                .Include(x => x.Series)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Update webinar.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="webinar">Webinar.</param>
        /// <returns>Updated webinar.</returns>
        public async Task<Webinar> Update(int id, Webinar webinar)
        {
            webinar.Series = await this.CreateSeriesIfNonExist(webinar.Series.Name);

            var currentWebinar = this.Get(id).Result;

            currentWebinar.Name = webinar.Name;
            currentWebinar.StartDateTime = webinar.StartDateTime;
            currentWebinar.Duration = webinar.Duration;
            currentWebinar.Series = webinar.Series;
            currentWebinar.CalculateEndDateTime();

            var updatedWebinar = this.dbContext.Webinars.Attach(currentWebinar);
            this.dbContext.Entry(currentWebinar).State = EntityState.Modified;

            await this.dbContext.SaveChangesAsync();

            return updatedWebinar.Entity;
        }

        /// <summary>
        /// Create webinar.
        /// </summary>
        /// <param name="webinar">Webinar.</param>
        /// <param name="seriesName">Series name.</param>
        /// <returns>Created webinar.</returns>
        public async Task<Webinar> Create(Webinar webinar, string seriesName)
        {
            webinar.Series = await this.CreateSeriesIfNonExist(seriesName);

            webinar.CalculateEndDateTime();

            var createdWebinar = await this.dbContext.Webinars.AddAsync(webinar);
            await this.dbContext.SaveChangesAsync();
            return createdWebinar.Entity;
        }

        /// <summary>
        /// Delete webinar.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Deleted webinar.</returns>
        public async Task<Webinar> Delete(int id)
        {
            var webinar = await this.Get(id);

            if (webinar == null)
            {
                return null;
            }

            this.dbContext.Webinars.Remove(webinar);

            await this.dbContext.SaveChangesAsync();
            return webinar;
        }

        private async Task<Series> CreateSeriesIfNonExist(string seriesName)
        {
            var currentSeries = await this.dbContext.Series
                .FirstOrDefaultAsync(x => x.Name == seriesName);

            if (currentSeries != null)
            {
                return currentSeries;
            }

            var series = await this.dbContext.Series.AddAsync(new Series { Name = seriesName });
            await this.dbContext.SaveChangesAsync();
            currentSeries = series.Entity;

            return currentSeries;
        }
    }
}