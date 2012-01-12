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
    }
}