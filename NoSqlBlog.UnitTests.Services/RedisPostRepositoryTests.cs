namespace NoSqlBlog.UnitTests.Services
{
    using System;
    using Core.Models;
    using NUnit.Framework;
    using NoSqlBlog.Services;

    [TestFixture]
    public class RedisPostRepositoryTests : PostRepositoryTestBase
    {
        private RedisPostRepository _repo;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            var host = "do.jnwebdev.com";
            var port = 9876;
            _repo = new RedisPostRepository(host, port, "whatwhat");
        }

        [Test]
        public override void GetRecentPostsReturnsSpecifiedNumberOfPosts()
        {
            throw new NotImplementedException();
        }

        [Test]
        public override void RecentPostsReturnInCorrectOrder()
        {
            throw new NotImplementedException();
        }

        [Test]
        public override void PostCanBeAdded()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repo.AddPost(post);
        }

        [Test]
        public override void PostCanBeDeleted()
        {
            throw new NotImplementedException();
        }

        [Test]
        public override void PostHasIdAfterSaving()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repo.AddPost(post);
            Assert.That(post.Id, Is.GreaterThan(0));
        }

        [Test]
        public override void CanAddACommentToAnExistingPost()
        {
            throw new NotImplementedException();
        }

        [Test]
        public override void CommentsOnAPostAreReturnedInChronologicalOrder()
        {
            throw new NotImplementedException();
        }

        [Test]
        public override void CommentsAreStillInOrderWhenOneIsAddedLater()
        {
            throw new NotImplementedException();
        }
    }
}