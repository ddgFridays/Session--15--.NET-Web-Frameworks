using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Domain;
using OpenRasta.Codecs;
using OpenRasta.Configuration;
using OpenRasta.TypeSystem;
using OpenRasta.Web;
using Newtonsoft.Json;

namespace _03_OpenRasta
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Uses.PipelineContributor<RavenDbPipelineContributor>();

                ResourceSpace.Has
                    .ResourcesOfType<IEnumerable<Article>>()
                    .AtUri("/articles")
                    .HandledBy<ArticleHandler>()
                    .AsJsonDataContract();
            }
        }
    }





    [MediaType("application/json;q=0.5", "json")]
    public class JsonCodec : IMediaTypeReader, IMediaTypeWriter
    {
        public object Configuration { get; set; }

        public object ReadFrom(IHttpEntity request, IType destinationType, string destinationName)
        {
            if(destinationType != null)
            {
                using(var reader = new StreamReader(request.Stream))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject(json, destinationType.StaticType);
                }
            }
            return Missing.Value;
        }

        public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
        {
            if (entity == null)
                return;
            var json = JsonConvert.SerializeObject(entity);
            using(var writer = new StreamWriter(response.Stream))
            {
                writer.Write(json);
            }
        }
    }
}