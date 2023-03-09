namespace BrainVision.Lab.SystemExt.Text.Json.Validation;

public interface IJsonSchemaGenerator
{
    public string Generate(Type t);

    public string Generate<T>();
}
