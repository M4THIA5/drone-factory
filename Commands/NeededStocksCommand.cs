namespace DroneFactory;

public class NeededStocksCommand : DroneCommand
{
    public override void Execute(string args)
    {
        var order = ParseArgs(args);
        var total = new Dictionary<string, int>();

        foreach (var (drone, qty) in order)
        {
            var recipe = Catalogue.Drones[drone];
            Console.WriteLine($"{qty} {drone} :");

            foreach (var (piece, count) in recipe.PieceCounts())
            {
                var pieceQty = count * qty;
                Console.WriteLine($"{pieceQty} {piece}");
                total[piece] = total.GetValueOrDefault(piece) + pieceQty;
            }
        }

        Console.WriteLine("Total :");

        foreach (var piece in Catalogue.Pieces)
            if (total.TryGetValue(piece, out var q)) Console.WriteLine($"{q} {piece}");

        foreach (var system in Catalogue.Systems)
            if (total.TryGetValue(system, out var q)) Console.WriteLine($"{q} {system}");
    }
}
