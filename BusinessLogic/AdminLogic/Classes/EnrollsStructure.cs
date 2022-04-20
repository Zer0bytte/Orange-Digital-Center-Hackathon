using BusinessLogic.AdminLogic.Interfaces;
using Domains;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.AdminLogic.Classes
{
    public class EnrollsStructure : IEnrollsStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }
        public EnrollsStructure(ODCCoursesManagmentContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public List<TbEnroll> GetPendingEnrolls()
        {
            IQueryable<TbEnroll> QueryEnrolls = DbContext.TbEnrolls;

            List<TbEnroll> Enrolls = QueryEnrolls.Select(x => new TbEnroll
            {
                VerificationCodeSent = x.VerificationCodeSent,
                Id = x.Id,
                StudentId = x.StudentId,
                CourseId = x.CourseId,
                Student = new TbStudent()
                {
                    StudentName = x.Student.StudentName,
                    StudentEmail = x.Student.StudentEmail
                },
                Course = new TbCourse()
                {
                    CourseName = x.Course.CourseName
                }
            }).Where(x => x.VerificationCodeSent == false).ToList();



            return Enrolls;
        }
    }
}
