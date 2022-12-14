#nullable disable

namespace BlogAPI.Core.Application.BlogPosts.DTO
{
    public class BlogPostDTO
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> TagList { get; set; }
    }
}
