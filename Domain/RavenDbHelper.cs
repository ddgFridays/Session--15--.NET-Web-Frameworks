using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using StructureMap;

namespace Domain
{
    public static class RavenDbHelper
    {
        public static void ConfigureRaven(IContainer container, string dataDirectory = @"C:\BlogData")
        {
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = dataDirectory,
                UseEmbeddedHttpServer = true
            };
            documentStore.Configuration.Port = 10000;
            documentStore.Initialize();

            container.Configure(x =>
            {
                x.For<DocumentStore>().Singleton().Use(documentStore);
                x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(() => container.GetInstance<DocumentStore>().OpenSession());
            });
        }

        public static void CleanUp(IContainer container)
        {
            var session = ObjectFactory.Container.GetInstance<IDocumentSession>();
            if (session != null && session.Advanced.HasChanges)
                session.SaveChanges();
        }
    }
}