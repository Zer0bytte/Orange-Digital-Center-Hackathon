using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbAdmin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSubAdmin { get; set; }
    }
}
