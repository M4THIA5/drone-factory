namespace DroneFactory.Builders;

using DroneFactory.Parts;

public class DroneAssemblyBuilder
{
    public IReadOnlyList<string> Build(string droneName, DroneRecipe recipe)
    {
        var steps = new List<string>();

        steps.Add($"PRODUCING {droneName}");

        var hull = new SimplePart(recipe.Hull);
        var core = new SimplePart(recipe.Core);
        var generators = recipe.Generators.Select(g => new SimplePart(g)).ToList();
        var moves = recipe.Moves.Select(m => new SimplePart(m)).ToList();
        var processor = new SimplePart(recipe.Processor);

        // The system is installed (not pulled from stock as a standalone piece).
        var stockOut = new List<IPart> { hull, core };
        stockOut.AddRange(generators);
        stockOut.AddRange(moves);
        stockOut.Add(processor);
        foreach (var part in stockOut)
            steps.Add($"GET_OUT_STOCK 1 {part.Label}");

        var coreWithSystem = new SimplePart(recipe.Core, recipe.System);
        steps.Add($"INSTALL {recipe.System} {recipe.Core}");

        // Generators are mounted in the hull before the main module, then the
        // movement modules; each of those intermediate assemblies is named.
        IPart acc = hull;
        var tmp = 1;
        foreach (var generator in generators)
        {
            var node = new CompositePart($"TMP{tmp++}", acc, generator);
            steps.Add(Assemble(node, acc, generator));
            acc = node;
        }

        foreach (var move in moves)
        {
            var node = new CompositePart($"TMP{tmp++}", acc, move);
            steps.Add(Assemble(node, acc, move));
            acc = node;
        }

        // Main module (with its system) then control module: left unnamed, the
        // final result being the known drone itself.
        var withCore = new CompositePart(null, acc, coreWithSystem);
        steps.Add(Assemble(withCore, acc, coreWithSystem));

        var final = new CompositePart(null, withCore, processor);
        steps.Add(Assemble(final, withCore, processor));

        steps.Add($"FINISHED {droneName}");

        return steps;
    }

    private static string Assemble(CompositePart result, IPart left, IPart right) =>
        result.Name is not null
            ? $"ASSEMBLE {result.Name} {left.Label} {right.Label}"
            : $"ASSEMBLE {left.Label} {right.Label}";
}
