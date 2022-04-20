using Domains;
using Microsoft.AspNetCore.Mvc;
using ODC_Students.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.StudentLogic.Classes
{
    public class StudentCoursesStructure : IStudentCoursesStructure
    {
        public StudentCoursesStructure(ODCCoursesManagmentContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public ODCCoursesManagmentContext DbContext { get; }


        public List<TbCourse> GetAllCourses()
        {
            List<TbCourse> Courses = DbContext.TbCourses.Select(x => new TbCourse
            {
                CourseId = x.CourseId,
                CategoryId = x.CategoryId,
                CourseName = x.CourseName,
                CourseLevel = x.CourseLevel,
                CreatedAt = x.CreatedAt,
                Category = new TbCategroie() { CategoryName = x.Category.CategoryName }
            }).ToList();
            return Courses;
        }
        public EnrollResult EnrollToCourse(EnrollModel enrollModel)
        {
            EnrollResult enrollResult = new EnrollResult();
            //check if user already enroll

            TbEnroll CheckIfEnrolled = DbContext.TbEnrolls.FirstOrDefault(x => x.StudentId == enrollModel.StudentId);
            if (CheckIfEnrolled is not null)
            {
                enrollResult.Message = $"You already enrolled to a course go to Quiz??";
                return enrollResult;
            }
            TbEnroll tbEnroll = new TbEnroll();
            tbEnroll.StudentId = enrollModel.StudentId;
            tbEnroll.CourseId = enrollModel.CourseId;
            tbEnroll.CreatedAt = System.DateTime.Now;
            DbContext.TbEnrolls.Add(tbEnroll);
            DbContext.SaveChanges();
            enrollResult.Message = "You have been enrolled to this course successfully, plase wait for verfication code";

            return enrollResult;
        }

        public SetCodeResult SetVerificationCode(int StudentId, int Code)
        {
            SetCodeResult Result = new SetCodeResult();
            IQueryable<TbVerificationCode> verificationCodes = DbContext.TbVerificationCodes;
            TbVerificationCode VCode = verificationCodes.FirstOrDefault(x => x.VerificationCode == Code);

            // TODO: Check if this code assigned to this user

            if (VCode is null)
            {
                Result.Message = "Invalid Code";
                return Result;
            }
            //check if verifiaction code not end
            if (DateTime.Now > VCode.EndDate)
            {
                Result.Message = "Code has been expired";
                return Result;
            }
            if (VCode.IsUsed == true)
            {
                Result.Message = "Code has been expired";
                return Result;

            }
            

            VCode.IsUsed = true;
            DbContext.SaveChanges();
            Result.Message = " verification  applied successfully. redirecting to exam...";
            Result.CourseId = VCode.CourseId;
            return Result;
        }


        public List<TbQuestion> GetCourseExam(int CourseId)
        {
            IQueryable<TbExam> Exams = DbContext.TbExams;
            List<TbExam> CourseExams = Exams.Where(x => x.CourseId == CourseId).ToList();
            TbExam Exam = CourseExams.OrderBy(r => Guid.NewGuid()).Take(1).FirstOrDefault();
            IQueryable<TbQuestion> AllQuestions = DbContext.TbQuestions;
            List<TbQuestion> ExamQuestions = AllQuestions.Select(x => new TbQuestion
            {
                ExamId = x.ExamId,
                Id = x.Id,
                QuestionContent = x.QuestionContent,
                FirstChoice = x.FirstChoice,
                SecondChoice = x.SecondChoice,
                ThirdChoice = x.ThirdChoice,
                FourthChoice = x.FourthChoice,

            }).Where(x => x.ExamId == Exam.ExamId).ToList();

            return ExamQuestions;
        }
    }
    public class EnrollResult
    {
        public string Message { get; set; }
    }
}
