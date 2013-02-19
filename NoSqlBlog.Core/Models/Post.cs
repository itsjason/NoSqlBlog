namespace NoSqlBlog.Core.Models
{
    using System;

    public class Post : Entity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public string[] Tags { get; set; }
        public string Slug { get; set; }
    }
}