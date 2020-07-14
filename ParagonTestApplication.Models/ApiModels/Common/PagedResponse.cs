using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ParagonTestApplication.Models.Common;

namespace ParagonTestApplication.Models.ApiModels.Common
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, HttpStatusCode statusCode, string message) : base(data, statusCode, message)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            
            
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }
        
        // public PagedResponse(T data, PaginationParameters paginationParameters, int totalCount)
        // {
        //     var response = new Response<T>(data);
        //     PageNumber = paginationParameters.PageNumber;
        //     PageSize = paginationParameters.PageNumber;
        //     Data = data;
        //     Message = null;
        // }

        public static PagedResponse<T> ToPagedResponse(T data, PaginationFilter paginationFilter,
            int totalRecords, HttpStatusCode statusCode, string message)
        {
            var response = new PagedResponse<T>(data, paginationFilter.PageNumber, paginationFilter.PageSize,
                statusCode, message);
            var totalPages = ((double)totalRecords / (double)paginationFilter.PageSize);
            var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            response.HasNextPage = paginationFilter.PageNumber < roundedTotalPages;
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}