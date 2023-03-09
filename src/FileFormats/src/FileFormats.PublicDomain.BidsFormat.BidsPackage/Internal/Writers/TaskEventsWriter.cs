using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class TaskEventsWriter
{
    public static async Task SaveAsync(string filePath, TaskEventCollection events)
    {
        List<TaskEventTsv> list = events.ConvertAll(p => new TaskEventTsv(p));
        await TsvTableWriter<TaskEventTsv>.SaveAsync(filePath, TaskEventTsv.TsvIdentifiers, list).ConfigureAwait(false);
    }
}

