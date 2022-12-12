using AutoMapper;
using BlogAPI.Core.Application.BlogPosts.DTO;
using BlogAPI.Core.Application.Common.Exceptions;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Model.BlogPosts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Core.Application.BlogPosts.Commands.UpdateBlogPost
{
    public class UpdateBlogPostCommand : IRequest<BlogPostDTO>
    {
        public UpdateBlogPostCommand(string slug, UpdateBlogPostViewModel blogPost)
        {
            Slug = slug;
            Title = blogPost.Title;
            Description = blogPost.Description;
            Body = blogPost.Body;
        }
        public string Slug { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Body { get; set; }
    }
    public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand, BlogPostDTO>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ISlugHelper _slugHelper;

        public UpdateBlogPostCommandHandler(IApplicationDbContext applicationDbContext,
                                            IMapper mapper, ISlugHelper slugHelper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _slugHelper = slugHelper;
        }
        public async Task<BlogPostDTO> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
        {

            var blogPost = await _applicationDbContext.BlogPosts
                                .FirstOrDefaultAsync(blog => blog.Slug == request.Slug);

            if(blogPost == null)
            {
                throw new NotFoundException("Blog Post doesn't exist!");
            }

            if(request.Title != null && !blogPost.Title.Equals(request.Title))
            {
                blogPost.Slug = _slugHelper.CreateSlug(request.Title);
                blogPost.Title = request.Title;
            }
            blogPost.Description = request.Description != null ? request.Description : blogPost.Description;
            blogPost.Body = request.Body != null ? request.Body : blogPost.Body;
            blogPost.UpdatedAt = DateTime.Now;

            _applicationDbContext.BlogPosts.Update(blogPost);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BlogPostDTO>(blogPost);
        }
    }
}
