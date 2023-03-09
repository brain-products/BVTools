using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal.Converters;

internal static class TaskEventsConverter
{
    public static TaskEventCollection? Collect(BrainVisionPackage filesContent)
    {
        IHeaderFileContentVer1 headerContent = filesContent.HeaderFileContent;
        IMarkerFileContentVer1? markerContent = filesContent.MarkerFileContent;

        IList<MarkerInfo>? markers = markerContent?.GetMarkers();
        if (markers == null)
            return null;

        double samplingFrequency = Common.GetSamplingFrequencyFromSamplingInterval(headerContent.SamplingInterval!.Value);

        TaskEventCollection taskEvents = new();

        foreach (MarkerInfo marker in markers)
        {
            TaskEvent taskEvent = new(
                //REQUIRED
                onset: marker.Position / samplingFrequency,
                duration: marker.Length / samplingFrequency)
            {
                //OPTIONAL
                Sample = marker.Position + 1, // I suppose bids uses 1-based sample position. This info is not present in the official bids documentations
                TrialType = marker.Type,
                Value = !string.IsNullOrEmpty(marker.Description) ? marker.Description : Defs.NotAvailable,
            };

            taskEvents.Add(taskEvent);
        }

        return taskEvents;
    }
}
