using System.Text.Json;
using System.Text.Json.Serialization;
using BrainVision.Lab.SystemExt.Text.Json.Properties;

namespace BrainVision.Lab.SystemExt.Text.Json.Serialization;

public class JsonStringVersionConverter : JsonConverter<Version>
{
    /// <exception cref="JsonException">Thrown if Converter fails to parse an object to Version type.</exception>
    public override Version? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string s = reader.GetString()!; // can't be null because HandleNull => false

        try
        {
            return Version.Parse(s);
        }
        catch (Exception e) when (e is ArgumentException || e is ArgumentOutOfRangeException || e is FormatException || e is OverflowException)
        {
            // Typically, if JsonSerializer fails to parse an object to requested Type, it throws a JsonException exception.
            // The exceptions are cought and re-thrown as JsonException in order to assure the same behavior of JsonStringVersionConverter as all standard system JsonSerializers do.
            // The message is copied from JSON string converter: "The JSON value could not be converted to System.String".
            // Unfortunately, it is not possible to deliver full information that normally JsonException contain:
            // Path, LineNumber and BytePositionInLine are still missing. These values are stored in Utf8JsonReader, but unfortunately not exposed in public properties.
            // For more info, See the internal dotnet core method: ThrowHelper.AddJsonExceptionInformation(in ReadStack state, in Utf8JsonReader reader, JsonException ex)
            // that adds these info in following format:
            // message += $" Path: {path} | LineNumber: {lineNumber} | BytePositionInLine: {bytePositionInLine}.";
            throw new JsonException($"{Resources.JsonValueCouldNotBeConvertedTo} {typeToConvert}.", e);
        }
    }

    public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());

    public override bool HandleNull => false; // false is anyway the default for reference types, so this method is added only for clarity
}
