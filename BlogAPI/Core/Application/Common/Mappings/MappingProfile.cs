using AutoMapper;
using BlogAPI.Core.Application.BlogPosts.DTO;
using BlogAPI.Core.Application.Comments.DTO;
using BlogAPI.Core.Domain.Entities;

namespace BlogAPI.Core.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BlogPost, BlogPostDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
