using prject.domain.Models;
using project.domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace project.domain.Models
{
    public class ServiceUser
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public Guid ServiceId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser Teachers { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}
