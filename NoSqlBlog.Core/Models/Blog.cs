namespace NoSqlBlog.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Blog : Entity
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Post> Posts { get; set; }

        public Blog()
        {
            Posts = new Collection<Post>();
        }
    }
}