using BlogAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPI.Infrastructure.Persistence.Configurations
{
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> entity)
        {
            entity.ToTable("BlogPosts");

            entity.Property(e => e.Slug).HasMaxLength(250);

            entity.Property(e => e.Title).HasMaxLength(250);

            entity.Property(e => e.Description).HasMaxLength(250);

            entity.Property(e => e.Body).HasColumnType("varchar(Max)");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        }
    }
}
