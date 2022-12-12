using AutoMapper;
using BlogAPI.Core.Application.Comments.DTO;
using BlogAPI.Core.Application.Common.Interfaces;
using MediatR;

namespace BlogAPI.Core.Application.Comments.Queries.GetCommentsFromPost
{
    public class GetCommentsFromPostQuery : IRequest<List<CommentDTO>>
    {
        public GetCommentsFromPostQuery(string slug)
        {
            Slug = slug;
        }
        public string Slug { get; set; }
    }
    public class GetCommentsFromPostQueryHandler : IRequestHandler<GetCommentsFromPostQuery, List<CommentDTO>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetCommentsFromPostQueryHandler(IApplicationDbContext applicationDbContext,
                                        IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<List<CommentDTO>> Handle(GetCommentsFromPostQuery request, CancellationToken cancellationToken)
        {
            var comments = _applicationDbContext.Comments.Where(x => x.BlogPost.Slug.Equals(request.Slug));
            return _mapper.Map<List<CommentDTO>>(comments);
        }

    }
}
