using Domains;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic
{
   public interface IAdminAuthentication
    {
        ODCCoursesManagmentContext DBContext { get; }

        public TbAdmin AssignSubAdmin(string Username, string Password);
        public TbAdmin AssignAdmin(string Username, string Password);
        public TbAdmin Login(string Username, string Password);
        public  Task<bool> SendVerificationCodeAsync(string Email, int CourseId, int StudentId);
    }
}