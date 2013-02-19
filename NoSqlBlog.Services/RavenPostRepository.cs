using System.Collections.Generic;

namespace NoSqlBlog.Services
{
    using Core.Interfaces;
    using Core.Models;
    using Raven.Client;
    using System.Linq;
    using Raven.Client.Document;
    using Raven.Client.Embedded;

    public class RavenPostRepository : IPostRepository
    {
        private readonly IDocumentStore _store;

        public RavenPostRepository(IDocumentStore store)
        {
            _store = store;
        }

        public RavenPostRepository()
        {
            _store = new EmbeddableDocumentStore();
            _store.Initialize();
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
            using (var session = _store.OpenSession())
            {
                session.Store(post);
                session.SaveChanges();
            }
        }

        public void DeletePost(Post post)
        {
            using (var session = _store.OpenSession())
            {
                session.Delete(session.Load<Post>(post.Id));
                session.SaveChanges();
            }
        }

        public void UpdatePost(Post post)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(post);
                session.SaveChanges();
            }
        }

        public Post GetById(int id)
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<Post>(id);
            }
        }
    }
}
