using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO
{
    internal class JsonStringPrefixedUnitConverter : JsonConverter<PrefixedUnit>
    {
        public override PrefixedUnit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // should never happen, object is never deserialized

        // The code below is a correct implementation of Read() method.
        // It is commented because parsing a value is not needed right now but may be used in the future.

        //    string s = reader.GetString();

        //    try
        //    {
        //        return Version.Parse(s);
        //    }
        //    catch (Exception e) when (e is ArgumentException || e is ArgumentOutOfRangeException || e is FormatException || e is OverflowException)
        //    {
        //        // if JsonSerializer fails to parse an object to requested Type, a JsonException exception is thrown.
        //        // The exception below is cough ad re-thrown as JsonException in order to assure the same behavior as for JsonSerializer.
        //        // The message is copied from JSON string converter: "The JSON value could not be converted to System.String".
        //        throw new JsonException($"{Resources.JsonValueCouldNotBeConvertedTo} {typeToConvert}.", e);
        //    }
        }

        public override void Write(Utf8JsonWriter writer, PrefixedUnit value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}
