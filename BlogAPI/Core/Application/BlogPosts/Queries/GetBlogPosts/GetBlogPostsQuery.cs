using AutoMapper;
using BlogAPI.Core.Application.BlogPosts.DTO;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Core.Application.Tags.DTO;
using BlogAPI.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogAPI.Core.Application.BlogPosts.Queries.GetBlogPosts
{
    public class GetBlogPostsQuery : IRequest<List<BlogPostDTO>>
    {
        public GetBlogPostsQuery(string tag)
        {
            Tag = tag;
        }
        public string Tag { get; set; }
    }
    public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, List<BlogPostDTO>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetBlogPostsQueryHandler(IApplicationDbContext applicationDbContext,
                                        IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<List<BlogPostDTO>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _applicationDbContext.BlogPosts
                                .AsNoTracking()
                                .Include(x => x.PostTags)
                                .ThenInclude(x => x.Tag)
                                .Where(GetExpression(request))
                                .Select(x => new BlogPostDTO()
                                {
                                    Body = x.Body,
                                    Title = x.Title,
                                    Slug = x.Slug,
                                    CreatedAt = x.CreatedAt,
                                    UpdatedAt = x.UpdatedAt,
                                    Description = x.Description,
                                    TagList = x.PostTags.Select(y => y.Tag.Name).ToList()
                                })
                                .OrderByDescending(x => x.CreatedAt).ToListAsync();

            return _mapper.Map<List<BlogPostDTO>>(posts);
        }
        private Expression<Func<BlogPost, bool>> GetExpression(GetBlogPostsQuery request)
        {
            if (request.Tag != null)
            {
                return x => _applicationDbContext.PostTags
                                            .Any(y => y.BlogPostId == x.Id &&
                                                      y.Tag.Name == request.Tag);
            }
            return x => true;
        }
    }
}
