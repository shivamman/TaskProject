using System;
using System.Collections.Generic;
using System.Text;

namespace project.domain.Models
{
    public abstract class Base
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
