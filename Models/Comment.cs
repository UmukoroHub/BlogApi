using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models
{
    public class Comment
    {
            public int Id { get; set; }

            [Required]
            public string Content { get; set; }
            public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
            // Foreign Key for the blog post
            [Required]
            public int BlogPostId { get; set; }

            [ForeignKey("BlogPostId")]
            public BlogPost BlogPost { get; set; }

            // Foreign Key for the user who wrote the comment (optional for anonymous users)
            public string? UserId { get; set; }  // Can be null for anonymous comments

          //  [ForeignKey("UserId")]
            public ApplicationUser? User { get; set; }  // Navigation Property

    }
}
