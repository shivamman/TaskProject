using prject.domain.Models;
using project.domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace project.domain.Models
{
    public class ProductUser
    {
        public Guid Id { get; set; }
        public bool IsPaid { get; set; }

        public Guid AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUsers { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
