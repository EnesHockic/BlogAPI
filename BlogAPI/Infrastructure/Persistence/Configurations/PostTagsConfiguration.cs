using BlogAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPI.Infrastructure.Persistence.Configurations
{
    public class PostTagsConfiguration : IEntityTypeConfiguration<PostTags>
    {
        public void Configure(EntityTypeBuilder<PostTags> entity)
        {
            entity.HasOne(e => e.BlogPost)
                .WithMany(b => b.PostTags)
                .HasForeignKey(e => e.BlogPostId);

            entity.HasOne(e => e.Tag)
                .WithMany(b => b.PostTags)
                .HasForeignKey(e => e.TagId);
        }
    }
}
