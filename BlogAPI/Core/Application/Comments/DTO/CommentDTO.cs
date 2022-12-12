namespace BlogAPI.Core.Application.Comments.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Body { get; set; }
    }
}
