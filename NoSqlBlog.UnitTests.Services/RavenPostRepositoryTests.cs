namespace NoSqlBlog.UnitTests.Services
{
    using System;
    using System.Threading;
    using Core.Models;
    using NUnit.Framework;
    using NoSqlBlog.Services;
    using System.Linq;
    using Raven.Client;
    using Raven.Client.Embedded;

    [TestFixture]
    public class RavenPostRepositoryTests : PostRepositoryTestBase
    {
        protected IDocumentStore _store;
        protected IDocumentSession _session;

        [SetUp]
        public void SetUp()
        {
            _store = Time("Get Raven Store", () => new EmbeddableDocumentStore() { RunInMemory = true });
            Time("Init Store", () => _store.Initialize());
            _session = Time("Get Session", () => _store.OpenSession());
        }

        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
            _store.Dispose();
        }

        [Test]
        public override void RepositoryReturnsSpecifiedNumberOfPosts()
        {
            var oldPost = new Post {CreatedAt = DateTime.Now.AddMinutes(-5)};
            var newPost = new Post {CreatedAt = DateTime.Now};

            _session.Store(oldPost);
            _session.Store(newPost);
            _session.SaveChanges();

            var repo = new RavenPostRepository(_store);
            var result = repo.GetRecentPosts(1).ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
        }

        [Test]
        public override void RecentPostsReturnInCorrectOrder()
        {
            var oldPost = new Post { CreatedAt = DateTime.Now.AddMinutes(-5), Title = "Old" };
            const string expected = "New";
            var newPost = new Post { CreatedAt = DateTime.Now, Title = expected };

            _session.Store(oldPost);
            _session.Store(newPost);
            _session.SaveChanges();

            var repo = new RavenPostRepository(_store);
            var result = repo.GetRecentPosts(2).ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo(expected));
        }

        [Test]
        public void PostCanBeInserted()
        {
            const string author = "Jason";
            var post = new Post() {Author = author, Title = "Wow", Content = "Yep", Tags = new[] {"Hot"}};
            _session.Store(post);
            _session.SaveChanges();
            ClearStaleIndexes();
            var posts = _session.Query<Post>().ToArray();
            Assert.That(posts.Length, Is.EqualTo(1));
            Assert.That(posts.Single().Author, Is.EqualTo(author));
        }

        [Test]
        public void PostHasIdAfterSaving()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _session.Store(post);
            _session.SaveChanges();
            ClearStaleIndexes();
            var posts = _session.Query<Post>().ToArray();
            Assert.That(posts.Length, Is.EqualTo(1));
            Assert.That(posts.Single().Id, Is.Not.Null);
            Assert.That(posts.Single().Id, Is.GreaterThan(0));
        }

        private void ClearStaleIndexes()
        {
            while (_store.DatabaseCommands.GetStatistics().StaleIndexes.Length > 0)
            {
                Thread.Sleep(10);
            }
        }
    }
}
