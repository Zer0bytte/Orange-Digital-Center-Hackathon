using BusinessLogic.AdminLogic;
using BusinessLogic.StudentLogic.Interfaces;
using Domains;
using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.StudentLogic.Classes
{
    public class StudentAuthStructure : IStudentAuthStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }
        public IStudentsStructure StudentsStructure { get; }

        public StudentAuthStructure(ODCCoursesManagmentContext DbContext, IStudentsStructure StudentsStructure)
        {
            this.DbContext = DbContext;
            this.StudentsStructure = StudentsStructure;
        }


        public RegisterResult Register(RegisterModel Model)
        {
            RegisterResult registerResult = new();

            bool UsernameExist = StudentsStructure.IsUsernameExist(Model.Username);
            bool EmailExist = StudentsStructure.IsEmailExist(Model.Email);
            if (EmailExist)
            {
                registerResult.Messages += "Email Exist ";
            }
            if (UsernameExist)
            {
                registerResult.Messages += "Username Exist ";
            }

            bool ValidatPassword = PasswordAdvisor.ValidatePassword(Model.Password);
            bool ValidateEmail = PasswordAdvisor.IsValidEmail(Model.Email);
            bool UnvalidUsername = Model.Username.Any(Char.IsWhiteSpace);

            if (UnvalidUsername)
            {
                registerResult.Messages = "Invalid Username";
                return registerResult;
            }
            if (!ValidatPassword)
            {
                registerResult.Messages = "Password Weak please use special chars and numbers  capital and small letters";
                return registerResult;
            }
            if (!ValidateEmail)
            {
                registerResult.Messages = "Incorrect Email";
                return registerResult;
            }

            if (!PasswordAdvisor.IsValidPhoneNumber(Model.Phone))
            {
                registerResult.Messages = "Incorrect Phone number";
                return registerResult;
            }
            if (EmailExist == false && UsernameExist == false)
            {
                TbStudent Student = new TbStudent();
                Student.StudentName = Model.Username;
                Student.StudentPhone = Model.Phone;
                Student.StudentEmail = Model.Email;
                Student.StudentCollege = Model.College;
                Student.Password = Argon2.Hash(Model.Password);
                Student.CreatedAt = DateTime.Now;
                DbContext.TbStudents.Add(Student);
                DbContext.SaveChanges();
                registerResult.Messages = "Account Registered Successfully";
            }

            return registerResult;

        }


        public TbStudent Login(LoginModel LoginModel)
        {
            IQueryable<TbStudent> Students = DbContext.TbStudents;
            TbStudent Student = Students.Where(x => x.StudentName == LoginModel.Username).FirstOrDefault();
            if (Student != null)
            {
                if (Argon2.Verify(Student.Password, LoginModel.Password))
                {
                    return Student;
                }
            }


            return new TbStudent();

        }
    }

    public class RegisterResult
    {
        public string Messages { get; set; }
    }
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string College { get; set; }

    }


}
