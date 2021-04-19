using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.Category.Request
{
    public class CategoryAddRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
