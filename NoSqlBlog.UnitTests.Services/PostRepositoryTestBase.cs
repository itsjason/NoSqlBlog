namespace NoSqlBlog.UnitTests.Services
{
    using System;
    using System.Diagnostics;
    using NUnit.Framework;

    public abstract class PostRepositoryTestBase
    {
        protected T Time<T>(string label, Func<T> func)
        {
            var timer = new Stopwatch();
            timer.Start();
            var result = func.Invoke();
            timer.Stop();
            Console.WriteLine("Action: {0}, Time Taken: {1}", label, Math.Round((double)timer.ElapsedMilliseconds, 2));
            return result;
        }

        [Test]
        public abstract void GetRecentPostsReturnsSpecifiedNumberOfPosts();

        [Test]
        public abstract void RecentPostsReturnInCorrectOrder();

        [Test]
        public abstract void PostCanBeAdded();

        [Test]
        public abstract void PostCanBeDeleted();

        [Test]
        public abstract void PostHasIdAfterSaving();

        [Test]
        public abstract void CanAddACommentToAnExistingPost();

        [Test]
        public abstract void CommentsOnAPostAreReturnedInChronologicalOrder();

        [Test]
        public abstract void CommentsAreStillInOrderWhenOneIsAddedLater();
    }
}