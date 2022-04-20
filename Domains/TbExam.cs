using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbExam
    {
        public TbExam()
        {
            TbQuestions = new HashSet<TbQuestion>();
            TbRevisions = new HashSet<TbRevision>();
        }

        public int ExamId { get; set; }
        public int CourseId { get; set; }
        public string ExamName { get; set; }

        public virtual TbCourse Course { get; set; }
        public virtual ICollection<TbQuestion> TbQuestions { get; set; }
        public virtual ICollection<TbRevision> TbRevisions { get; set; }
    }
}
