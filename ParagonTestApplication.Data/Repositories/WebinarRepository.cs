using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParagonTestApplication.Data.Contracts;
using ParagonTestApplication.Models.Common;
using ParagonTestApplication.Models.DataModels;

namespace ParagonTestApplication.Data.Repositories
{
    public class WebinarRepository : IAllWebinars
    {
        private readonly MainDbContext _dbContext;

        public WebinarRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Webinar>> GetAll()
        {
            return await _dbContext.Webinars.Include(x => x.Series).ToListAsync();
        }

        public async Task<PagedList<Webinar>> GetFilteredList(WebinarParameters webinarFilter,
            PaginationFilter paginationFilter)
        {
            var queryable = _dbContext.Webinars.Include(x => x.Series).AsQueryable();

            if (webinarFilter.MinDateTime != null)
                queryable = queryable.Where(x =>
                    x.StartDateTime <= webinarFilter.MaxDateTime.Value);
            if (webinarFilter.MaxDateTime != null)
                queryable = queryable.Where(x => x.EndDateTime >= webinarFilter.MinDateTime.Value);
            if (webinarFilter.MinDuration != null)
                queryable = queryable.Where(x => x.Duration <= webinarFilter.MaxDuration.Value);
            if (webinarFilter.MaxDuration != null)
                queryable = queryable.Where(x => x.Duration >= webinarFilter.MinDuration.Value);
            if (webinarFilter.SeriesId != null) queryable = queryable.Where(x => x.SeriesId == webinarFilter.SeriesId);

            var webinars = await queryable
                .OrderByDescending(x => x.StartDateTime)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToListAsync();
            return new PagedList<Webinar>(webinars, queryable.Count(), paginationFilter.PageNumber,
                paginationFilter.PageSize);
        }

        public async Task<Webinar> Get(int id)
        {
            return await _dbContext.Webinars
                .Include(x => x.Series)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Webinar> Update(int id, Webinar webinar)
        {
            webinar.Series = await CreateSeriesIfNonExist(webinar.Series.Name);

            var currentWebinar = Get(id).Result;

            currentWebinar.Name = webinar.Name;
            currentWebinar.StartDateTime = webinar.StartDateTime;
            currentWebinar.Duration = webinar.Duration;
            currentWebinar.Series = webinar.Series;
            currentWebinar.CalculateEndDateTime();

            var updatedWebinar = _dbContext.Webinars.Attach(currentWebinar);
            _dbContext.Entry(currentWebinar).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return updatedWebinar.Entity;
        }

        public async Task<Webinar> Create(Webinar webinar, string seriesName)
        {
            webinar.Series = await CreateSeriesIfNonExist(seriesName);

            webinar.CalculateEndDateTime();

            var createdWebinar = await _dbContext.Webinars.AddAsync(webinar);
            await _dbContext.SaveChangesAsync();
            return createdWebinar.Entity;
        }

        public async Task<Webinar> Delete(int id)
        {
            var webinar = await Get(id);

            if (webinar == null) return null;

            _dbContext.Webinars.Remove(webinar);

            await _dbContext.SaveChangesAsync();
            return webinar;
        }

        private async Task<Series> CreateSeriesIfNonExist(string seriesName)
        {
            var currentSeries = await _dbContext.Series
                .FirstOrDefaultAsync(x => x.Name == seriesName);

            if (currentSeries != null) return currentSeries;

            var series = await _dbContext.Series.AddAsync(new Series
            {
                Name = seriesName
            });
            await _dbContext.SaveChangesAsync();
            currentSeries = series.Entity;

            return currentSeries;
        }
    }
}