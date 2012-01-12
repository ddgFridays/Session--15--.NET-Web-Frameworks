using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.Hosting.HttpListener;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace _03_OpenRasta
{
    public class OpenRastaHost : HttpListenerHost
    {
        public IConfigurationSource Configuration { get; set; }
        public override bool ConfigureRootDependencies(IDependencyResolver resolver)
        {
            var result = base.ConfigureRootDependencies(resolver);
            if (result && Configuration != null)
            {
                resolver.AddDependencyInstance<IConfigurationSource>(Configuration);
                ConfigureRavenDb(resolver);
            }
            return result;
        }

        private void ConfigureRavenDb(IDependencyResolver resolver)
        {
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = @"C:\BlogData",
                UseEmbeddedHttpServer = true
            };
            documentStore.Configuration.Port = 10000;
            documentStore.Initialize();

            resolver.AddDependencyInstance<DocumentStore>(documentStore);
        }
    }
}