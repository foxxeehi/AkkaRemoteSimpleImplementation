using System.Collections.Generic;

namespace SharedProject
{
    //Server Side
    public class RunImageAnalysis
    {
        public string ConfigurationString { get; private set; }
        public int ImageId { get; private set; }
        public RunImageAnalysis(string configurationString, int imageId)
        {
            ConfigurationString = configurationString;
            ImageId = imageId;
        }
    }

    public class StitchImages
    {
        public IReadOnlyList<int> ListOfImagesId { get; private set; }

        public StitchImages(IReadOnlyList<int> listOfImagesId)
        {
            ListOfImagesId = listOfImagesId;
        }
    }

    //Client Side

    public class ImageAnalysisResult
    {
        public string MeasurementResult { get; private set; }

        public ImageAnalysisResult(string measurementResult)
        {
            MeasurementResult = measurementResult;
        }
    }

    public class StitchImagesResult
    {
        public int ImageId { get; private set; }

        public StitchImagesResult(int imageId)
        {
            ImageId = imageId;
        }
    }
}