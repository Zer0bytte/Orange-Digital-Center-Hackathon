using BusinessLogic.AdminLogic;
using BusinessLogic.AdminLogic.Classes;
using BusinessLogic.StudentLogic.Classes;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ODC_Students.Controllers
{
    [Route("student/api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentCoursesController : ControllerBase
    {
        public IStudentCoursesStructure StudentCoursesStructure { get; }
        public IQuestionsStructure QuestionsStructure { get; }
        public ODCCoursesManagmentContext DbContext { get; }

        public StudentCoursesController(IStudentCoursesStructure StudentCoursesStructure, IQuestionsStructure QuestionsStructure, ODCCoursesManagmentContext DbContext)
        {
            this.StudentCoursesStructure = StudentCoursesStructure;
            this.QuestionsStructure = QuestionsStructure;
            this.DbContext = DbContext;
        }
        [HttpGet("ViewCourses")]
        public List<TbCourse> ViewCourses()
        {
            return StudentCoursesStructure.GetAllCourses();
        }
        [HttpPost("EnrollToCourse")]
        public EnrollResult EnrollToCourse(EnrollModel Model)
        {
            var Result = StudentCoursesStructure.EnrollToCourse(Model);

            return Result;
        }



        [HttpPost("SetVerificationCode")]
        public IActionResult SetVerificationCode(CourseVerificationCodeModel Model)
        {
            var Result = StudentCoursesStructure.SetVerificationCode(Model.StudentId, Model.VCode);

            return Ok(Result);
        }


        [HttpGet("GetCourseExam")]
        public List<TbQuestion> GetCourseExam(int CourseId)
        {


            List<TbQuestion> CourseExam = StudentCoursesStructure.GetCourseExam(CourseId);
            return CourseExam;
        }


        [HttpGet("GetInterviewStatus")]
        public IActionResult GetInterviewStatus(TbStudent StudentId)
        {
            if (StudentId is null)
            {
                return Ok("Student not found");
            }
            if (StudentId.StudentId == 0)
            {
                return Ok("Student not found");
            }


            TbRevision Revision = DbContext.TbRevisions.FirstOrDefault(x => x.StudentId == StudentId.StudentId);
            if (Revision is null)
            {
                return Ok("Student revision not found");
            }
            return Ok(Revision.Status);
        }


        [HttpPost("PublishStudentAnswers")]
        public IActionResult PublishStudentAnswers(List<ModelAnswer> Answers)
        {
            int RightAnswers = 0;
            int WrongAnwsers = 0;
            int ExamId = 0;
            int StudentId = 0;
            foreach (var Answer in Answers)
            {
                if (Answer.StudentAnswer == "UnAnswered")
                {
                    WrongAnwsers++;
                    continue;
                }
                TbQuestion QuestionRightAnswer = QuestionsStructure.GetQuestionById(Answer.QeustionId);
                ExamId = QuestionRightAnswer.ExamId;
                StudentId = Answer.StudentId;
                if (Answer.StudentAnswer == QuestionRightAnswer.QuestionRightAnswer)
                    RightAnswers++;
                else
                    WrongAnwsers++;
            }


            TbRevision Revision = new TbRevision();
            Revision.ExamId = ExamId;
            Revision.StudentId = StudentId;
            Revision.TotalRightDegrees = RightAnswers;
            Revision.TotalWrongDegrees = WrongAnwsers;
            decimal StudentDegree = Math.Round(Decimal.Divide(RightAnswers, Answers.Count) * 100, 0);
            Revision.StudentDegree = StudentDegree;
            Revision.StudentDegree = StudentDegree;
            Revision.Status = "Waiting";
            DbContext.TbRevisions.Add(Revision);
            DbContext.SaveChanges();
            if (StudentDegree >= 50)
            {
                return Ok($"You passed the exam with Percentage: {StudentDegree}%");

            }
            return Ok($"You failed in the exam with percentage {StudentDegree}%");
        }
    }
}
