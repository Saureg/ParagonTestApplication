namespace ParagonTestApplication.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Series interface.
    /// </summary>
    public interface IAllSeries
    {
        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<Series>> GetAll();
    }
}