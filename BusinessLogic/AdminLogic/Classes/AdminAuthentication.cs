using BusinessLogic.AdminLogic.Interfaces;
using Domains;
using Isopoh.Cryptography.Argon2;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic
{
    public class AdminAuthentication : IAdminAuthentication
    {
        public ODCCoursesManagmentContext DBContext { get; }
        public IMailService MailService { get; }
        public ICoursesStructure CoursesStructure { get; }

        public AdminAuthentication(ODCCoursesManagmentContext DBContext, IMailService MailService, ICoursesStructure CoursesStructure)
        {
            this.DBContext = DBContext;
            this.MailService = MailService;
            this.CoursesStructure = CoursesStructure;
        }
        /// <summary>
        /// Login to adminpanel
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public TbAdmin Login(string Username, string Password)
        {

            var User = DBContext.TbAdmins.FirstOrDefault(x => x.Username == Username);
            if (Argon2.Verify(User.Password, Password))
            {
                return User;
            }
            return new TbAdmin();
        }
        /// <summary>
        /// Create SubAdmin
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public TbAdmin AssignSubAdmin(string Username, string Password)
        {
            var HashedPassword = Argon2.Hash(Password);
            TbAdmin SubAdmin = new TbAdmin();
            SubAdmin.IsSubAdmin = true;
            SubAdmin.Username = Username;
            SubAdmin.Password = HashedPassword;
            DBContext.TbAdmins.Add(SubAdmin);
            DBContext.SaveChanges();

            return SubAdmin;
        }

        public TbAdmin AssignAdmin(string Username, string Password)
        {
            var HashedPassword = Argon2.Hash(Password);
            TbAdmin Admin = new TbAdmin();
            Admin.IsSubAdmin = false;
            Admin.Username = Username;
            Admin.Password = HashedPassword;
            DBContext.TbAdmins.Add(Admin);
            DBContext.SaveChanges();

            return Admin;
        }


        public async Task<bool> SendVerificationCodeAsync(string Email, int CourseId, int StudentId)
        {
            Random generator = new Random();
            int VerificationCd = generator.Next(1000, 9999);

            TbVerificationCode VerficationCode = new TbVerificationCode();

            VerficationCode.VerificationCode = VerificationCd;

            VerficationCode.CourseId = CourseId;
            VerficationCode.EndDate = System.DateTime.Now.AddDays(2);
            DBContext.TbVerificationCodes.Add(VerficationCode);

            IQueryable<TbEnroll> Enrolls = DBContext.TbEnrolls;

            var Enroll = Enrolls.FirstOrDefault(x => x.StudentId == StudentId && x.CourseId == CourseId && x.VerificationCodeSent == false);
            if (Enroll is not null)
            {

                Enroll.VerificationCodeSent = true;
                TbCourse Course = CoursesStructure.GetCourseById(CourseId);
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = Email;
                mailRequest.Subject = "Odange Digital Center Verification Code";
                mailRequest.Body = $"Your verifcation code for {Course.CourseName} is <b>{VerificationCd}</b>";
                await MailService.SendEmailAsync(mailRequest);
                DBContext.SaveChanges();
                return true;
            }
            return false;

        }
    }
}
