using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbQuestion
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionRightAnswer { get; set; }
        public int ExamId { get; set; }
        public string FirstChoice { get; set; }
        public string SecondChoice { get; set; }
        public string ThirdChoice { get; set; }
        public string FourthChoice { get; set; }

        public virtual TbExam Exam { get; set; }
    }
}
