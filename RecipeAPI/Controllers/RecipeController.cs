using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public RecipeController(RecipeDbContext context, ILogger<CategoryController> logger, UserManager<AppUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
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

        [HttpPost(Name = "AddFavorite")]
        public async Task<StatusCodeResult> AddFavorite([FromBody] FavoriteRequest request)
        {
            if (!_context.favorites.Any(f => f.UserId == request.UserId && f.RecipeId == request.RecipeId))
            {
                Favorite favorite = new Favorite
                {
                    UserId = request.UserId,
                    RecipeId = request.RecipeId
                };
                await _context.favorites.AddAsync(favorite);
                await _context.SaveChangesAsync();
            }
            return StatusCode(200);
        }

        [HttpPost(Name = "RemoveFavorite")]
        public async Task<StatusCodeResult> RemoveFavorite([FromBody] FavoriteRequest request)
        {
            Favorite favorite = await _context.favorites.Where(f => f.UserId == request.UserId && f.RecipeId == request.RecipeId).FirstOrDefaultAsync();
            if (favorite != null)
            {
                _context.favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            return StatusCode(200);
        }

        [HttpGet(Name = "GetFavoriteRecipesByUserId")]
        public IEnumerable<Recipe> GetFavoriteRecipesByUserId(string userId)
        {
            List<Favorite> favorites = _context.favorites.Where(f => f.UserId == userId).Include(f => f.Recipe)
                                                                                        .Include(f => f.Recipe.Category)
                                                                                        .ToList();
            List<Recipe> recipes = new List<Recipe>();
            foreach (var favorite in favorites)
                recipes.Add(favorite.Recipe);
            return recipes.OrderBy(r => r.Name);
        }

        [HttpGet(Name = "GetRecipesByUserId")]
        public async Task<IEnumerable<Recipe>> GetRecipesByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            List<Recipe> recipes = new List<Recipe>();
            if (user != null)
                recipes.AddRange(_context.recipes.Where(r => r.AuthorName == user.UserName).Include(r => r.Category).ToList());
            return recipes;
        }

        [HttpPost(Name = "RemoveRecipe")]
        public async Task<StatusCodeResult> RemoveRecipe(int recipeId)
        {
            Recipe recipe = await _context.recipes.FindAsync(recipeId);
            if (recipe != null)
            {
                _context.recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
            return StatusCode(200);
        }

        [HttpPost(Name = "Like")]
        public async Task<StatusCodeResult> Like([FromBody] FavoriteRequest request)
        {
            if (!_context.likes.Any(f => f.UserId == request.UserId && f.RecipeId == request.RecipeId))
            {
                Like like = new Like
                {
                    UserId = request.UserId,
                    RecipeId = request.RecipeId
                };
                Recipe recipe = await _context.recipes.FindAsync(request.RecipeId);
                recipe.Likes++;
                await _context.likes.AddAsync(like);
                await _context.SaveChangesAsync();
            }
            return StatusCode(200);
        }

        [HttpPost(Name = "UnLike")]
        public async Task<StatusCodeResult> UnLike([FromBody] FavoriteRequest request)
        {
            Like like = await _context.likes.Where(f => f.UserId == request.UserId && f.RecipeId == request.RecipeId).FirstOrDefaultAsync();
            if (like != null)
            {
                _context.likes.Remove(like);
                Recipe recipe = await _context.recipes.FindAsync(request.RecipeId);
                recipe.Likes--;
                await _context.SaveChangesAsync();
            }
            return StatusCode(200);
        }

        [HttpGet(Name = "GetLikedRecipesByUserId")]
        public IEnumerable<Recipe> GetLikedRecipesByUserId(string userId)
        {
            List<Like> likes = _context.likes.Where(f => f.UserId == userId).Include(f => f.Recipe)
                                                                                        .Include(f => f.Recipe.Category)
                                                                                        .ToList();
            List<Recipe> recipes = new List<Recipe>();
            foreach (var favorite in likes)
                recipes.Add(favorite.Recipe);
            return recipes.OrderBy(r => r.Name);
        }

        [HttpGet(Name = "GetRecipe")]
        public async Task<Recipe> GetRecipe(int recipeId)
        {
            return await _context.recipes.Where(r => r.Id == recipeId).Include(r => r.Category).FirstOrDefaultAsync();
        }

        [HttpPost(Name = "AddRecipe")]
        public async Task<StatusCodeResult> AddRecipe([FromBody] Recipe recipe)
        {
            await _context.recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}

