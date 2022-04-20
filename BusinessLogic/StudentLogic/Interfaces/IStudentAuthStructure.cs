using BusinessLogic.AdminLogic;
using BusinessLogic.StudentLogic.Classes;
using Domains;

namespace BusinessLogic.StudentLogic.Interfaces
{
    public interface IStudentAuthStructure
    {
        ODCCoursesManagmentContext DbContext { get; }
        IStudentsStructure StudentsStructure { get; }

        TbStudent Login(LoginModel LoginModel);
        RegisterResult Register(RegisterModel Model);
    }
}