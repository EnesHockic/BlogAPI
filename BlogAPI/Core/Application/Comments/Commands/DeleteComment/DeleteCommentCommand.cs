using BlogAPI.Core.Application.Common.Exceptions;
using BlogAPI.Core.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Core.Application.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public DeleteCommentCommand(string slug, int id)
        {
            Slug = slug;
            CommentId = id;
        }
        public string Slug { get; set; }
        public int CommentId { get; set; }
    }
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteCommentCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _applicationDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug));
            if (post == null)
            {
                throw new NotFoundException("Blog Post doesn't exist!");
            }
            var comment = await _applicationDbContext.Comments
                                .FirstOrDefaultAsync(x => x.Id.Equals(request.CommentId) && x.BlogPostId == post.Id);
            if (comment == null)
            {
                throw new NotFoundException("Comment doesn't exist!");
            }

            _applicationDbContext.Comments.Remove(comment);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
