using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic
{
   public interface IStudentsStructure
    {

        TbStudent GetStudentById(int StudentId);
        public List<TbStudent> GetAllStudents();
        public TbRevision GetStudentRevisionById(int StudentId);
        public bool IsUsernameExist(string Username);
        public bool IsEmailExist(string Email);
    }
}