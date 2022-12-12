using BlogAPI.Core.Application.BlogPosts.Commands.AddBlogPost;
using BlogAPI.Core.Application.BlogPosts.Commands.DeleteBlogPost;
using BlogAPI.Core.Application.BlogPosts.Commands.UpdateBlogPost;
using BlogAPI.Core.Application.BlogPosts.Commands.UpdateBlogPostTags;
using BlogAPI.Core.Application.BlogPosts.Queries.GetBlogPost;
using BlogAPI.Core.Application.BlogPosts.Queries.GetBlogPosts;
using BlogAPI.Core.Application.Comments.Commands.AddComment;
using BlogAPI.Core.Application.Comments.Commands.DeleteComment;
using BlogAPI.Core.Application.Comments.Queries.GetCommentsFromPost;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Model.BlogPosts;
using BlogAPI.Model.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class BlogPostController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] string? tag)
        {
            var posts = await Mediator.Send(new GetBlogPostsQuery(tag)).ConfigureAwait(false);

            return Ok(posts);
        }
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetPost(string slug)
        {
            var post = await Mediator.Send(new GetBlogPostQuery(slug)).ConfigureAwait(false);

            return Ok(post);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(AddBlogPostViewModel post)
        {
            var result = await Mediator.Send(new AddBlogPostCommand(post)).ConfigureAwait(false);

            if (post.TagList != null)
            {
                await Mediator.Send(new UpdateBlogPostTagsCommand(result.Slug, post.TagList)).ConfigureAwait(false);
            }
            return Ok(result);
        }
        [HttpDelete("slug")]
        public async Task<IActionResult> DeletePost(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException("Post details are not correct!");
            }

            await Mediator.Send(new DeleteBlogPostCommand(slug)).ConfigureAwait(false);

            return Ok("Post successfuly deleted!");
        }
        [HttpPut("{slug}")]
        public async Task<IActionResult> UpdatePost(string slug, UpdateBlogPostViewModel post)
        {
            
            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException("Post details are not correct!");
            }
            var result = await Mediator.Send(new UpdateBlogPostCommand(slug, post)).ConfigureAwait(false);
            return Ok(result);
        }
        [HttpGet("{slug}/comments")]
        public async Task<IActionResult> GetComments(string slug)
        {
            var comments = await Mediator.Send(new GetCommentsFromPostQuery(slug));
            return Ok(comments);
        }
        [HttpPost("{slug}/comments")]
        public async Task<IActionResult> AddComment(string slug, CommentViewModel comment)
        {
            if (string.IsNullOrEmpty(slug))
            {
                throw new ArgumentNullException("Post details are not correct!");
            }
            var result = await Mediator.Send(new AddCommentCommand(slug, comment)).ConfigureAwait(false);
            return Ok(result);
        }
        [HttpDelete("{slug}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string slug, int id)
        {
            if (string.IsNullOrEmpty(slug) || id == 0)
            {
                throw new ArgumentNullException("Post/Comment details are not correct!");
            }
            await Mediator.Send(new DeleteCommentCommand(slug, id)).ConfigureAwait(false);
            return Ok("Comment successfuly deleted!");
        }
    }
}
