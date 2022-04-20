using BusinessLogic.StudentLogic.Classes;
using BusinessLogic.StudentLogic.Interfaces;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAuthManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ODC_Students.Controllers
{
    [Route("student/api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IAuthManager AuthManager { get; }
        public IStudentAuthStructure StudentStructure { get; }

        public AuthenticationController(IAuthManager authManager, IStudentAuthStructure StudentStructure)
        {
            AuthManager = authManager;
            this.StudentStructure = StudentStructure;
        }

        [HttpPost("RegisterStudent")]
        public IActionResult RegisterStudent(RegisterModel Model)
        {
            RegisterResult Result = StudentStructure.Register(Model);
            return Ok(Result);
        }

        [HttpPost("LoginStudent")]
        public IActionResult Login(LoginModel Model)
        {
            string Result = AuthManager.Authenticate(Model.Username, Model.Password);
            return Ok(Result);
        }
    }
}
