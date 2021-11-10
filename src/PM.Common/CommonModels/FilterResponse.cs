using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Common.CommonModels
{
    public class FilterResponse<T>
    {
        public FilterResponse(T items, int totalItemsQuantity)
        {
            Items = items;
            TotalItemsQuantity = totalItemsQuantity;
        }
        public int TotalItemsQuantity { get; set; }
        public T Items { get; set; }
    }
}
