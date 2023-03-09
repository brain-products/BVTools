using System.Text.Json;
using Json.Schema;

namespace BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Validators;

internal class JsonEverythingValidator : IJsonSchemaValidator
{
    private readonly JsonSchema _jsonSchema;

    private readonly ValidationOptions _options = new();

    public JsonEverythingValidator(string schema)
    {
        _jsonSchema = JsonSchema.FromText(schema);
        _options.ValidateAs = Draft.Draft202012;

        // indicates that nodes will be organized in a condensed structure that mimicks the schema
        _options.OutputFormat = OutputFormat.Detailed;

        // set to maximum strictness

        // Summary:
        //     Specifies whether the `format` keyword should fail validations for unknown formats.
        //     Default is false.
        //
        // Remarks:
        //     This option is applied whether `format` is using annotation or assertion behavior.
        _options.OnlyKnownFormats = true;

        // Summary:
        //     Specifies whether the `format` keyword should be required to provide validation
        //     results. Default is false, which just produces annotations for drafts 2019-09
        //     and prior or follows the behavior set forth by the format-annotation vocabulary
        //     requirement in the `$vocabulary` keyword in a meta-schema declaring draft 2020-12.
        _options.RequireFormatValidation = true;

        // Summary:
        //     Indicates whether the schema should be validated against its `$schema` value.
        //     this is not typically necessary.
        _options.ValidateMetaSchema = true;
    }

    public bool Validate(string json, out IList<string> errorMessages)
    {
        // code change due to: https://github.com/gregsdennis/json-everything/issues/295
        ValidationResults validationResults = _jsonSchema.Validate(JsonDocument.Parse(json).RootElement, _options);

        (int _, errorMessages) = GetFailCountAndMessages(validationResults);
        return validationResults.IsValid;
    }

    private static (int FailCount, IList<string> ErrorMessages) GetFailCountAndMessages(ValidationResults validationResults)
    {
        int failCount = 0;
        List<string> messages = new();
        if (validationResults.Message != null)
            messages.Add(validationResults.Message);

        if (validationResults.HasNestedResults)
        {
            failCount = validationResults.NestedResults.Count(p => !p.IsValid);
            foreach (ValidationResults nestedResult in validationResults.NestedResults)
            {
                (int f, IList<string> m) = GetFailCountAndMessages(nestedResult);
                if (!nestedResult.IsValid && nestedResult.Message != null)
                    failCount += f;
                messages.AddRange(m);
            }
        }
        return (failCount, messages);
    }
}
