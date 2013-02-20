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

        public IEnumerable<Post> GetRecentPosts(int postCount)
        {
            throw new NotImplementedException();
            //_client.AddItemToSortedSet(_client.Set);
        }

        public void AddPost(Post post)
        {
            post.Id = this.GetNextPostId();
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

        private int GetNextPostId()
        {
            const string PostIdKey = "PostIdentifierKey";
            var id = (int)_bareClient.Increment(PostIdKey, 1);
            return (id == 0) ? (int)_bareClient.Increment(PostIdKey, 1) : id;
        }
    }
}