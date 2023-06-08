using System;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Model;

namespace RecipeAPI.DAL
{
	public class RecipeDbContext: DbContext
	{
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options) { }
        public DbSet<Category> categories { get; set; }
        public DbSet<Recipe> recipes { get; set; }
    }
}
