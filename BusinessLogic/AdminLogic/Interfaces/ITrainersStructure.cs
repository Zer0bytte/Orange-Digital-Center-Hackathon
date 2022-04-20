using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic.Classes
{
    public interface ITrainersStructure
    {
        ODCCoursesManagmentContext DbContext { get; }

        TbTrainer CreateTrainer(string TrainerName);
        bool DeleteTrainer(int TrainerId);
        List<TbTrainer> GetAllTrainers();
        TbTrainer GetTrainerById(int TrainerId);
        TbTrainer UpdateTrainer(int TrainerId, string TrainerName);
    }
}