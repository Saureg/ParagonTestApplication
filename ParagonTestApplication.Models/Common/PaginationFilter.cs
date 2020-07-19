namespace ParagonTestApplication.Models.Common
{
    /// <summary>
    /// Pagination filter model.
    /// </summary>
    public class PaginationFilter
    {
        /// <summary>
        /// Gets or sets page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 50;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 50 ? 50 : pageSize;
        }
    }
}