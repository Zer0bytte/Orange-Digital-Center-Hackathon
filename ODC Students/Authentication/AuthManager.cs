using BusinessLogic.AdminLogic;
using BusinessLogic.StudentLogic.Interfaces;
using Domains;
using Microsoft.IdentityModel.Tokens;
using StudentAuthManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationStudent
{
    public class AuthManager : IAuthManager
    {
        public IStudentAuthStructure Authentication { get; }
        public AuthManager(IStudentAuthStructure authentication)
        {
            Authentication = authentication;
        }

        public string Key { get; }

        public string Authenticate(string Username, string Password)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Username = Username;
            loginModel.Password = Password;

            TbStudent Student = Authentication.Login(loginModel);
            if (Student.StudentId == 0)
            {
                return "Username or password wrong";
            }
            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenKey = Encoding.ASCII.GetBytes("NIUXq4yk8GTGelfusivAHfSrdPEHvXWMXA1Khnlhnpk=");
            var TokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("StudentId", Student.StudentId.ToString()),
                    new Claim(ClaimTypes.Role,"Student")
                }),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(TokenKey),
                SecurityAlgorithms.HmacSha256Signature)

            };
            var Token = TokenHandler.CreateToken(TokenDecriptor);
            return TokenHandler.WriteToken(Token);
        }

       
    }
}
