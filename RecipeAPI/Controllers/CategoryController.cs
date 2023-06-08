using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.DAL;
using RecipeAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private RecipeDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(RecipeDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetCategories")]
        public IEnumerable<Category> GetCategories()
        {
            return _context.categories.ToList();
        }
    }
}

