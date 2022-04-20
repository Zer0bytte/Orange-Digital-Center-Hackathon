using BusinessLogic.AdminLogic;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuthenticationJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        public ODCCoursesManagmentContext DbContext { get; }
        public IExamsStructure ExamsStructure { get; }

        public ExamsController(ODCCoursesManagmentContext DbContext, IExamsStructure ExamsStructure)
        {
            this.DbContext = DbContext;
            this.ExamsStructure = ExamsStructure;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllExams")]
        public List<TbExam> GetAllExams()
        {
            List<TbExam> Exams = ExamsStructure.GetAllExams();
            return Exams;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateExam")]
        public IActionResult CreateExam([FromBody] TbExam Exam)
        {

            TbExam CreatedExam = ExamsStructure.CreateExam(Exam.CourseId, Exam.ExamName);
            if (CreatedExam.ExamId == 0)
            {
                return BadRequest("Error in exam Creation");

            }

            return Ok("Exam Created Successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateExam")]
        public IActionResult UpdateExam([FromBody] TbExam Exam)
        {
            if (Exam.ExamId == 0)
                return BadRequest("Error in exam update");
            if (string.IsNullOrWhiteSpace(Exam.ExamName))
                return BadRequest("Please Enter exam name");

            TbExam UpdatedExam = ExamsStructure.UpdateExam(Exam.ExamId, Exam.CourseId, Exam.ExamName);
            if (UpdatedExam.ExamId == 0)
            {
                return BadRequest("Error in exam update");

            }

            return Ok("Exam updated Successfully");

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteExam")]
        public IActionResult DeleteExam([FromBody] TbExam Exam)
        {
            if (Exam.ExamId == 0)
                return BadRequest("Error in exam data");

            ExamsStructure.DeleteExam(Exam.ExamId);

            return Ok("Exam Deleted Successfully");

        }

    }

}
