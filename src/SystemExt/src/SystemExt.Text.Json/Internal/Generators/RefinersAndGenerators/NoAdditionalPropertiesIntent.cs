using Json.Schema;
using Json.Schema.Generation;

namespace BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Generators.RefinersAndGenerators;

/// <summary>
/// Selfmade solution to apple the Property "additionalProperties":false recursively to each class. 
/// Official solution will be provided lateron: https://github.com/gregsdennis/json-everything/issues/264
/// </summary>
internal class NoAdditionalPropertiesIntent : ISchemaKeywordIntent
{
    public void Apply(JsonSchemaBuilder builder) => builder.AdditionalProperties(false);
}
