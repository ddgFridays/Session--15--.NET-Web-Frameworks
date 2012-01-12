using Domain;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace _02_Nancy
{
    public class DemoTest
    {
        [Fact]
        public void DontWriteTestsLikeThis()
        {
            var browser = new Browser(new TestBlogBootstrapper());

            var result = browser.Post("/articles", with =>
                {
                    with.HttpRequest();
                    with.JsonBody(new Article { Title = "test", Content = "testing content" });
                });
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);

            var newArticle = result.Body.DeserializeJson<Article>();
            Assert.False(string.IsNullOrEmpty(newArticle.Id));

            result = browser.Get("/articles/test", with => with.HttpRequest());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var article = result.Body.DeserializeJson<Article>();

            Assert.Equal("testing content", article.Content);
        }
    }
}