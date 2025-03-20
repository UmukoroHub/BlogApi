using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models
{
    public class BlogPostDTO
    {
        public int Id { get; set; }  // Added this for batch updates
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
