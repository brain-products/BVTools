using System.Text.Json;
using System.Text.Json.Serialization;
using BrainVision.Lab.SystemExt.Text.Json.Serialization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO
{
    internal static class JsonSerializerWithStandardConverters
    {
        public static string Serialize<T>(T objectToSerialize, bool writeIndented = false, bool ignoreNullValues = true) where T : notnull
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = writeIndented,
                IgnoreNullValues = ignoreNullValues,
            };

            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonStringVersionConverter());
            options.Converters.Add(new JsonStringPrefixedUnitConverter());

            return JsonSerializer.Serialize(objectToSerialize, objectToSerialize.GetType(), options);
        }
    }
}
