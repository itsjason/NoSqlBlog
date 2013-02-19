namespace NoSqlBlog.Core.Models
{
    using System;

    public class Comment
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public Comment()
        {
            CreatedAt = DateTime.Now;
        }
    }
}