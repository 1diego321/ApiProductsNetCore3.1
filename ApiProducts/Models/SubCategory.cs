using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiProducts.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Category Category { get; set; }
        public List<Product> Product { get; set; }
    }
}