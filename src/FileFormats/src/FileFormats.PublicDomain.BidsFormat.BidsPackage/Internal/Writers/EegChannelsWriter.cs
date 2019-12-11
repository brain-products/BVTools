using System.Collections.Generic;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal static class EegChannelsWriter
    {
        public static void Save(string filePath, EegChannelCollection channels)
        {
            List<EegChannelTsv> list = channels.ConvertAll(p => new EegChannelTsv(p));
            TsvTableWriter<EegChannelTsv>.Save(filePath, EegChannelTsv.TsvIdentifiers, list);
        }
    }
}
