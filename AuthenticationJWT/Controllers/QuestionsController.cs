using BusinessLogic.AdminLogic;
using BusinessLogic.AdminLogic.Classes;
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
    public class QuestionsController : ControllerBase
    {
        public IQuestionsStructure QuestionsStructure { get; }
        public IExamsStructure ExamsStructure { get; }

        public QuestionsController(IQuestionsStructure QuestionsStructure, IExamsStructure ExamsStructure)
        {
            this.QuestionsStructure = QuestionsStructure;
            this.ExamsStructure = ExamsStructure;
        }


        [HttpPost("AddQuestion")]
        public IActionResult Post([FromBody] TbQuestion Question)
        {
            if (Question.ExamId != 0 && string.IsNullOrWhiteSpace(Question.FirstChoice)
                && string.IsNullOrWhiteSpace(Question.SecondChoice)
                && string.IsNullOrWhiteSpace(Question.ThirdChoice)
                && string.IsNullOrWhiteSpace(Question.FourthChoice)
                && string.IsNullOrWhiteSpace(Question.QuestionRightAnswer))
            {
                return BadRequest("Please enter question data");
            }


            TbQuestion AddedQuestion = QuestionsStructure.AddQuestion(Question.ExamId, Question.QuestionContent
                     , Question.QuestionRightAnswer, Question.FirstChoice,
                     Question.SecondChoice, Question.ThirdChoice, Question.FourthChoice);
            if (AddedQuestion.Id == 0)
                return BadRequest("Error in question creation");

            return Ok("Question Added Successfully");
        }

        [HttpGet("GetExamQuestions")]
        public List<TbQuestion> GetExamQuestions(int ExamId)
        {
            List<TbQuestion> ExamQuestions = ExamsStructure.GetExamQuestions(ExamId);
            return ExamQuestions;
        }



        [HttpPost("UpdateQuestion")]
        public IActionResult UpdateQuestion([FromBody] TbQuestion Question)
        {

            TbQuestion UpdatedQuestion = QuestionsStructure.EditQuestion(Question.Id, Question.QuestionContent,
                        Question.QuestionRightAnswer, Question.FirstChoice,
                        Question.SecondChoice,
                        Question.ThirdChoice, Question.FourthChoice, Question.ExamId);
            if (UpdatedQuestion != null)
            {
                return Ok("Question Updated");

            }
            return BadRequest("Error in question data");
        }


        [HttpPost("DeleteQuestion")]
        public IActionResult DeleteQuestion([FromBody] TbQuestion Question)
        {
            bool Deleted = QuestionsStructure.DeleteQuestion(Question.Id);
            if (Deleted)
                return Ok("Question Delete Successfully");

            return BadRequest("Failed to delete question");

        }


    }
}
