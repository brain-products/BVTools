using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class EegElectrodesWriter
{
    public static async Task SaveAsync(string filePath, EegElectrodeCollection electrodes)
    {
        List<EegElectrodeTsv> list = electrodes.ConvertAll(p => new EegElectrodeTsv(p));
        await TsvTableWriter<EegElectrodeTsv>.SaveAsync(filePath, EegElectrodeTsv.TsvIdentifiers, list).ConfigureAwait(false);
    }
}
