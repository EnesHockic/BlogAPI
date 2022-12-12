using AutoMapper;
using BlogAPI.Core.Application.BlogPosts.DTO;
using BlogAPI.Core.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Core.Application.BlogPosts.Queries.GetBlogPost
{
    public class GetBlogPostQuery : IRequest<BlogPostDTO>
    {
        public GetBlogPostQuery(string slug)
        {
            Slug = slug;
        }
        public string Slug { get; set; }
    }
    public class GetBlogPostQueryHandler : IRequestHandler<GetBlogPostQuery, BlogPostDTO>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetBlogPostQueryHandler(IApplicationDbContext applicationDbContext,
                                        IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<BlogPostDTO> Handle(GetBlogPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _applicationDbContext.BlogPosts
                                .AsNoTracking()
                                .Include(x => x.PostTags)
                                .ThenInclude(x => x.Tag)
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
                                .FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug));
            
            return post;
        }
    }
}
