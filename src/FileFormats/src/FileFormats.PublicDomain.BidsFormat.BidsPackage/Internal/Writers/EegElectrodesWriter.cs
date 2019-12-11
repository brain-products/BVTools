using System.Collections.Generic;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal static class EegElectrodesWriter
    {
        public static void Save(string filePath, EegElectrodeCollection electrodes)
        {
            List<EegElectrodeTsv> list = electrodes.ConvertAll(p => new EegElectrodeTsv(p));
            TsvTableWriter<EegElectrodeTsv>.Save(filePath, EegElectrodeTsv.TsvIdentifiers, list);
        }
    }
}
