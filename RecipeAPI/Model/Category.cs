using System;
namespace RecipeAPI.Model
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? ImageUrl { get; set; }
		public string? IconUrl { get; set; }
	}
}
