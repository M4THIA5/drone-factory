namespace DroneFactory;

public record DroneRecipe(
    string Hull,
    string Core,
    string Generator,
    string Move,
    string Processor,
    string System);

public static class Catalogue
{
    public static readonly IReadOnlyList<string> Pieces = new[]
    {
        "Hull_HG1", "Hull_HF1", "Hull_HS1",
        "Core_CG1", "Core_C3D1",
        "Generator_GG1", "Generator_GF1", "Generator_GS1",
        "Move_MF1", "Move_ML1", "Move_MS1", "Move_MM1",
        "Processor_PG1", "Processor_P3D1",
    };

    public static readonly IReadOnlyList<string> Systems = new[]
    {
        "System_SG1", "System_S3D1",
    };

    public static readonly IReadOnlyDictionary<string, string> SystemForCore =
        new Dictionary<string, string>
        {
            ["Core_CG1"] = "System_SG1",
            ["Core_C3D1"] = "System_S3D1",
        };

    public static readonly IReadOnlyDictionary<string, DroneRecipe> Drones =
        new Dictionary<string, DroneRecipe>
        {
            ["DXF-1"] = new("Hull_HF1", "Core_C3D1", "Generator_GF1", "Move_MF1", "Processor_P3D1", "System_S3D1"),
            ["RDL-1"] = new("Hull_HG1", "Core_CG1",  "Generator_GG1", "Move_ML1", "Processor_PG1",  "System_SG1"),
            ["WDS-1"] = new("Hull_HS1", "Core_C3D1", "Generator_GS1", "Move_MS1", "Processor_P3D1", "System_S3D1"),
            ["DYM-1"] = new("Hull_HG1", "Core_CG1",  "Generator_GG1", "Move_MM1", "Processor_PG1",  "System_SG1"),
        };
}
