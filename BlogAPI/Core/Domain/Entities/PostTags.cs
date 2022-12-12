#nullable disable

namespace BlogAPI.Core.Domain.Entities
{
    public class PostTags
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual BlogPost BlogPost { get; set; }
    }
}
