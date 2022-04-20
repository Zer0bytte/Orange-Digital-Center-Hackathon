using BusinessLogic.AdminLogic;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategroiesController : ControllerBase
    {
        public CategroiesController(ICategoriesStructure CategoryStructure)
        {
            this.CategoryStructure = CategoryStructure;
        }

        public ICategoriesStructure CategoryStructure { get; }

        [HttpGet("GetAllCategories")]
        public List<TbCategroie> Get()
        {
            return CategoryStructure.GetAllCategorys();
        }
        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody] CategoryModel Category)
        {
            TbCategroie CategoryById = CategoryStructure.GetCategoryById(Category.CateogryId);
            if (CategoryById is null)
                return BadRequest("Category not found");
            TbCategroie CategoryByName = CategoryStructure.GetCategoryByName(Category.CategoryName);

            if (CategoryByName is not null && CategoryByName.CategoryId == CategoryById.CategoryId)
                return Ok("No Changes");

            if (CategoryByName is not null)
                return BadRequest("Category name already exist");

            CategoryStructure.UpdateCategory(Category.CateogryId, Category.CategoryName);
            return Ok("Category updated succesfully");

        }

  //      [Authorize]
        [HttpPost("AddCategory")]
        public IActionResult Post([FromBody] CategoryModel Category)
        {
            if (string.IsNullOrWhiteSpace(Category.CategoryName))
                return Ok("Invalid Category Name");

            if (CategoryStructure.GetCategoryByName(Category.CategoryName) is not null)
            {
                return Ok("Category already exist");
            }
            CategoryStructure.CreateCategory(Category.CategoryName);
            return Ok("Category Added Successfully");
        }

        [HttpPost("DeleteCategory")]
        public IActionResult DeleteCategory([FromBody] CategoryModel Category)
        {
            if (CategoryStructure.GetCategoryById(Category.CateogryId) is null)
                return BadRequest("Category not found");

            CategoryStructure.DeleteCategory(Category.CateogryId);
            return Ok("Categoy deleted successfully");
        }


    }
}
