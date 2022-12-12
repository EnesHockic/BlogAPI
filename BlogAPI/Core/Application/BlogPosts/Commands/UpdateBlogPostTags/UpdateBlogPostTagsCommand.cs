using BlogAPI.Core.Application.Common.Exceptions;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Core.Application.BlogPosts.Commands.UpdateBlogPostTags
{
    public class UpdateBlogPostTagsCommand : IRequest<bool>
    {
        public UpdateBlogPostTagsCommand(string slug, List<string> tagList)
        {
            Slug = slug;
            TagList = tagList;
        }
        public string Slug { get; set; }
        public List<string> TagList { get; set; }
    }
    public class UpdateBlogPostTagsCommandHandler : IRequestHandler<UpdateBlogPostTagsCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateBlogPostTagsCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(UpdateBlogPostTagsCommand request, CancellationToken cancellationToken)
        {
            var post = await _applicationDbContext.BlogPosts
                .FirstOrDefaultAsync(x => x.Slug == request.Slug);

            if (post == null)
            {
                throw new NotFoundException("Blog Post doesn't exist!");
            }

            var tags = await _applicationDbContext.Tags.ToListAsync(cancellationToken);

            var newTags = new List<PostTags>();
            var tagsToRemove = new List<PostTags>();
            var postTags = await _applicationDbContext.PostTags
                .Include(x => x.Tag)
                .Where(x => x.BlogPostId == post.Id)
                .ToListAsync();

            tagsToRemove.AddRange(postTags.Where(x => !request.TagList.Any(y => x.Tag.Name == y)));
            newTags.AddRange(request.TagList
                                    .Where(x => !postTags.Any(y => y.Tag.Name == x))
                                    .Select(x => new PostTags() 
                                                { 
                                                    BlogPostId = post.Id,
                                                    TagId = tags.FirstOrDefault(y => y.Name == x)!.Id
                                    }));

            _applicationDbContext.PostTags.RemoveRange(tagsToRemove);
            _applicationDbContext.PostTags.AddRange(newTags);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
