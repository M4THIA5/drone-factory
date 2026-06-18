namespace DroneFactory;

public record DroneRecipe(
    string Hull,
    string Core,
    string Generator,
    string Move,
    string Processor,
    string System);

public enum PieceKind
{
    Hull,
    Core,
    Generator,
    Move,
    Processor,
    System,
}

public static class Catalogue
{
    public static readonly IReadOnlyList<string> Pieces = new[]
    {
        "Hull_HG1", "Hull_HF1", "Hull_HS1",
        "Core_CG1", "Core_C3D1",
        "Generator_GG1", "Generator_GF1", "Generator_GS1",
        "Move_MF1", "Move_ML1", "Move_MS1", "Move_MM1", "Move_MU1", "Move_MS2",
        "Processor_PG1", "Processor_P3D1", "Processor_PU1",
    };

    public static readonly IReadOnlyList<string> Systems = new[]
    {
        "System_SG1", "System_S3D1",
    };

    public static readonly IReadOnlyDictionary<string, IReadOnlySet<string>> PieceTypes =
        new Dictionary<string, IReadOnlySet<string>>
        {
            // Coques
            ["Hull_HG1"] = Set("S"),
            ["Hull_HF1"] = Set(),
            ["Hull_HS1"] = Set("S"),
            // Modules principaux
            ["Core_CG1"] = Set("2D"),
            ["Core_C3D1"] = Set("2D", "3D"),
            // Générateurs
            ["Generator_GG1"] = Set(),
            ["Generator_GF1"] = Set(),
            ["Generator_GS1"] = Set("S"),
            // Modules de déplacement
            ["Move_MF1"] = Set("F"),
            ["Move_ML1"] = Set("L"),
            ["Move_MS1"] = Set("S"),
            ["Move_MM1"] = Set("M"),
            ["Move_MU1"] = Set("M", "L"),
            ["Move_MS2"] = Set("M", "S"),
            // Modules de contrôle
            ["Processor_PG1"] = Set("2D"),
            ["Processor_P3D1"] = Set("3D"),
            ["Processor_PU1"] = Set("2D", "3D"),
            // Systèmes principaux
            ["System_SG1"] = Set("2D"),
            ["System_S3D1"] = Set("2D", "3D"),
        };

    public static readonly Dictionary<string, DroneRecipe> Drones =
        new()
        {
            ["DXF-1"] = new("Hull_HF1", "Core_C3D1", "Generator_GF1", "Move_MF1", "Processor_P3D1", "System_S3D1"),
            ["RDL-1"] = new("Hull_HG1", "Core_CG1",  "Generator_GG1", "Move_ML1", "Processor_PG1",  "System_SG1"),
            ["WDS-1"] = new("Hull_HS1", "Core_C3D1", "Generator_GS1", "Move_MS1", "Processor_P3D1", "System_S3D1"),
            ["DYM-1"] = new("Hull_HG1", "Core_CG1",  "Generator_GG1", "Move_MM1", "Processor_PG1",  "System_SG1"),
        };

    public static IReadOnlySet<string> TypesOf(string piece) =>
        PieceTypes.TryGetValue(piece, out var types) ? types : EmptySet;

    public static bool IsKnownPiece(string name) =>
        PieceTypes.ContainsKey(name);

    public static PieceKind KindOf(string piece) => piece switch
    {
        _ when piece.StartsWith("Hull_") => PieceKind.Hull,
        _ when piece.StartsWith("Core_") => PieceKind.Core,
        _ when piece.StartsWith("Generator_") => PieceKind.Generator,
        _ when piece.StartsWith("Move_") => PieceKind.Move,
        _ when piece.StartsWith("Processor_") => PieceKind.Processor,
        _ when piece.StartsWith("System_") => PieceKind.System,
        _ => throw new CommandException($"`{piece}` has no recognized piece kind"),
    };

    private static readonly IReadOnlySet<string> EmptySet = new HashSet<string>();

    private static IReadOnlySet<string> Set(params string[] tags) => new HashSet<string>(tags);
}
