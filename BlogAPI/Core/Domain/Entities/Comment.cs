#nullable disable

namespace BlogAPI.Core.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int? BlogPostId { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual BlogPost BlogPost { get; set; }
    }
}
