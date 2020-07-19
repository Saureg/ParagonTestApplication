namespace ParagonTestApplication.Models.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Paged list model.
    /// </summary>
    /// <typeparam name="T">Data.</typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// Gets or sets current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets total count.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets or sets has previous option.
        /// </summary>
        public bool HasPrevious => this.CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether has next.
        /// </summary>
        public bool HasNext => this.CurrentPage < this.TotalPages;

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        public PagedList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">Items.</param>
        /// <param name="count">Count.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.Items = items;
        }
    }
}