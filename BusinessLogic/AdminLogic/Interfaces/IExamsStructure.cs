using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic
{
   public interface IExamsStructure
    {

        public TbExam GetExamById(int ExamId);
        public TbExam CreateExam(int CourseId, string ExamName);
        public bool DeleteExam(int ExamId);
        public List<TbQuestion> GetExamQuestions(int ExamId);
        public TbExam UpdateExam(int ExamId, int CourseId, string ExamName);
        public List<TbExam> GetAllExams();

    }
}