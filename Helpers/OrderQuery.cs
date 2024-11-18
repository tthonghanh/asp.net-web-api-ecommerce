using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Swashbuckle.AspNetCore.Annotations;

namespace ecommerce.Helpers
{
    public class OrderQuery
    {
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
        public int? PageSize { get; set; } = 10;
        public int? PageNumber { get; set; } = 1;
    }
}