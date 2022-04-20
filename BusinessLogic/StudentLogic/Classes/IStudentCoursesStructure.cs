using Domains;
using ODC_Students.Controllers;
using System.Collections.Generic;

namespace BusinessLogic.StudentLogic.Classes
{
    public interface IStudentCoursesStructure
    {
        ODCCoursesManagmentContext DbContext { get; }

        EnrollResult EnrollToCourse(EnrollModel enrollModel);
        List<TbCourse> GetAllCourses();
        public List<TbQuestion> GetCourseExam(int CourseId);
        public SetCodeResult SetVerificationCode(int StudentId,int Code);
    }
}