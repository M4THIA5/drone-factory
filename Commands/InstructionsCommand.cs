// Commands/InstructionsCommand.cs
namespace DroneFactory;

public class InstructionsCommand : DroneCommand
{
    public override void Execute(string args)
    {
        var order = ParseArgs(args);

        foreach (var (drone, qty) in order)
        {
            var recipe = Catalogue.Drones[drone];
            for (var i = 0; i < qty; i++)
            {
                Console.WriteLine($"PRODUCING {drone}");
                Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Hull}");
                Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Core}");
                Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Generator}");
                Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Move}");
                Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Processor}");
                Console.WriteLine($"INSTALL {recipe.System} {recipe.Core}");
                Console.WriteLine($"ASSEMBLE TMP1 {recipe.Hull} {recipe.Generator}");
                Console.WriteLine($"ASSEMBLE TMP2 TMP1 {recipe.Move}");
                Console.WriteLine($"ASSEMBLE TMP2 {recipe.Core}{{{recipe.System}}}");
                Console.WriteLine($"ASSEMBLE [TMP2, {recipe.Core}{{{recipe.System}}}] {recipe.Processor}");
                Console.WriteLine($"FINISHED {drone}");
            }
        }
    }
}
