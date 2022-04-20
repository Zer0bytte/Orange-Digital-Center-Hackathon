using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic.Classes
{
    public class QuestionsStructure : IQuestionsStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }
        public QuestionsStructure(ODCCoursesManagmentContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public TbQuestion AddQuestion(int ExamId,
          string QuestionContent,
          string RightAnswer,
          string FirstChoice,
          string SecondChoice, string ThirdChoice, string FourthChoice)
        {
            TbQuestion question = new TbQuestion();
            question.ExamId = ExamId;
            question.QuestionContent = QuestionContent;
            question.QuestionRightAnswer = RightAnswer;
            question.FirstChoice = FirstChoice;
            question.SecondChoice = SecondChoice;
            question.ThirdChoice = ThirdChoice;
            question.FourthChoice = FourthChoice;
            DbContext.TbQuestions.Add(question);
            DbContext.SaveChanges();
            return question;
        }

        public TbQuestion GetQuestionById(int QuestionId)
        {
            IQueryable<TbQuestion> Questions = DbContext.TbQuestions;

            TbQuestion Question = Questions.FirstOrDefault(x => x.Id == QuestionId);
            return Question;
        }
        public TbQuestion EditQuestion(int QuestionId,
          string QuestionContent,
          string RightAnswer,
          string FirstChoice,
          string SecondChoice, string ThirdChoice, string FourthChoice, int ExamId)
        {
            TbQuestion question = GetQuestionById(QuestionId);
            question.ExamId = ExamId;
            question.QuestionContent = QuestionContent;
            question.QuestionRightAnswer = RightAnswer;
            question.FirstChoice = FirstChoice;
            question.SecondChoice = SecondChoice;
            question.ThirdChoice = ThirdChoice;
            question.FourthChoice = FourthChoice;
            DbContext.SaveChanges();
            return question;
        }

        public TbQuestion GetAllQuestionsForExam(int ExamId)
        {
            IQueryable<TbQuestion> Questions = DbContext.TbQuestions;

            TbQuestion Question = Questions.FirstOrDefault(x => x.ExamId == ExamId);
            return Question;
        }


        public List<TbQuestion> GetAllQuestions()
        {
            List<TbQuestion> Questions = DbContext.TbQuestions.ToList();
            return Questions;
        }



        public bool DeleteQuestion(int QuestionId)
        {
            TbQuestion Question = DbContext.TbQuestions.FirstOrDefault(x => x.Id == QuestionId);
            DbContext.TbQuestions.Remove(Question);
            DbContext.SaveChanges();
            return true;
        }
    }
}
