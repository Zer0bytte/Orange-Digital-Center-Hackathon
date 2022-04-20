using BusinessLogic.AdminLogic;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentManagerController : ControllerBase
    {
        public IStudentsStructure StudentStructure { get; }
        public ODCCoursesManagmentContext DbContext { get; }

        public StudentManagerController(IStudentsStructure StudentStructure, ODCCoursesManagmentContext DbContext)
        {
            this.StudentStructure = StudentStructure;
            this.DbContext = DbContext;
        }
        [Authorize(Roles = "Admin,SubAdmin")]
        [HttpGet("GetAllStudents")]
        public List<TbStudent> Get()
        {
            return StudentStructure.GetAllStudents();
        }

        [Authorize(Roles = "Admin,SubAdmin")]
        [HttpGet("GetStudentExams/{StudentId}")]
        public TbRevision Get(int StudentId)
        {
            var Revision = StudentStructure.GetStudentRevisionById(StudentId);
            return Revision;
        }

        [Authorize(Roles = "SubAdmin")]
        [HttpPost("SendAcceptStatusMessage")]
        public IActionResult SendAcceptStatusMessage(TbStudent Student)
        {
            if (Student.StudentId == 0)
            {
                return Ok("Student Not found");

            }
            var Revision = StudentStructure.GetStudentRevisionById(Student.StudentId);
           
            if (Revision != null)
            {
                Revision.Status = "Accepted";
                DbContext.SaveChanges();
                return Ok("Student Accepted to interview");
            }
            return Ok("Student Not found");
        }

    }
}
