using System;
using System.IO;
using log4net.Config;
using Nancy.Hosting.Self;
using Topshelf;

namespace _02_Nancy
{
    class Program
    {
        static void Main()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(".\\log4net.config"));

            var host = HostFactory.New(config =>
            {
                config.Service<NancyHost>(s =>
                {
                    s.ConstructUsing(_ => new NancyHost(new Uri("http://localhost:10002"), new BlogBootstrapper()));
                    s.WhenStarted(nancy => nancy.Start());
                    s.WhenStopped(nancy => nancy.Stop());
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