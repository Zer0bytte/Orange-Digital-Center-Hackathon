using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic.Classes
{
    public class TrainersStructure : ITrainersStructure
    {
        public TrainersStructure(ODCCoursesManagmentContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public ODCCoursesManagmentContext DbContext { get; }

        public TbTrainer GetTrainerById(int TrainerId)
        {
            IQueryable<TbTrainer> Trainers = DbContext.TbTrainers;

            TbTrainer Trainer = Trainers.FirstOrDefault(x => x.TrainerId == TrainerId);
            return Trainer;
        }

        public List<TbTrainer> GetAllTrainers()
        {
            List<TbTrainer> Trainers = DbContext.TbTrainers.ToList();
            return Trainers;
        }
        public TbTrainer CreateTrainer(string TrainerName)
        {
            TbTrainer Trainer = new TbTrainer();
            Trainer.Name = TrainerName;
            DbContext.TbTrainers.Add(Trainer);
            DbContext.SaveChanges();
            return Trainer;

        }


        public TbTrainer UpdateTrainer(int TrainerId, string TrainerName)
        {
            TbTrainer Trainer = GetTrainerById(TrainerId);
            Trainer.Name = "Trainer";
            DbContext.SaveChanges();
            return Trainer;
        }

        public bool DeleteTrainer(int TrainerId)
        {
            TbTrainer Trainer = GetTrainerById(TrainerId);
            DbContext.Remove(Trainer);
            DbContext.SaveChanges();
            return true;
        }
    }
}
