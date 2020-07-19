namespace ParagonTestApplication.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ParagonTestApplication.Models.Common;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Webinars interface.
    /// </summary>
    public interface IAllWebinars
    {
        /// <summary>
        /// Get all webinars.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<Webinar>> GetAll();

        /// <summary>
        /// Get filtered webinar list.
        /// </summary>
        /// <param name="webinarFilter">Webinar filter.</param>
        /// <param name="paginationFilter">Pagination filter.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<PagedList<Webinar>> GetFilteredList(WebinarParameters webinarFilter, PaginationFilter paginationFilter);

        /// <summary>
        /// Get webinar.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<Webinar> Get(int id);

        /// <summary>
        /// Update webinar.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="webinar">Webinar.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<Webinar> Update(int id, Webinar webinar);

        /// <summary>
        /// Create webinar.
        /// </summary>
        /// <param name="webinar">Webinar.</param>
        /// <param name="seriesName">Series name.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<Webinar> Create(Webinar webinar, string seriesName);

        /// <summary>
        /// Delete webinar.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<Webinar> Delete(int id);
    }
}