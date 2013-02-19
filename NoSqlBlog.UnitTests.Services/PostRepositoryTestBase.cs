namespace NoSqlBlog.UnitTests.Services
{
    using System;
    using System.Diagnostics;
    using NUnit.Framework;
    using Raven.Client;
    using Raven.Client.Embedded;

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
        public abstract void RepositoryReturnsSpecifiedNumberOfPosts();

        [Test]
        public abstract void RecentPostsReturnInCorrectOrder();
    }
}