using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic.Interfaces
{
    public interface IEnrollsStructure
    {
        public List<TbEnroll> GetPendingEnrolls();
    }
}