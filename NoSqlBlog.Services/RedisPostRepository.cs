namespace NoSqlBlog.Services
{
    using System;
    using System.Collections.Generic;
    using Core.Interfaces;
    using Core.Models;
    using ServiceStack.Redis;
    using ServiceStack.Redis.Generic;

    public class RedisPostRepository : IPostRepository
    {
        private readonly IRedisTypedClient<Post> _client;

        public RedisPostRepository()
        {
            var redisClient = new RedisClient();
            _client = redisClient.As<Post>();
        }

        public RedisPostRepository(string host, int port, string password)
        {
            var redisClient = new RedisClient(host, port, password);
            _client = redisClient.As<Post>();
        }

        public RedisPostRepository(Uri serverUri)
        {
            var redisClient = new RedisClient(serverUri);
            _client = redisClient.As<Post>();
        }

        public IEnumerable<Post> GetRecentPosts(int postCount)
        {
            throw new NotImplementedException();
            //_client.AddItemToSortedSet(_client.Set);
        }

        public void AddPost(Post post)
        {
            _client.Store(post);
            _client.Save();
        }

        public void DeletePost(Post post)
        {
            _client.Delete(post);
            _client.Save();
        }

        public void UpdatePost(Post post)
        {
            AddPost(post);
        }

        public Post GetById(int id)
        {
            return _client.GetById(id);
        }
    }
}