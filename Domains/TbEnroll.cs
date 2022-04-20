using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbEnroll
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public bool VerificationCodeSent { get; set; }
        public DateTime CreatedAt { get; set; }


        public virtual TbCourse Course { get; set; }
        public virtual TbStudent Student { get; set; }
    }
}
