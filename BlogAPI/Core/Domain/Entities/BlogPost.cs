#nullable disable

namespace BlogAPI.Core.Domain.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<PostTags> PostTags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
