using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbTeaching
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TrainerId { get; set; }

        public virtual TbCourse Course { get; set; }
        public virtual TbTrainer Trainer { get; set; }
    }
}
