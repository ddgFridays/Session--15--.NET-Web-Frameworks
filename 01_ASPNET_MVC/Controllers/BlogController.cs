using System.Linq;
using System.Web.Mvc;
using Domain;
using Raven.Client;

namespace _01_ASPNET_MVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly IDocumentSession _session;

        public BlogController(IDocumentSession session)
        {
            _session = session;
        }

        [HttpGet]
        public ActionResult List()
        {
            var articles = _session.Query<Article>();
            return Json(articles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(string articletitle)
        {
            var article = LoadArticleByTitle(articletitle);
            return Json(article, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Article article)
        {
            _session.Store(article);
            _session.SaveChanges();
            return Json(article);
        }

        [HttpPut]
        public ActionResult Update(string articletitle, Article article)
        {
            var existingArticle = LoadArticleByTitle(articletitle);
            if (existingArticle != null)
            {
                existingArticle.Title = article.Title;
                existingArticle.Content = article.Content;
                _session.Store(existingArticle);
                _session.SaveChanges();
            }
            return Json(article);
        }

        [HttpDelete]
        public void Delete(string articletitle)
        {
            var article = LoadArticleByTitle(articletitle);
            if(article != null)
                _session.Delete(article);
        }

        private Article LoadArticleByTitle(string title)
        {
            return _session
                .Query<Article>()
                .SingleOrDefault(a => a.Title.Equals(title));
        }
    }
}