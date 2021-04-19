using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Stock { get; set; }

        public int SubCategoryId { get; set; }

        public int ProductStatusId { get; set; }
    }
}
