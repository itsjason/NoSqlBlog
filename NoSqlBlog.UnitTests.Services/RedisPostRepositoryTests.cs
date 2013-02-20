namespace NoSqlBlog.UnitTests.Services
{
    using System;
    using System.Collections.Generic;

    using Core.Models;
    using NUnit.Framework;
    using NoSqlBlog.Services;

    using System.Linq;

    [TestFixture]
    public class RedisPostRepositoryTests : PostRepositoryTestBase
    {
        private RedisPostRepository _repo;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            const string Host = "localhost"; // "do.jnwebdev.com";
            const int Port = 6379; // 9876;
            const string Password = null; // "whatwhat"
            _repo = new RedisPostRepository(Host, Port, Password); 
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
        public void PostsHaveUniqueIds()
        {
            var posts = new List<Post>();

            const int PostCount = 1000;

            for (var i = 0; i < PostCount; i++)
            {
                var j = i.ToString();
                posts.Add(new Post() { Author = "Jason " + j, Title = "Wow + j", Content = "Yep + j", Tags = new[] { "Hot" + j } });
            }

            Console.WriteLine("{0} Posts Created.", PostCount);

            foreach (var post in posts)
            {
                _repo.AddPost(post);
                if (post.Id % 100 == 0)
                {
                    Console.WriteLine("Post {0} Added to Repo.", post.Id);
                }
            }

            var ids = posts.Select(p => p.Id);
            var uniqueIds = posts.Select(p => p.Id).Distinct();

            Assert.That(ids.Count(), Is.EqualTo(uniqueIds.Count()));
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