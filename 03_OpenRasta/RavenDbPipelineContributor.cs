using OpenRasta.DI;
using OpenRasta.Pipeline;
using OpenRasta.Web;
using Raven.Client;
using Raven.Client.Document;

namespace _03_OpenRasta
{
    public class RavenDbPipelineContributor : IPipelineContributor
    {
        private readonly IDependencyResolver _resolver;

        public RavenDbPipelineContributor(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(OpenSession)
                .Before<KnownStages.IOperationExecution>();
        }

        private PipelineContinuation OpenSession(ICommunicationContext arg)
        {
            var documentStore = _resolver.Resolve<DocumentStore>();
            _resolver.AddDependencyInstance<IDocumentSession>(documentStore.OpenSession(), DependencyLifetime.PerRequest);
            return PipelineContinuation.Continue;
        }
    }
}