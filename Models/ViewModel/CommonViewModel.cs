using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class PageListViewModel
    {
        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }
    }

    public class SearchModel
    {
        public SearchModel()
        {
            this.PageSize = 10;
            this.CurrentPage = 1;
        }
        public SearchModel(int pageSize, int currentPage)
        {
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
        }

        public string? Keyword { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
    }

    public class SaveChangesResult
    {
        public SaveChangesResult(bool isSuccess, string? message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public bool IsSuccess { get; set; }

        public string? Message { get; set; }
    }
}
