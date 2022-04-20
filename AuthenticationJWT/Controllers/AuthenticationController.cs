using BusinessLogic;
using BusinessLogic.AdminLogic;
using BusinessLogic.AdminLogic.Interfaces;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IAuthManager _authManager { get; set; }
        public IAdminAuthentication AdminAuth { get; }
        public ODCCoursesManagmentContext DbContext { get; }

        public AuthenticationController(IAuthManager authManager, IAdminAuthentication AdminAuth, ODCCoursesManagmentContext DbContext)
        {
            _authManager = authManager;
            this.AdminAuth = AdminAuth;
            this.DbContext = DbContext;
        }


        [HttpPost("Auth")]
        public IActionResult Authenticate([FromBody] LoginModel User)
        {
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Password))
            {
                return Ok("Username or password wrong");
            }
            var Token = _authManager.Authenticate(User.Username, User.Password);
            if (Token is null)
                return Unauthorized();

            return Ok(Token);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterSubAdmin")]
        public IActionResult RegisterSubAdmin([FromBody] LoginModel User)
        {
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Password))
            {
                return Ok("Username or password wrong");
            }
            TbAdmin CheckRegistered = DbContext.TbAdmins.FirstOrDefault(x => x.Username == User.Username);
            if (CheckRegistered is not null)
            {
                return Ok("Username Already Exist");
            }
            if (!PasswordAdvisor.ValidatePassword(User.Password))
            {
                return Ok("Password weak");

            }

            TbAdmin SubAdmin = AdminAuth.AssignSubAdmin(User.Username, User.Password);
            if (SubAdmin.Id != 0)
            {
                return Ok("SubAdmin Registered");
            }
            return BadRequest("cannot register subadmin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("SendVerificationCode")]
        public async Task<IActionResult> SendCode(SendCodeModel sendCodeModel)
        {
            bool SendResult = await AdminAuth.SendVerificationCodeAsync(sendCodeModel.Email, sendCodeModel.CourseId, sendCodeModel.StudentId);
            if (SendResult)
            {
                return Ok("Verification Code Sent");
            }
            return BadRequest("Please try again");
        }
    }
}
