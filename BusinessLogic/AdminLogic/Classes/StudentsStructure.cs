using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic.Classes
{
    public class StudentsStructure : IStudentsStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }
        public StudentsStructure(ODCCoursesManagmentContext DbContext)
        {
            this.DbContext = DbContext;
        }


        public TbStudent GetStudentById(int StudentId)
        {
            TbStudent Student = DbContext.TbStudents.Include(x => x.TbRevisions).FirstOrDefault(x => x.StudentId == StudentId);
            return Student;
        }

        public bool IsUsernameExist(string Username)
        {
            TbStudent Student = DbContext.TbStudents.FirstOrDefault(x => x.StudentName == Username);
            if (Student is null)
                return false;


            return true; //False not exist
        }

        public bool IsEmailExist(string Email)
        {
            TbStudent Student = DbContext.TbStudents.FirstOrDefault(x => x.StudentEmail == Email);
            if (Student is null)
                return false;


            return true;
        }



        public TbRevision GetStudentRevisionById(int StudentId)
        {
            TbRevision StudentRevision = DbContext.TbRevisions.FirstOrDefault(x => x.StudentId == StudentId);
            return StudentRevision;
        }
        public List<TbStudent> GetAllStudents()
        {
            List<TbStudent> Students = DbContext.TbStudents.ToList();
            return Students;
        }
    }
}
