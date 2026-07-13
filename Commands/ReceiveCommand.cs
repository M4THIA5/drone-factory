namespace DroneFactory;

// RECEIVE ARGS — books incoming stock. ARGS is a `<qty> <name>` list where each
// name is a known piece, system or drone. Unlike DroneCommand args (drones only),
// so this parses its own list rather than reusing ParseArgs.
public class ReceiveCommand : ICommand
{
    private readonly Stock _stock;
    public ReceiveCommand(Stock stock) => _stock = stock;

    public void Execute(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new CommandException("Empty argument list");

        var received = new List<(string Name, int Qty)>();

        foreach (var raw in args.Split(','))
        {
            var part = raw.Trim();
            if (part.Length == 0)
                throw new CommandException("Empty entry in argument list");

            var tokens = part.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new CommandException($"Invalid entry `{part}`, expected `<qty> <Name>`");

            if (!int.TryParse(tokens[0], out var qty) || qty <= 0)
                throw new CommandException($"`{tokens[0]}` is not a valid quantity");

            var name = tokens[1];
            if (!Catalogue.IsKnownPiece(name) && !Catalogue.Drones.ContainsKey(name))
                throw new CommandException($"`{name}` is not a recognized piece or drone");

            received.Add((name, qty));
        }

        foreach (var (name, qty) in received)
        {
            if (Catalogue.Drones.ContainsKey(name))
                _stock.AddDrone(name, qty, "RECEIVE");
            else
                _stock.AddPiece(name, qty, "RECEIVE");
        }

        Console.WriteLine("STOCK_UPDATED");
    }
}
