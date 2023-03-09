using System.Text.Json;
using System.Text.Json.Serialization;
using BrainVision.Lab.SystemExt.Text.Json.Serialization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

internal static class JsonSerializerWithStandardConverters
{
    public static async Task SerializeAsync<T>(Stream stream, T objectToSerialize, bool writeIndented = false, bool ignoreNullValues = true) where T : notnull
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = writeIndented,
            DefaultIgnoreCondition = ignoreNullValues ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never,
        };

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new JsonStringVersionConverter());
        options.Converters.Add(new JsonStringPrefixedUnitConverter());

        await JsonSerializer.SerializeAsync(stream, objectToSerialize, objectToSerialize.GetType(), options).ConfigureAwait(false);
    }
}
