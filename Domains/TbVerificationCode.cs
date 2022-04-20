using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbVerificationCode
    {
        public int Id { get; set; }
        public int VerificationCode { get; set; }
        public int CourseId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime EndDate { get; set; }

        public virtual TbCourse Course { get; set; }
    }
}
