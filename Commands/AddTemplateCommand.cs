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

        var byKind = new Dictionary<PieceKind, string>();
        foreach (var piece in parts.Skip(1))
        {
            if (!Catalogue.IsKnownPiece(piece))
                throw new CommandException($"`{piece}` is not a recognized piece");

            var kind = Catalogue.KindOf(piece);
            if (byKind.ContainsKey(kind))
                throw new CommandException($"Duplicate {kind} piece (`{byKind[kind]}` and `{piece}`)");

            byKind[kind] = piece;
        }

        foreach (var kind in Enum.GetValues<PieceKind>())
            if (!byKind.ContainsKey(kind))
                throw new CommandException($"Missing {kind} piece");

        var recipe = new DroneRecipe(
            byKind[PieceKind.Hull], byKind[PieceKind.Core],    byKind[PieceKind.Generator],
            byKind[PieceKind.Move], byKind[PieceKind.Processor], byKind[PieceKind.System]);

        Categorizer.Validate(recipe);
        Catalogue.Drones[name] = recipe;
        _stock.AddDrone(name, 0);

        Console.WriteLine("TEMPLATE_ADDED");
    }
}
