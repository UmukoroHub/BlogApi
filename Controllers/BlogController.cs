using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly BlogDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5); // Cache expiry
        public BlogController(ILogger<BlogController> logger, BlogDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _logger = logger;
            _cache = cache;

        }

        // GET: api/blog/all
        [HttpGet("All", Name = "GetAllBlogPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBlogPost()
        {
            string cacheKey = "all_blog_posts";

            // Check if data is in cache
            if (_cache.TryGetValue(cacheKey, out List<BlogPost> cachedBlogs))
            {
                _logger.LogInformation("Returning cached blog posts.");
                return Ok(cachedBlogs);
            }

            _logger.LogInformation("Fetching all blog posts.");
            var blogposts = await _dbContext.BlogPosts.Include(b => b.Comments).ToListAsync();

            if (!blogposts.Any())
            {
                _logger.LogWarning("No blog posts found.");
                return NotFound("No blog posts available.");
            }

            // Store in cache for 5 minutes
            _cache.Set(cacheKey, blogposts, _cacheDuration);

            _logger.LogInformation("Successfully fetched {Count} blog posts.", blogposts.Count);
            return Ok(blogposts);
        }

        // GET: api/blog/1
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlogPostById(int id)
        {
            _logger.LogInformation("Fetching blog post with ID: {Id}", id);
            var blogpost = await _dbContext.BlogPosts.FindAsync(id);

            if (blogpost == null)
            {
                _logger.LogWarning("Blog post with ID {id} not found.", id);
                return NotFound($"Blog post with ID {id} not found.");
            }

            _logger.LogInformation("Successfully fetched blog post with ID {Id}.", id);
            return Ok(blogpost);
        }

        // GET: api/blog/title
        [HttpGet("{title}", Name = "GetAllBlogPostByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlogPostByName(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.LogWarning("Search term for blog post title is empty.");
                return BadRequest("Search term cannot be empty.");
            }

            _logger.LogInformation("Searching blog posts with title containing '{Title}'", title);
            var blogposts = await _dbContext.BlogPosts
                .Where(b => EF.Functions.Like(b.Title, $"%{title}%"))
                .ToListAsync();

            if (!blogposts.Any())
            {
                _logger.LogWarning("No blog posts found for title '{Title}'", title);
                return NotFound($"No blog posts found with title containing '{title}'.");
            }

            _logger.LogInformation("Found {Count} blog posts for title '{Title}'", blogposts.Count, title);
            return Ok(blogposts);
        }

        // POST: api/blog
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBlogPost(BlogPostDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while adding blog post.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Adding new blog post with title '{Title}'", model.Title);

            var blogpost = new BlogPost
            {
                Title = model.Title,
                Content = model.Content,

            };

            await _dbContext.BlogPosts.AddAsync(blogpost);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully added new blog post with ID {Id}", blogpost.Id);
            return CreatedAtRoute("GetAllBlogPost", new { id = blogpost.Id }, blogpost);
        }

        // PUT: api/blog/1
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditBlogPost(int id, BlogPostDTO blogPostDTO)
        {
            _logger.LogInformation("Editing blog post with ID {Id}", id);

            var blogpost = await _dbContext.BlogPosts.FindAsync(id);
            if (blogpost == null)
            {
                _logger.LogWarning("Blog post with ID {Id} not found for editing.", id);
                return NotFound($"Blog post with ID {id} not found.");
            }

            blogpost.Title = blogPostDTO.Title;
            blogpost.Content = blogPostDTO.Content;

            _dbContext.BlogPosts.Update(blogpost);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully edited blog post with ID {Id}.", id);
            return Ok(blogpost);
        }

        // DELETE: api/blog/1
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            _logger.LogInformation("Deleting blog post with ID {Id}", id);

            var blogpost = await _dbContext.BlogPosts.FindAsync(id);
            if (blogpost == null)
            {
                _logger.LogWarning("Blog post with ID {Id} not found for deletion.", id);
                return NotFound($"Blog post with ID {id} not found.");
            }

            _dbContext.BlogPosts.Remove(blogpost);
            await _dbContext.SaveChangesAsync();

            _cache.Remove(blogpost);

            _logger.LogInformation("Successfully deleted blog post with ID {Id}.", id);
            return Ok(blogpost);
        }
    }
}
