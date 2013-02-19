using System.Collections.Generic;

namespace NoSqlBlog.Services
{
    using System;
    using Core.Models;
    using Raven.Client;
    using System.Linq;

    public class RavenPostRepository
    {
        private readonly IDocumentStore _store;

        public RavenPostRepository(IDocumentStore store)
        {
            _store = store;
        }

        public IEnumerable<Post> GetRecentPosts(int postCount)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Post>().OrderByDescending(p => p.CreatedAt).Take(postCount);
            }
        }

        public void AddPost(Post post)
        {
            
        }

        public void DeletePost(Post post)
        {
            
        }
    }
}
