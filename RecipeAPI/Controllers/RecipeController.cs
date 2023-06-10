using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.DAL;
using RecipeAPI.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RecipeController : Controller
    {
        private RecipeDbContext _context;
        private readonly ILogger<CategoryController> _logger;
        public RecipeController(RecipeDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetPopularRecipes")]
        public IEnumerable<Recipe> GetPopularRecipes()
        {
            return _context.recipes.Include(r => r.Category).OrderByDescending(r => r.Likes).Take(26).ToList();
        }

        [HttpGet(Name = "GetRecommendedRecipes")]
        public IEnumerable<Recipe> GetRecommendedRecipes()
        {
            return _context.recipes.Include(r => r.Category).OrderByDescending(r => Guid.NewGuid()).Take(10).ToList();
        }

        [HttpGet(Name = "Search")]
        public IEnumerable<Recipe> SearchRecipes(string query)
        {
            return _context.recipes.Include(r => r.Category).Where(r => r.Name.Contains(query)).ToList();
        }

        [HttpGet(Name = "GetRecipesByCategoryId")]
        public IEnumerable<Recipe> GetRecipesByCategoryId(int id)
        {
            return _context.recipes.Include(r => r.Category).Where(r => r.CategoryId == id).ToList();
        }
    }
}

