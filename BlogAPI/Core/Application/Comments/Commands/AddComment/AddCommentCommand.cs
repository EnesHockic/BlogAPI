using AutoMapper;
using BlogAPI.Core.Application.Comments.DTO;
using BlogAPI.Core.Application.Common.Exceptions;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Model.Comments;
using MediatR;

namespace BlogAPI.Core.Application.Comments.Commands.AddComment
{
    public class AddCommentCommand : IRequest<CommentDTO>
    {
        public AddCommentCommand(string slug, CommentViewModel comment)
        {
            Slug = slug;
            Body = comment.Body;
        }
        public string Slug { get; set; }
        public string Body { get; set; }
    }
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDTO>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public AddCommentCommandHandler(IApplicationDbContext applicationDbContext,
                                        IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var post = _applicationDbContext.BlogPosts.FirstOrDefault(x => x.Slug.Equals(request.Slug));

            if (post == null)
            {
                throw new NotFoundException("Blog Post doesn't exist!");
            }
            
            var comment = new Comment()
            {
                BlogPostId = post.Id,
                Body = request.Body,
            };
            _applicationDbContext.Comments.Add(comment);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CommentDTO>(comment);

        }
    }
}
