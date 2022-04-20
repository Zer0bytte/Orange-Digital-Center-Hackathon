using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbCourse
    {
        public TbCourse()
        {
            TbEnrolls = new HashSet<TbEnroll>();
            TbExams = new HashSet<TbExam>();
            TbTeachings = new HashSet<TbTeaching>();
            TbVerificationCodes = new HashSet<TbVerificationCode>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TbCategroie Category { get; set; }
        public virtual ICollection<TbEnroll> TbEnrolls { get; set; }
        public virtual ICollection<TbExam> TbExams { get; set; }
        public virtual ICollection<TbTeaching> TbTeachings { get; set; }
        public virtual ICollection<TbVerificationCode> TbVerificationCodes { get; set; }
    }
}
