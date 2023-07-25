using System;
namespace RecipeAPI.Model
{
	public class Like
	{
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}

