using System;
using System.Collections.Generic;
using System.Linq;

namespace ParagonTestApplication.Models.Common
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
 
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        // public bool HasPrevious { get; set; }
        // public bool HasNext { get; set; }

        public List<T> Items { get; set; }
        
        public PagedList()
        {
        }
        
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;

            //AddRange(items);
        }
 
        // public static PagedList<T> ToPagedList(List<T> source, int totalCount, int pageNumber, int pageSize)
        // {
        //     var count = totalCount;
        //     var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //
        //     return new PagedList<T>(source, count, pageNumber, pageSize);
        // }
    }
}