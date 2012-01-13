using System.Collections.Generic;
using System.Linq;
using Domain;
using Raven.Client;

namespace _03_OpenRasta
{
    public class ArticleHandler
    {
        private readonly IDocumentSession _session;

        public ArticleHandler(IDocumentSession session)
        {
            _session = session;
        }

        public IEnumerable<Article> Get()
        {
            return _session.Query<Article>().ToList();
        }

        public Article Get(string title)
        {
            return LoadArticle(title);
        }

        public Article Post(Article article)
        {
            _session.Store(article);
            _session.SaveChanges();
            return article;
        }

        public Article Put(string title, Article article)
        {
            var existingArticle = LoadArticle(title);
            if (existingArticle != null)
            {
                existingArticle.Title = article.Title;
                existingArticle.Content = article.Content;
                _session.Store(existingArticle);
                _session.SaveChanges();
            }
            return existingArticle;
        }

        public void Delete(string title)
        {
            var article = LoadArticle(title);
            if (article != null)
                _session.Delete(article);
        }

        private Article LoadArticle(string title)
        {
            return _session
                .Query<Article>()
                .SingleOrDefault(a => a.Title.Equals(title.ToLower()));
        }
    }
}