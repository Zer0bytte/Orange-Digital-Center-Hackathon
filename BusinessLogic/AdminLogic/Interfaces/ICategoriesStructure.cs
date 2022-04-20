using Domains;
using System.Collections.Generic;

namespace BusinessLogic.AdminLogic
{
  public  interface ICategoriesStructure
    {
        ODCCoursesManagmentContext DbContext { get; }

        TbCategroie CreateCategory(string CategoryName);
        bool DeleteCategory(int CategoryId);
        List<TbCategroie> GetAllCategorys();
        TbCategroie GetCategoryById(int CategoryId);
        TbCategroie UpdateCategory(int CategoryId, string CategoryName);
        public TbCategroie GetCategoryByName(string CategoryName);
    }
}