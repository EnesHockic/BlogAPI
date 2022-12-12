namespace BlogAPI.Model.BlogPosts
{
    public class AddBlogPostViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public List<string>? TagList { get; set; }
    }
}
