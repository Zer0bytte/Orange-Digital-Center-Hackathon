using Domains;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.AdminLogic.Classes
{
    public class CategoriesStructure : ICategoriesStructure
    {
        public ODCCoursesManagmentContext DbContext { get; }

        public CategoriesStructure(ODCCoursesManagmentContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public TbCategroie GetCategoryById(int CategoryId)
        {
            IQueryable<TbCategroie> Categories = DbContext.TbCategroies;

            TbCategroie Category = Categories.FirstOrDefault(x => x.CategoryId == CategoryId);
            return Category;
        }
        public TbCategroie GetCategoryByName(string CategoryName)
        {
            IQueryable<TbCategroie> Categories = DbContext.TbCategroies;

            TbCategroie Category = Categories.FirstOrDefault(x => x.CategoryName == CategoryName);
            return Category;
        }
        public List<TbCategroie> GetAllCategorys()
        {            List<TbCategroie> Categorys = DbContext.TbCategroies.ToList();
            return Categorys;
        }
        public TbCategroie CreateCategory(string CategoryName)
        {
            TbCategroie Category = new TbCategroie();
            Category.CategoryName = CategoryName;
            DbContext.TbCategroies.Add(Category);
            DbContext.SaveChanges();
            return Category;
        }


        public TbCategroie UpdateCategory(int CategoryId, string CategoryName)
        {
            TbCategroie Category = GetCategoryById(CategoryId);
            Category.CategoryName = CategoryName;
            DbContext.SaveChanges();
            return Category;
        }

        public bool DeleteCategory(int CategoryId)
        {
            TbCategroie Category = GetCategoryById(CategoryId);
            DbContext.Remove(Category);
            DbContext.SaveChanges();
            return true;
        }
    }
}
