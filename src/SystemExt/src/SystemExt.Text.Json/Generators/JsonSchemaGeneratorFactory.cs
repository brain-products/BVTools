using BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Generators;

namespace BrainVision.Lab.SystemExt.Text.Json.Validation;

public static class JsonSchemaGeneratorFactory
{
    /// <summary>
    /// https://www.nuget.org/packages/JsonSchema.Net
    /// https://github.com/gregsdennis/json-everything
    /// </summary>
    public static IJsonSchemaGenerator Create() => new JsonEverythingGenerator();
}
