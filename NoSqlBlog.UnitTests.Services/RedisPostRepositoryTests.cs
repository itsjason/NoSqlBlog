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
        private RedisPostRepository _repository;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            const string Host = "localhost"; // "do.jnwebdev.com";
            const int Port = 6379; // 9876;
            const string Password = null; // "whatwhat"
            this._repository = new RedisPostRepository(Host, Port, Password); 
        }

        [Test]
        public override void GetRecentPostsReturnsSpecifiedNumberOfPosts()
        {
            AddPosts(11);
            var posts = this._repository.GetRecentPosts(10).ToArray();
            Assert.That(posts.Length, Is.EqualTo(10));
        }

        [Test]
        public override void RecentPostsReturnInCorrectOrder()
        {
            var newTitle = "New Post " + Guid.NewGuid().ToString();
            var oldTitle = "Old Post " + Guid.NewGuid().ToString();

            var oldPost = new Post { CreatedAt = DateTime.Now.AddMinutes(-5), Title = oldTitle };
            var newPost = new Post { Title = newTitle };

            this._repository.AddPost(oldPost);
            this._repository.AddPost(newPost);

            const int PostCount = 2;
            var result = this._repository.GetRecentPosts(PostCount).ToArray();

            Assert.That(result.Length, Is.EqualTo(PostCount));
            Assert.That(result.First().Title, Is.EqualTo(newTitle));
            Assert.That(result.Last().Title, Is.EqualTo(oldTitle));
        }

        [Test]
        public override void PostCanBeAdded()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            this._repository.AddPost(post);
        }

        [Test]
        public override void PostCanBeDeleted()
        {
            var post = this._repository.GetRecentPosts(1).Single();
            this._repository.DeletePost(post);
            var retrievedPost = this._repository.GetById(post.Id);
            Assert.That(retrievedPost, Is.Null);
        }

        [Test]
        public override void PostHasIdAfterSaving()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            this._repository.AddPost(post);
            Assert.That(post.Id, Is.GreaterThan(0));
        }

        [Test]
        public void PostsHaveUniqueIds()
        {
            const int PostCount = 100;
            var posts = AddPosts(PostCount);
            Console.WriteLine("{0} Posts Created.", PostCount);

            this.Time(
                "Adding " + PostCount.ToString() + " Posts to Redis",
                () =>
                    {
                        foreach (var post in posts)
                        {
                            this._repository.AddPost(post);
                        }

                        return 1;
                    });

            var ids = posts.Select(p => p.Id);
            var uniqueIds = posts.Select(p => p.Id).Distinct();

            Assert.That(ids.Count(), Is.EqualTo(uniqueIds.Count()));
        }

        private static List<Post> AddPosts(int postCount)
        {
            var posts = new List<Post>();

            for (var i = 0; i < postCount; i++)
            {
                var j = i.ToString();
                posts.Add(
                    new Post() { Author = "Jason " + j, Title = "Wow + j", Content = "Yep + j", Tags = new[] { "Hot" + j } });
            }

            return posts;
        }

        [Test]
        public override void CanAddACommentToAnExistingPost()
        {
            const string Commenter = "Commenter";
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            post.Comments.Add(new Comment { Name = Commenter, Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);
            var result = _repository.GetById(post.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(1));
            Assert.That(result.Comments.Single().Name, Is.EqualTo(Commenter));
        }

        [Test]
        public override void CommentsOnAPostAreReturnedInChronologicalOrder()
        {
            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);
            const string FirstCommentName = "Commenter 1";
            post.Comments.Add(new Comment { Name = FirstCommentName, Email = "comment@comment.com", Content = "Halla!" });
            post.Comments.Add(new Comment { Name = "Commenter 2", Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);
            var result = _repository.GetById(post.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(2));
            Assert.That(result.Comments.First().Name, Is.EqualTo(FirstCommentName));
        }

        [Test]
        public override void CommentsAreStillInOrderWhenOneIsAddedLater()
        {
            const string FirstCommentName = "Commenter 1";
            const string ThirdCommentName = "Commenter 3";

            var post = new Post() { Author = "Jason", Title = "Wow", Content = "Yep", Tags = new[] { "Hot" } };
            _repository.AddPost(post);

            post.Comments.Add(new Comment { Name = FirstCommentName, Email = "comment@comment.com", Content = "Halla!" });
            post.Comments.Add(new Comment { Name = "Commenter 2", Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);

            post = _repository.GetById(post.Id);
            post.Comments.Add(new Comment { Name = ThirdCommentName, Email = "comment@comment.com", Content = "Halla!" });
            _repository.UpdatePost(post);

            var result = _repository.GetById(post.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(3));
            Assert.That(result.Comments.First().Name, Is.EqualTo(FirstCommentName));
            Assert.That(result.Comments.Last().Name, Is.EqualTo(ThirdCommentName));
        }
    }
}