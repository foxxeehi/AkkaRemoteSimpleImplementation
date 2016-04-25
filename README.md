This is a simple example for using AKKA.remote.

This solution contains three projects.

1. ImageProcessingService
2. WebClient
3. SharedProject

How it works : 

Overview:
The WebClient will consume the ImageProcessingService by typing "run"/"stitch" in WebClient console application. 
Both console application will print out some messages after receiving.

Detail:
In WebClient project:
An actor system called "Deployer" was created. Under the actor system there is an actor called "WebClientActor". 
The actor system "Deployer" remotely deployed an actor called "ImageProcessingActorRemoteDeployment" on ImageProcessingService and also 
hold an actor reference using actor selection called "_imageProcessingServiceLocalDeploymentActor". The "Deployer" has 1 local actor and
2 actor reference from ImageProcessingService.

In ImageProcessingService project:
An actor system called "ImageProcessingService" was created. Under the actor system there is an actor called "ImageProcessingActorLocalDeployment".
Remember in WebClient project we remotely deployed an actor called "ImageProcessingActorRemoteDeployment" so in ImageProcessingService,
there are 2 actors.

In SharedProject project:
Define a ImageProcessingServiceActor.
Define 2 messages using on WebClientside and 2 messages using on ImageProcessingService side.
All actors and messages shared by both sides should be put here.
