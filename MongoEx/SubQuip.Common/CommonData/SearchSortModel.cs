using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SubQuip.Common.Enums;

namespace SubQuip.Common.CommonData
{
    public class SearchSortModel
    {
        public string SearchString { get; set; }

        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Desc;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public dynamic SearchResult { get; set; }
    }
}
