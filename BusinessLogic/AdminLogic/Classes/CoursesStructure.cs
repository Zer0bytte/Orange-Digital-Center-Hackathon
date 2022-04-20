using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AdminLogic
{
    public class CoursesStructure : ICoursesStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }

        public CoursesStructure(ODCCoursesManagmentContext dbContext)
        {
            DbContext = dbContext;
        }

        public TbCourse GetCourseByName(string CourseName)
        {
            IQueryable<TbCourse> Courses = DbContext.TbCourses;
            TbCourse Course = Courses.FirstOrDefault(x => x.CourseName == CourseName);
            return Course;

        }
        public List<TbCourse> GetAllCourses()
        {
            List<TbCourse> Courses = DbContext.TbCourses.Select(x => new TbCourse
            { 
                CourseId = x.CourseId,
                CategoryId = x.CategoryId,
                CourseName = x.CourseName,
                CourseLevel = x.CourseLevel,
                CreatedAt = x.CreatedAt,
                Category = new TbCategroie() { CategoryName = x.Category.CategoryName}
            }).ToList();
            return Courses;
        }

        public TbCourse CreateCourse(string CourseName, int CategoryId, string CourseLevel)
        {
            try
            {
                TbCourse Course = new TbCourse();
                Course.CourseName = CourseName;
                Course.CourseLevel = CourseLevel;
                Course.CategoryId = CategoryId;
                Course.CreatedAt = DateTime.Now;
                DbContext.TbCourses.Add(Course);
                DbContext.SaveChanges();

                return Course;
            }
            catch (Exception)
            {

                return new TbCourse();
            }


        }
        public TbCourse GetCourseById(int CourseId)
        {
            IQueryable<TbCourse> Courses = DbContext.TbCourses;
            TbCourse Course = Courses.Where(x => x.CourseId == CourseId).FirstOrDefault();
            return Course;
        }

        public TbCourse UpdateCourse(int CourseId, string CourseName, int CategoryId, string CourseLevel)
        {
            try
            {
                TbCourse Course = GetCourseById(CourseId);
                Course.CourseName = CourseName;
                Course.CategoryId = CategoryId;
                Course.CourseLevel = CourseLevel;
                DbContext.SaveChanges();
                return Course;
            }
            catch (Exception)
            {
                return new TbCourse();
            }

        }

        public bool DeleteCourse(int CourseId)
        {
            TbCourse Course = GetCourseById(CourseId);
            DbContext.TbCourses.Remove(Course);
            DbContext.SaveChanges();

            return true;

        }
    }
}
