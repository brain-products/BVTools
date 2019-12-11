using System.Collections.Generic;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal static class TaskEventsWriter
    {
        public static void Save(string filePath, TaskEventCollection events)
        {
            List<TaskEventTsv> list = events.ConvertAll(p => new TaskEventTsv(p));
            TsvTableWriter<TaskEventTsv>.Save(filePath, TaskEventTsv.TsvIdentifiers, list);
        }
    }
}

