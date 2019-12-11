using System.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO
{
    internal static class JsonWriter
    {
        public static void Save<T>(string path, T data) where T : notnull
        {
            string json = JsonSerializerWithStandardConverters.Serialize(data, true, true);
            using StreamWriter streamWriter = File.CreateText(path);
            streamWriter.Write(json);
        }
    }
}
