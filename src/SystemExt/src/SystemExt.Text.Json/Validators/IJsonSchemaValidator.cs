namespace BrainVision.Lab.SystemExt.Text.Json.Validation;

public interface IJsonSchemaValidator
{
    bool Validate(string json, out IList<string> errorMessages);
}
