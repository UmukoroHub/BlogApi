using Microsoft.AspNetCore.Identity;

namespace BlogApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public string FullName { get; set; }

        public string ProfilePictureUrl { get; set; }  // Optional field

        // Navigation property for BlogPosts
        public List<BlogPost> BlogPosts { get; set; } = new();
        // Navigation property for comments made by the user
        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
