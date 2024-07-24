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
        [SwaggerSchema(Description = "Start date of the order period")]
        public DateOnly? DateStart { get; set; }
        [SwaggerSchema(Description = "End date of the order period")]
        public DateOnly? DateEnd { get; set; }
    }
}