namespace NoSqlBlog.Services
{
    using System;
    using System.Collections.Generic;
    using Core.Interfaces;
    using Core.Models;
    using ServiceStack.Redis;
    using ServiceStack.Redis.Generic;

    using System.Linq;

    public class RedisPostRepository : IPostRepository
    {
        private readonly IRedisClient _bareClient;
        private readonly IRedisTypedClient<Post> _client;

        public RedisPostRepository()
        {
            _bareClient = new RedisClient();
            _client = _bareClient.As<Post>();
        }

        public RedisPostRepository(string host, int port, string password = null)
        {
            _bareClient = new RedisClient(host, port, password);
            _client = _bareClient.As<Post>();
        }

        public RedisPostRepository(Uri serverUri)
        {
            _bareClient = new RedisClient(serverUri);
            _client = _bareClient.As<Post>();
        }

        private IRedisSortedSet<Post> RecentPosts { get { return _client.SortedSets["urn:Post:RecentPostsaaa"]; } }

        public IEnumerable<Post> GetRecentPosts(int postCount)
        {
            return _client.GetAll().OrderByDescending(p => p.Id).Take(postCount);
            //return _client.GetRangeFromSortedSetDesc(RecentPosts, upperBound, upperBound - postCount);
        }

        public void AddPost(Post post)
        {
            post.Id = this.GetNextPostId();
            _client.Store(post);
            RecentPosts.Add(post);
        }

        public void DeletePost(Post post)
        {
            var success = RecentPosts.Remove(post);
            _client.DeleteById(post.Id);
            if (!success)
                throw new ArgumentException("Post not found.");
        }

        public void UpdatePost(Post post)
        {
            var retrievedPost = _client.GetById(post.Id);

            retrievedPost.Comments = post.Comments;
            retrievedPost.Content = post.Content;
            retrievedPost.CreatedAt = post.CreatedAt;
            retrievedPost.Slug = post.Slug;
            retrievedPost.Tags = post.Tags;
            retrievedPost.Title = post.Title;

            _client.Store(retrievedPost);
        }

        public Post GetById(int id)
        {
            return _client.GetById(id);
        }

        private int GetNextPostId()
        {
            return (int)_client.GetNextSequence();
        }
    }
}