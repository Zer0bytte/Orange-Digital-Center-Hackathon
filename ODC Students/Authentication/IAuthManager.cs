using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAuthManager
{
    public interface IAuthManager
    {
        string Authenticate(string Username, string Password);
    }
}
