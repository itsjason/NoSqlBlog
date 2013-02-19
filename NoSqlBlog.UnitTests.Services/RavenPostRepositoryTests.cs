namespace NoSqlBlog.UnitTests.Services
{
    using System;
    using System.Threading;
    using Core.Interfaces;
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
        protected IPostRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _store = Time("Get Raven Store", () => new EmbeddableDocumentStore() { RunInMemory = true });
            Time("Init Store", () => _store.Initialize());
            _session = Time("Get Session", () => _store.OpenSession());
            _repository = new RavenPostRepository(_store);
        }

        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
            _store.Dispose();
        }

        [Test]
        public void PostIsAdded()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            ClearStaleIndexes();
            var results = _repository.GetRecentPosts(2).ToArray();
            Assert.That(results.Length, Is.EqualTo(1));
        }

        [Test]
        public void PostIsDeleted()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            ClearStaleIndexes();
            _repository.DeletePost(post);
            ClearStaleIndexes();
            var results = _repository.GetRecentPosts(1).ToArray();
            Assert.That(results.Length, Is.EqualTo(0));
        }

        [Test]
        public override void RepositoryReturnsSpecifiedNumberOfPosts()
        {
            var oldPost = new Post {CreatedAt = DateTime.Now.AddMinutes(-5)};
            var newPost = new Post {CreatedAt = DateTime.Now};

            _repository.AddPost(oldPost);
            _repository.AddPost(newPost);

            var result = _repository.GetRecentPosts(1).ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
        }

        [Test]
        public override void RecentPostsReturnInCorrectOrder()
        {
            var oldPost = new Post { CreatedAt = DateTime.Now.AddMinutes(-5), Title = "Old" };
            const string expected = "New";
            var newPost = new Post { CreatedAt = DateTime.Now, Title = expected };

            _repository.AddPost(oldPost);
            _repository.AddPost(newPost);

            var result = _repository.GetRecentPosts(2).ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result.First().Title, Is.EqualTo(expected));
        }

        [Test]
        public void PostHasIdAfterSaving()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            ClearStaleIndexes();
            var posts = _repository.GetRecentPosts(2).ToArray();
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
