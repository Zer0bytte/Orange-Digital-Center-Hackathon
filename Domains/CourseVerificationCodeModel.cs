// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ODC_Students.Controllers
{
    public class CourseVerificationCodeModel
    {
        public int VCode { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }


    }
}
