using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic
{
    public interface ICoursesStructure
    {
        ODCCoursesManagmentContext DbContext { get; }

        TbCourse CreateCourse(string CourseName, int CategoryId, string CourseLevel);
        bool DeleteCourse(int CourseId);
        List<TbCourse> GetAllCourses();
        TbCourse GetCourseById(int CourseId);
        TbCourse GetCourseByName(string CourseName);
        TbCourse UpdateCourse(int CourseId, string CourseName, int CategoryId, string CourseLevel);
    }
}