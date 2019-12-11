using System;
using System.Globalization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal static class EnumParser
    {
        public static string? ToString<T>(T? enumValue) where T : struct, Enum, IConvertible // Enum is a struct with IConvertible interface
            => (enumValue?.ToInt32(CultureInfo.InvariantCulture) == default) ? null : enumValue.ToString();
    }
}
