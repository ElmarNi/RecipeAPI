using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RecipeAPI.Model;

namespace RecipeAPI.DAL
{
	public class RecipeDbContext: IdentityDbContext<AppUser>
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options) { }
        public DbSet<Category> categories { get; set; }
        public DbSet<Recipe> recipes { get; set; }
    }
}
