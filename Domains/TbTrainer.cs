using System;
using System.Collections.Generic;

#nullable disable

namespace Domains
{
    public partial class TbTrainer
    {
        public TbTrainer()
        {
            TbTeachings = new HashSet<TbTeaching>();
        }

        public int TrainerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TbTeaching> TbTeachings { get; set; }
    }
}
