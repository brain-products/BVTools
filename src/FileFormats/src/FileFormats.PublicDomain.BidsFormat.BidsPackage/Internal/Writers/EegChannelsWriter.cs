using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class EegChannelsWriter
{
    public static async Task SaveAsync(string filePath, EegChannelCollection channels)
    {
        List<EegChannelTsv> list = channels.ConvertAll(p => new EegChannelTsv(p));
        await TsvTableWriter<EegChannelTsv>.SaveAsync(filePath, EegChannelTsv.TsvIdentifiers, list).ConfigureAwait(false);
    }
}
