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

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _store = Time("Get Raven Store", () => new EmbeddableDocumentStore() { RunInMemory = true });
            Time("Init Store", () => _store.Initialize());
        }

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            _store.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            
            _session = Time("Get Session", () => _store.OpenSession());
            _repository = new RavenPostRepository(_store);
        }

        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
        }

        [Test]
        public override void PostCanBeAdded()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            WaitForIndexesToBeCurrent();
            var result = _repository.GetById(post.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public override void PostCanBeDeleted()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            WaitForIndexesToBeCurrent();
            _repository.DeletePost(post);
            WaitForIndexesToBeCurrent();
            var result = _repository.GetById(post.Id);
            Assert.That(result, Is.Null);
        }

        [Test]
        public override void GetRecentPostsReturnsSpecifiedNumberOfPosts()
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
            const string expected = "New Post";

            var oldPost = new Post { CreatedAt = DateTime.Now.AddMinutes(-5), Title = "Old" };
            var newPost = new Post { Title = expected };

            _repository.AddPost(oldPost);
            _repository.AddPost(newPost);
            WaitForIndexesToBeCurrent();

            var result = _repository.GetRecentPosts(2).ToArray();

            Assert.That(result.First().Title, Is.EqualTo(expected));
        }

        [Test]
        public override void PostHasIdAfterSaving()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            Assert.That(post.Id, Is.GreaterThan(0));
        }

        [Test]
        public override void CanAddACommentToAnExistingPost()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            WaitForIndexesToBeCurrent();
            const string commenter = "Commenter";
            post.Comments.Add(new Comment {Name = commenter, Email = "comment@comment.com", Content = "Halla!"});
            _repository.UpdatePost(post);
            WaitForIndexesToBeCurrent();
            var result = _repository.GetById(post.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(1));
            Assert.That(result.Comments.Single().Name, Is.EqualTo(commenter));
        }

        [Test]
        public override void CommentsOnAPostAreReturnedInChronologicalOrder()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            WaitForIndexesToBeCurrent();
            const string firstCommentName = "Commenter 1";
            post.Comments.Add(new Comment { Name = firstCommentName, Email = "comment@comment.com", Content = "Halla!" });
            post.Comments.Add(new Comment { Name = "Commenter 2", Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);
            WaitForIndexesToBeCurrent();
            var result = _repository.GetById(post.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(2));
            Assert.That(result.Comments.First().Name, Is.EqualTo(firstCommentName));
        }

        [Test]
        public override void CommentsAreStillInOrderWhenOneIsAddedLater()
        {
            const string firstCommentName = "Commenter 1";
            const string thirdCommentName = "Commenter 3";

            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            WaitForIndexesToBeCurrent();
            
            post.Comments.Add(new Comment { Name = firstCommentName, Email = "comment@comment.com", Content = "Halla!" });
            post.Comments.Add(new Comment { Name = "Commenter 2", Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);
            WaitForIndexesToBeCurrent();

            post = _repository.GetById(post.Id);
            post.Comments.Add(new Comment { Name = thirdCommentName, Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);
            WaitForIndexesToBeCurrent();

            var result = _repository.GetById(post.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(3));
            Assert.That(result.Comments.First().Name, Is.EqualTo(firstCommentName));
            Assert.That(result.Comments.Last().Name, Is.EqualTo(thirdCommentName));
        }

        private void WaitForIndexesToBeCurrent()
        {
            while (_store.DatabaseCommands.GetStatistics().StaleIndexes.Length > 0)
            {
                Thread.Sleep(10);
            }
        }
    }
}
