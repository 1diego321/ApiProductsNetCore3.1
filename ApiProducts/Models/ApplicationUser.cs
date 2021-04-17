using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [StringLength(128)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [StringLength(128)]
        public byte[] PasswordSalt { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int ApplicationUserStatusId { get; set; }

        [ForeignKey(nameof(ApplicationUserStatusId))]
        public ApplicationUserStatus ApplicationUserStatus { get; set; }
    }
}
