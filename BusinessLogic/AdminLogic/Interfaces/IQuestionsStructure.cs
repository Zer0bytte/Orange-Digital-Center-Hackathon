using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic.Classes
{
    public interface IQuestionsStructure
    {
        ODCCoursesManagmentContext DbContext { get; }

        TbQuestion AddQuestion(int ExamId, string QuestionContent, string RightAnswer, string FirstChoice, string SecondChoice, string ThirdChoice, string FourthChoice);
        bool DeleteQuestion(int QuestionId);
        TbQuestion EditQuestion(int QuestionId, string QuestionContent, string RightAnswer, string FirstChoice, string SecondChoice, string ThirdChoice, string FourthChoice, int ExamId);
        List<TbQuestion> GetAllQuestions();
        TbQuestion GetAllQuestionsForExam(int ExamId);
        TbQuestion GetQuestionById(int QuestionId);
    }
}