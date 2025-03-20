using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly BlogDbContext _dbcontext;

        public CommentController(BlogDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // 1️⃣ Add a comment to a BlogPost
        //[Authorize] // Only authenticated users can comment
        [HttpPost("post-comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostComment([FromBody] CommentDTO commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blogPost = await _dbcontext.BlogPosts.FindAsync(commentDto.BlogPostId);
            if (blogPost == null)
                return NotFound("Blog post not found.");

            var user = await _dbcontext.Users.FindAsync(commentDto.UserId);
            if (user == null)
                return NotFound("User not found.");

            var comment = new Comment
            {
                Content = commentDto.Content,
                BlogPostId = commentDto.BlogPostId,
                UserId = commentDto.UserId
            };

            _dbcontext.Comments.Add(comment);
            await _dbcontext.SaveChangesAsync();

            return Ok(new { message = "Comment added successfully!" });
        }


        // 2️⃣ Get all comments for a BlogPost
        [HttpGet("{blogPostId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentsForBlogPost(int blogPostId)
        {
            var comments = await _dbcontext.Comments
                .Where(c => c.BlogPostId == blogPostId)
                .ToListAsync();

            return Ok(comments);
        }

        // 3️⃣ Get a single comment by ID
        [HttpGet("comment/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _dbcontext.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }
            return Ok(comment);
        }
    }
}
