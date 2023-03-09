namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

internal static class JsonWriter
{
    public static async Task SaveAsync<T>(string path, T data) where T : notnull
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        StreamWriter streamWriter = File.CreateText(path);
#pragma warning restore CA2000 // Dispose objects before losing scope
        await using (streamWriter.ConfigureAwait(false))
        {
            await JsonSerializerWithStandardConverters.SerializeAsync(streamWriter.BaseStream, data, true, true).ConfigureAwait(false);
        }
    }
}
