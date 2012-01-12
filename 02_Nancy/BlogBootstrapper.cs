using System.IO;
using Domain;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Testing.Fakes;
using StructureMap;

namespace _02_Nancy
{
    public class BlogBootstrapper : StructureMapNancyBootstrapper
    {
        protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            RavenDbHelper.ConfigureRaven(container);
        }
    }

    internal class TestBlogBootstrapper : StructureMapNancyBootstrapper
    {
        private const string TestDbDirectory = @"C:\TestBlogData";

        protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Configure(x => x.SelectConstructor(() => new FakeNancyModule()));

            RavenDbHelper.ConfigureRaven(container, TestDbDirectory);
        }
    }
}