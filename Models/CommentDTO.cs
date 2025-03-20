namespace BlogApi.Models
{
    public class CommentDTO
    {
        public string Content { get; set; }
        public int BlogPostId { get; set; }
        public string UserId { get; set; }
    }
}
