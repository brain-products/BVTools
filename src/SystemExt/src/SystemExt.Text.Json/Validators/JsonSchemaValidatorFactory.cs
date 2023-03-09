using BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Validators;

namespace BrainVision.Lab.SystemExt.Text.Json.Validation;

public static class JsonSchemaValidatorFactory
{
    /// <summary>
    /// https://www.nuget.org/packages/JsonSchema.Net
    /// https://github.com/gregsdennis/json-everything
    /// </summary>
    public static IJsonSchemaValidator Create(string schema) => new JsonEverythingValidator(schema);
}
