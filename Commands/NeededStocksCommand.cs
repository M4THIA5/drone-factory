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

            foreach (var piece in new[] { recipe.Hull, recipe.Core, recipe.Generator, recipe.Move, recipe.Processor, recipe.System })
            {
                Console.WriteLine($"{qty} {piece}");
                total[piece] = total.GetValueOrDefault(piece) + qty;
            }
        }

        Console.WriteLine("Total :");

        foreach (var piece in Catalogue.Pieces)
            if (total.TryGetValue(piece, out var q)) Console.WriteLine($"{q} {piece}");

        foreach (var system in Catalogue.Systems)
            if (total.TryGetValue(system, out var q)) Console.WriteLine($"{q} {system}");
    }
}
