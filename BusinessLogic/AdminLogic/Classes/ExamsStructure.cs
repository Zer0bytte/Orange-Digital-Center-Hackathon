using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic
{
    public class ExamsStructure : IExamsStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }
        public IStudentsStructure StudentStructure { get; }

        public ExamsStructure(ODCCoursesManagmentContext DbContext, IStudentsStructure StudentStructure)
        {
            this.DbContext = DbContext;
            this.StudentStructure = StudentStructure;
        }

        public TbExam CreateExam(int CourseId, string ExamName)
        {
            try
            {

                //Create Exam
                TbExam Exam = new TbExam();
                Exam.CourseId = CourseId;
                Exam.ExamName = ExamName;
                DbContext.TbExams.Add(Exam);
                DbContext.SaveChanges();
                return Exam;
            }
            catch (Exception)
            {
                return new TbExam();

            }
        }
        public TbExam GetExamById(int ExamId)
        {
            TbExam Exam = DbContext.TbExams.FirstOrDefault(x => x.ExamId == ExamId);
            return Exam;
        }
        public TbExam UpdateExam(int ExamId, int CourseId, string ExamName)
        {
            TbExam Exam = GetExamById(ExamId);
            Exam.CourseId = CourseId;
            Exam.ExamName = ExamName;
            DbContext.SaveChanges();
            return Exam;
        }
        public bool DeleteExam(int ExamId)
        {
            TbExam Exam = GetExamById(ExamId);
            DbContext.Remove(Exam);
            DbContext.SaveChanges();
            return true;
        }


        public List<TbQuestion> GetExamQuestions(int ExamId)
        {
            var Questions = DbContext.TbQuestions.Where(x => x.ExamId == ExamId).ToList();
            return Questions;
        }

        public List<TbExam> GetAllExams()
        {
            var Exams = DbContext.TbExams.Select(x => new TbExam
            {
                ExamName = x.ExamName,
                CourseId = x.CourseId,
                ExamId = x.ExamId,
                Course = new TbCourse { CourseName = x.Course.CourseName }

            }).ToList();

            return Exams;

        }
    }
}
