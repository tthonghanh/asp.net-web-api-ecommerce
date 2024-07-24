using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;

namespace ecommerce.Helpers
{
    public class QueryObject
    {
        public string? ProductName { get; set; }
        public string? SortBy { get; set; }
        public bool IsDecending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}