using BlogAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Core.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<BlogPost> BlogPosts { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<PostTags> PostTags { get; set; }
        DbSet<Comment> Comments { get; set; } 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
