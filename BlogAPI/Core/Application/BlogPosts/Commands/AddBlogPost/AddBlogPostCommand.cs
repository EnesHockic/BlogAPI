using AutoMapper;
using BlogAPI.Core.Application.BlogPosts.DTO;
using BlogAPI.Core.Application.Common.Exceptions;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Model.BlogPosts;
using MediatR;

namespace BlogAPI.Core.Application.BlogPosts.Commands.AddBlogPost
{
    public class AddBlogPostCommand : IRequest<BlogPostDTO>
    {
        public AddBlogPostCommand(AddBlogPostViewModel blogPost)
        {
            Title = blogPost.Title;
            Description = blogPost.Description;
            Body = blogPost.Body;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
    }
    public class AddBlogPostCommandHandler : IRequestHandler<AddBlogPostCommand, BlogPostDTO>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ISlugHelper _slugHelper;

        public AddBlogPostCommandHandler(IApplicationDbContext applicationDbContext,
                                        IMapper mapper,ISlugHelper slugHelper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _slugHelper = slugHelper;
        }
        public async Task<BlogPostDTO> Handle(AddBlogPostCommand request, CancellationToken cancellationToken)
        {
            var slug = _slugHelper.CreateSlug(request.Title);

            var existingPost = _applicationDbContext.BlogPosts.FirstOrDefault(b => b.Slug == slug);

            if(existingPost != null)
            {
                throw new DuplicateException("Blog Post with the same title already exists!");
            }

            var blogPost = new BlogPost()
            {
                Slug = slug,
                Title = request.Title,
                Description = request.Description,
                Body = request.Body
            };
            _applicationDbContext.BlogPosts.Add(blogPost);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BlogPostDTO>(blogPost);
        }
    }
}
