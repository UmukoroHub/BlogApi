using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        [Required]
        public string UserId { get; set; }  // IdentityUser Id is a string

       // [ForeignKey("UserId")]
        public ApplicationUser Author { get; set; }  // Navigation Property

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
