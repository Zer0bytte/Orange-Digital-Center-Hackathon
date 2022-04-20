using AuthenticationJWT;
using BusinessLogic.AdminLogic;
using Domains;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT
{
    public class AuthManager : IAuthManager
    {
        public IAdminAuthentication Authentication { get; }
        public AuthManager(IAdminAuthentication authentication)
        {
            Authentication = authentication;
        }

        public string Key { get; }

        public string Authenticate(string Username, string Password)
        {
            TbAdmin AdminUser = Authentication.Login(Username, Password);
            if (AdminUser.Id == 0)
            {
                return "Wrong Password";
            }
            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenKey = Encoding.ASCII.GetBytes("NIUXq4yk8GTGelfusivAHfSrdPEHvXWMXA1Khnlhnpk=");
            var TokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Username", Username),
                    new Claim(ClaimTypes.Role,AdminUser.IsSubAdmin? "SubAdmin":"Admin")
                }),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(TokenKey),
                SecurityAlgorithms.HmacSha256Signature)

            };
            var Token = TokenHandler.CreateToken(TokenDecriptor);
            return TokenHandler.WriteToken(Token);
        }
    }
}
