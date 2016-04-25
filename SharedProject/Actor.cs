using System;
using System.Linq;
using Akka.Actor;

namespace SharedProject
{
    // The image processing service actor
    public class ImageProcessingServiceActor : ReceiveActor
    {
        public ImageProcessingServiceActor()
        {
            Receive<RunImageAnalysis>(result =>
            {
                Console.WriteLine("Run image analysis, Image id : {0} with configuration : {1} from {2}", result.ImageId,
                    result.ConfigurationString, Sender);
                Sender.Tell(new ImageAnalysisResult("measurementResult"));
            });

            Receive<StitchImages>(result =>
            {
                Console.WriteLine("Stitch images, from image id : {0} to image id : {1} from {2}",
                    result.ListOfImagesId.FirstOrDefault(), result.ListOfImagesId.LastOrDefault(), Sender);
                Sender.Tell(new StitchImagesResult(result.ListOfImagesId.LastOrDefault() + 1));
            });
        }
    }
}
