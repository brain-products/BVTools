using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using BrainVision.Lab.SystemExt.Text.Json.Serialization.Properties;

namespace BrainVision.Lab.SystemExt.Text.Json.Serialization
{
    public class JsonStringVersionConverter : JsonConverter<Version>
    {
        /// <exception cref="JsonException">Thrown if Converter fails to parse an object to Version type.</exception>
        public override Version Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string s = reader.GetString();

            try
            {
                return Version.Parse(s);
            }
            catch (Exception e) when (e is ArgumentException || e is ArgumentOutOfRangeException || e is FormatException || e is OverflowException)
            {
                // Typically, if JsonSerializer fails to parse an object to requested Type, it throws a JsonException exception.
                // The exceptions are cough and re-thrown as JsonException in order to assure the same behavior of JsonStringVersionConverter as all standard system JsonSerializers do.
                // The message is copied from JSON string converter: "The JSON value could not be converted to System.String".
                throw new JsonException($"{Resources.JsonValueCouldNotBeConvertedTo} {typeToConvert}.", e);
            }
        }

        public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}
