using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Generators;
using Json.Schema.Generation.Intents;

namespace BrainVision.Lab.SystemExt.Text.Json.Validation.Internal.Generators.RefinersAndGenerators;

internal class VersionGenerator : ISchemaGenerator
{
    public bool Handles(Type type) => type.Equals(typeof(Version));

    public void AddConstraints(SchemaGenerationContextBase context)
    {
        context.Intents.Clear();
        context.Intents.Add(new TypeIntent(SchemaValueType.String));
        //context.Intents.Add(new PatternIntent("^([0-9]{1})-([0-9]{1})$"));//MJTODOlater 
    }
}
