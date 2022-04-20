using BusinessLogic.AdminLogic.Interfaces;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollsController : ControllerBase
    {
        public IEnrollsStructure EnrollsStructure { get; }
        public EnrollsController(IEnrollsStructure EnrollsStructure)
        {
            this.EnrollsStructure = EnrollsStructure;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetEnrolls")]
        public List<TbEnroll> Get()
        {
            return EnrollsStructure.GetPendingEnrolls();
        }

      
    }
}
