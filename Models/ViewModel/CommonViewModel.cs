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

        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }

        public bool HasFirst { get; set; }

        public bool HasLast { get; set; }
    }
}
