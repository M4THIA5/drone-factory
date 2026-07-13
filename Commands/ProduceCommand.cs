namespace DroneFactory;

public class ProduceCommand : DroneCommand
{
    private readonly Stock _stock;
    public ProduceCommand(Stock stock) => _stock = stock;

    public override void Execute(string args)
    {
        var order = ParseArgs(args);
        var needed = ComputeNeeded(order);

        foreach (var (piece, qty) in needed)
            if (_stock.GetPiece(piece) < qty)
                throw new CommandException("Insufficient stock");

        foreach (var (piece, qty) in needed)
            _stock.ConsumePiece(piece, qty, "PRODUCE");

        foreach (var (drone, qty) in order)
            _stock.AddDrone(drone, qty, "PRODUCE");

        Console.WriteLine("STOCK_UPDATED");
    }
}
