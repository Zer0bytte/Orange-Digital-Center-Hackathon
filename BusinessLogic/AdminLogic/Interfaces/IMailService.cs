using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
