using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbStudent
    {
        public TbStudent()
        {
            TbEnrolls = new HashSet<TbEnroll>();
            TbRevisions = new HashSet<TbRevision>();
        }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPhone { get; set; }
        public string StudentCollege { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        public virtual ICollection<TbEnroll> TbEnrolls { get; set; }
        public virtual ICollection<TbRevision> TbRevisions { get; set; }
    }
}
