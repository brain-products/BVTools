using System.Runtime.CompilerServices;
using System.Text.Json;
using BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Generators.RefinersAndGenerators;
using Json.Schema;
using Json.Schema.Generation;

[assembly: InternalsVisibleTo("BrainVision.Lab.SystemExt.Text.Json.Tests")]

namespace BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Generators;

/// <summary>
/// https://github.com/gregsdennis/json-everything/blob/master/JsonSchema.Generation/JsonSchemaBuilderExtensions.cs
/// https://endjin.com/blog/2021/05/csharp-serialization-with-system-text-json-schema
/// https://coder.social/gregsdennis/json-everything
/// </summary>
internal class JsonEverythingGenerator : IJsonSchemaGenerator
{
    public string Generate(Type t)
    {
        SchemaGeneratorConfiguration config = new()
        {
            //MJTODOlater could be set to Nullability.AllowForNullableValueTypes
            Nullability = Nullability.Disabled,
            PropertyOrder = PropertyOrder.AsDeclared
        };
        config.Generators.Add(new VersionGenerator());
        config.Refiners.Add(new NoAdditionalPropertiesRefiner());
        JsonSchemaBuilder builder = new JsonSchemaBuilder().FromType(t, config);

        JsonSchema jsonSchema = builder.Build();
        string schema = JsonSerializer.Serialize(jsonSchema);
        return schema;
    }

    public string Generate<T>() => Generate(typeof(T));
}
