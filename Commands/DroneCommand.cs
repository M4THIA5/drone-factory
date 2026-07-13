namespace DroneFactory;

public abstract class DroneCommand : ICommand
{
    public abstract void Execute(string args);

    protected static Dictionary<string, int> ParseArgs(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new CommandException("Empty argument list");

        var result = new Dictionary<string, int>();

        foreach (var raw in args.Split(','))
        {
            var part = raw.Trim();
            if (part.Length == 0)
                throw new CommandException("Empty entry in argument list");

            var tokens = part.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new CommandException($"Invalid entry `{part}`, expected `<qty> <DroneName>`");

            if (!int.TryParse(tokens[0], out var qty) || qty <= 0)
                throw new CommandException($"`{tokens[0]}` is not a valid quantity");

            var droneName = tokens[1];
            if (!Catalogue.Drones.ContainsKey(droneName))
                throw new CommandException($"`{droneName}` is not a recognized drone");

            result[droneName] = result.GetValueOrDefault(droneName) + qty;
        }

        return result;
    }

    protected static Dictionary<string, int> ComputeNeeded(Dictionary<string, int> order)
    {
        var needed = new Dictionary<string, int>();

        foreach (var (drone, qty) in order)
        {
            var recipe = Catalogue.Drones[drone];
            foreach (var piece in recipe.AllPieces())
                needed[piece] = needed.GetValueOrDefault(piece) + qty;
        }

        return needed;
    }
}
