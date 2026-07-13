namespace DroneFactory;

public class AddTemplateCommand : DroneCommand
{
    private readonly Stock _stock;
    public AddTemplateCommand(Stock stock) => _stock = stock;

    public override void Execute(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new CommandException("Empty argument list");

        var parts = args.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            throw new CommandException("Expected `ADD_TEMPLATE TEMPLATE_NAME, Piece1, ..., PieceN`");

        var name = parts[0];
        if (Catalogue.Drones.ContainsKey(name))
            throw new CommandException($"`{name}` is already a known drone");
        if (Catalogue.IsKnownPiece(name))
            throw new CommandException($"`{name}` is already a known piece name");

        var byKind = new Dictionary<PieceKind, List<string>>();
        foreach (var piece in parts.Skip(1))
        {
            if (!Catalogue.IsKnownPiece(piece))
                throw new CommandException($"`{piece}` is not a recognized piece");

            var kind = Catalogue.KindOf(piece);
            if (!byKind.TryGetValue(kind, out var list))
                byKind[kind] = list = new List<string>();
            list.Add(piece);
        }

        // Generators (1-2) and movement modules (1-3) may repeat; every other
        // kind must appear exactly once. Upper bounds are checked in Validate.
        var recipe = new DroneRecipe(
            Single(byKind, PieceKind.Hull), Single(byKind, PieceKind.Core),
            Multiple(byKind, PieceKind.Generator), Multiple(byKind, PieceKind.Move),
            Single(byKind, PieceKind.Processor), Single(byKind, PieceKind.System));

        Categorizer.Validate(recipe);
        Catalogue.Drones[name] = recipe;
        _stock.AddDrone(name, 0, "ADD_TEMPLATE");

        Console.WriteLine("TEMPLATE_ADDED");
    }

    private static string Single(Dictionary<PieceKind, List<string>> byKind, PieceKind kind)
    {
        if (!byKind.TryGetValue(kind, out var list))
            throw new CommandException($"Missing {kind} piece");
        if (list.Count > 1)
            throw new CommandException($"A drone must have exactly one {kind} piece");
        return list[0];
    }

    private static IReadOnlyList<string> Multiple(Dictionary<PieceKind, List<string>> byKind, PieceKind kind)
    {
        if (!byKind.TryGetValue(kind, out var list))
            throw new CommandException($"Missing {kind} piece");
        return list;
    }
}
