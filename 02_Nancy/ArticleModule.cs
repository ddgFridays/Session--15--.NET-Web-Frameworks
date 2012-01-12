using System;
using System.Linq;
using Domain;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;


namespace _02_Nancy
{
    public class ArticleModule : NancyModule
    {
        private readonly IDocumentSession _session;

        public ArticleModule(IDocumentSession session) : base("articles")
        {
            _session = session;

            Get["/"] = _ => Response.AsJson(_session.Query<Article>());
            Get["/{title}"] = _ => Response.AsJson(LoadArticle((string)_.title));

            Post["/"] = _ => CreateArticle(this.Bind<Article>("Id"));
            Put["/{title}"] = _ => UpdateArticle(_.title, this.Bind<Article>());
            Delete["/{title}"] = _ => DeleteArticle(_.title);

            Before += ctx =>
                {
                    Console.WriteLine("{0} request for URL {1}", ctx.Request.Method, ctx.Request.Url.Path);
                    return null;
                };
            After += ctx => Console.WriteLine("SUCCESS!");
        }

        private Response CreateArticle(Article article)
        {
            _session.Store(article);
            _session.SaveChanges();
            return Response.AsJson(article);
        }

        private Response UpdateArticle(string title, Article updatedArticle)
        {
            var existingArticle = LoadArticle(title);
            if (existingArticle == null)
                return 404;
            existingArticle.Title = updatedArticle.Title;
            existingArticle.Content = updatedArticle.Content;
            _session.Store(existingArticle);
            _session.SaveChanges();
            return Response.AsJson(updatedArticle);
        }

        private Response DeleteArticle(string title)
        {
            var article = LoadArticle(title);
            if (article == null)
                return 404;
            _session.Delete(article);
            _session.SaveChanges();
            return 200;
        }

        private Article LoadArticle(string title)
        {
            return _session
                .Query<Article>()
                .FirstOrDefault(a => a.Title.Equals(title.ToLower()));
        }
    }
}