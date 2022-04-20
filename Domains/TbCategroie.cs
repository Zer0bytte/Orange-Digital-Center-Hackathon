using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbCategroie
    {
        public TbCategroie()
        {
            TbCourses = new HashSet<TbCourse>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<TbCourse> TbCourses { get; set; }
    }
}
