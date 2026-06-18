namespace DroneFactory;

public class StocksCommand : DroneCommand
{
    private readonly Stock _stock;
    public StocksCommand(Stock stock) => _stock = stock;

    public override void Execute(string args)
    {
        foreach (var drone in Catalogue.Drones.Keys)
            Console.WriteLine($"{_stock.GetDrone(drone)} {drone}");

        foreach (var piece in Catalogue.Pieces)
            Console.WriteLine($"{_stock.GetPiece(piece)} {piece}");

        foreach (var system in Catalogue.Systems)
            Console.WriteLine($"{_stock.GetPiece(system)} {system}");
    }
}
