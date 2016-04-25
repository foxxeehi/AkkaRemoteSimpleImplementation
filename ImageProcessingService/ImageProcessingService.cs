using System;
using Akka.Actor;
using Akka.Configuration;
using SharedProject;

namespace ImageProcessingService
{
    class ImageProcessingService
    {
        static void Main()
        {
            using (var system = ActorSystem.Create("ImageProcessingService", ConfigurationFactory.ParseString(@"
                akka {  
                    actor.provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                    remote {
                        helios.tcp {
		                    port = 8090
		                    hostname = localhost
                        }
                    }
                }")))
            {
                system.ActorOf(Props.Create(() => new ImageProcessingServiceActor()), "ImageProcessingActorLocalDeployment");
                Console.ReadKey();
            }
        }
    }
}
