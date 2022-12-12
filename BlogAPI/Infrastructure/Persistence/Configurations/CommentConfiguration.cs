using BlogAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPI.Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.Property(e => e.Body).HasColumnType("varchar(max)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(e => e.BlogPost)
                .WithMany(b => b.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(e => e.BlogPostId);
        }
    }
}
