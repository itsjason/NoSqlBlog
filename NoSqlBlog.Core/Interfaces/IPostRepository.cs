namespace NoSqlBlog.Core.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IPostRepository
    {
        IEnumerable<Post> GetRecentPosts(int postCount);
        void AddPost(Post post);
        void DeletePost(Post post);
    }
}