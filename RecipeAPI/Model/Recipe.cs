using System;
namespace RecipeAPI.Model
{
	public class Recipe
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? AuthorName { get; set; }
		public string Description { get; set; }
		public string? Time { get; set; }
		public int? People { get; set; }
		public string? ImageUrl { get; set; }
		public int CategoryId { get; set; }
		public virtual Category Category { get; set; }
		public string Ingridients { get; set; }
		public int? Likes { get; set; }
	}
}

