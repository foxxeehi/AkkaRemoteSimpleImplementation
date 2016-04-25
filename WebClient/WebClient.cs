using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;
using SharedProject;

namespace WebClient
{
    class WebClient
    {
        static private IActorRef _imageProcessingServiceRemoteDeploymentActor;
        static private ActorSelection _imageProcessingServiceLocalDeploymentActor;
        static private IActorRef _webClientActor;

        // The web client actor
        class WebClientActor : ReceiveActor
        {
            public WebClientActor()
            {
                // Request message
                Receive<RunImageAnalysis>(result =>
                {
                    _imageProcessingServiceRemoteDeploymentActor.Tell(new RunImageAnalysis("configurationString", 1));
                    _imageProcessingServiceLocalDeploymentActor.Tell(new RunImageAnalysis("configurationString", 1));
                });

                Receive<StitchImages>(result =>
                {
                    _imageProcessingServiceRemoteDeploymentActor.Tell(new StitchImages(new List<int> { 1, 2 }));
                    _imageProcessingServiceLocalDeploymentActor.Tell(new StitchImages(new List<int> { 1, 2 }));
                });

                // Result message
                Receive<ImageAnalysisResult>(result =>
                {
                    Console.WriteLine("Received measurement : {1} from {0}", Sender, result.MeasurementResult);
                });

                Receive<StitchImagesResult>(result =>
                {
                    Console.WriteLine("Received stitch image ID : {1} from {0}", Sender, result.ImageId);
                });

                Receive<Terminated>(terminated =>
                {
                    Console.WriteLine(terminated.ActorRef);
                    Console.WriteLine("Was address terminated? {0}", terminated.AddressTerminated);
                });
            }
        }

        static void Main()
        {
            using (var system = ActorSystem.Create("Deployer",
                // The configuration for remote actor deployment and remote actor system communication.
                // This part of code can also be put in App.config using HOCON.
                ConfigurationFactory.ParseString(@"
                akka {  
                    actor{
                        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                        deployment {
                            /ImageProcessingActorRemoteDeployment {
                                remote = ""akka.tcp://ImageProcessingService@localhost:8090""
                            }
                        }
                    }
                    remote {
                        helios.tcp {
		                    port = 0
		                    hostname = localhost
                        }
                    }
                }")))
            {
                // Deploy an actor in the "Image processing service" actor system remotely and assign a name to it. 
                // The deployment section above tells the actor system to deploy this actor on the imge processing service side.
                _imageProcessingServiceRemoteDeploymentActor =
                    system.ActorOf(Props.Create(() => new ImageProcessingServiceActor()),
                        "ImageProcessingActorRemoteDeployment");
                // Get the "Image processing service" actor reference using address.
                _imageProcessingServiceLocalDeploymentActor =
                    system.ActorSelection(
                        "akka.tcp://ImageProcessingService@localhost:8090/user/ImageProcessingActorLocalDeployment");
                // Create an actor in "WebClient" actor system
                _webClientActor = system.ActorOf(Props.Create(() => new WebClientActor()), "WebClientActor");
                while (true)
                {
                    var input = Console.ReadLine();
                    if (input == "run")
                    {
                        _webClientActor.Tell(new RunImageAnalysis("configurationString", 1));
                    }
                    if (input == "stitch")
                    {
                        _webClientActor.Tell(new StitchImages(new List<int> { 1, 2 }));
                    }
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
