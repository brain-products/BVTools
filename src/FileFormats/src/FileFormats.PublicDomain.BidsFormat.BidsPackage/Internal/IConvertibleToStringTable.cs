using System.Collections.Generic;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal interface IConvertibleToStringTable
    {
        List<string?> ToList();
    }
}
