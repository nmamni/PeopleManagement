using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Common.CommonModels
{
   public class FilterModel<T>
    {
        public PageRequest PageRequest { get; set; }
        public T Filter { get; set; }

        public FilterModel()
        {
            PageRequest = new PageRequest
            {
                Index = 0,
                ShowPerPage = 10,
                SortingColumn = "ID"
            };
        }
    }

    public class PageRequest
    {
        public int Index { get; set; }
        public int ShowPerPage { get; set; }
        public string SortingColumn { get; set; }
    }
}
