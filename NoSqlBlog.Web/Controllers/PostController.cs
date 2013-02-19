using System.Web.Mvc;

namespace NoSqlBlog.Web.Controllers
{
    using Core.Interfaces;
    using System.Linq;
    using Core.Models;

    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Index()
        {
            return View(_postRepository.GetRecentPosts(10).ToArray());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Post post)
        {
            _postRepository.AddPost(post);
            AddMessage("Post Added!");
            return RedirectToAction("View", new {id = post.Id});
        }

        private void AddMessage(string message)
        {
            ViewBag.Message = message;
        }

        public ActionResult View(int id)
        {
            var post = _postRepository.GetById(id);
            return View(post);
        }
    }
}
