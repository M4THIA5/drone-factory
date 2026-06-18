namespace DroneFactory;

public class VerifyCommand : DroneCommand
{
    private readonly Stock _stock;
    public VerifyCommand(Stock stock) => _stock = stock;

    public override void Execute(string args)
    {
        var order = ParseArgs(args);
        var needed = ComputeNeeded(order);

        foreach (var (piece, qty) in needed)
        {
            if (_stock.GetPiece(piece) < qty)
            {
                Console.WriteLine("UNAVAILABLE");
                return;
            }
        }

        Console.WriteLine("AVAILABLE");
    }
}
