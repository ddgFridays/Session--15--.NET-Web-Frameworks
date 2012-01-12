using System.IO;
using log4net.Config;
using Topshelf;

namespace _03_OpenRasta
{
    class Program
    {
        static void Main()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(".\\log4net.config"));

            var host = HostFactory.New(config =>
            {
                config.Service<OpenRastaHost>(s =>
                {
                    s.ConstructUsing(_ => new OpenRastaHost { Configuration = new Configuration() });
                    s.WhenStarted(openrasta =>
                        {
                            openrasta.Initialize(new[] { "http://localhost:10003/" }, "/", null);
                            openrasta.StartListening();
                        });
                    s.WhenStopped(openrasta => openrasta.StopListening());
                });
                config.RunAsLocalSystem();
                config.SetDescription("A service to serve my blog data.");
                config.SetDisplayName("Blog Data Service");
                config.SetServiceName("BlogDataService");
            });
            host.Run();
        }
    }
}