using BlogAPI.Core.Application.Common.Exceptions;
using BlogAPI.Core.Application.Common.Interfaces;
using MediatR;

namespace BlogAPI.Core.Application.BlogPosts.Commands.DeleteBlogPost
{
    public class DeleteBlogPostCommand : IRequest<bool>
    {
        public DeleteBlogPostCommand(string slug)
        {
            Slug = slug;
        }
        public string Slug { get; set; }
    }
    public class DeleteBlogPostCommandHandler : IRequestHandler<DeleteBlogPostCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteBlogPostCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
        {
            var post = _applicationDbContext.BlogPosts.FirstOrDefault(x => x.Slug == request.Slug);

            if (post == null)
            {
                throw new NotFoundException("Blog Post doesn't exist!");
            }

            _applicationDbContext.BlogPosts.Remove(post);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
