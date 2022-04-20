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
    public class CoursesController : ControllerBase
    {
        public ICoursesStructure CoursesStructure { get; }

        public CoursesController(ICoursesStructure CoursesStructure)
        {
            this.CoursesStructure = CoursesStructure;
        }
        // GET: api/<CoursesController>
        [HttpGet("GetAllCourses")]
        public List<TbCourse> GetAllCourses()
        {
            return CoursesStructure.GetAllCourses();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateCourse")]
        public IActionResult Post([FromBody] TbCourse courseModel)
        {
            if (courseModel is null)
            {

                return BadRequest("Invalid Data");
            }
            if (string.IsNullOrWhiteSpace(courseModel.CourseName) || string.IsNullOrWhiteSpace(courseModel.CourseLevel) || courseModel.CategoryId == 0)
            {
                return BadRequest("Invalid Data");

            }
            if (CoursesStructure.GetCourseByName(courseModel.CourseName) is not null)
                return BadRequest("Course with that name already exist");

            TbCourse CreatedCourse = CoursesStructure.CreateCourse(courseModel.CourseName,
                    courseModel.CategoryId,
                    courseModel.CourseLevel);

            if (CreatedCourse.CategoryId == 0)
                return BadRequest("Category not found");

            return Ok("Course Added Succesfully");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateCourse")]
        public IActionResult UpdateCourse(TbCourse courseModel)
        {
            TbCourse CoruseById = CoursesStructure.GetCourseById(courseModel.CourseId);
            if (CoruseById is null)
                return BadRequest("Course not found");
            TbCourse CourseByName = CoursesStructure.GetCourseByName(courseModel.CourseName);

            if (CourseByName is not null && CourseByName.CourseId == CoruseById.CourseId && CourseByName.CategoryId == courseModel.CategoryId && CourseByName.CourseLevel == courseModel.CourseLevel)
            {
                return BadRequest("no changes");

            }



            TbCourse Course = CoursesStructure.UpdateCourse(courseModel.CourseId, courseModel.CourseName, courseModel.CategoryId, courseModel.CourseLevel);
            if (Course.CategoryId == 0)
                return BadRequest("Category not found");

            return Ok("Course updated succesfully");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteCourse")]
        public IActionResult DeleteCourse([FromBody] TbCourse Course)
        {
            if (CoursesStructure.GetCourseById(Course.CourseId) is null)
                return BadRequest("Course not found");

            CoursesStructure.DeleteCourse(Course.CourseId);
            return Ok("Course deleted successfully");
        }

    }


}
