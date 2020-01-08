namespace AdminAssistant.Domain.Blog
{
    public class PostTag : IEntity
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
