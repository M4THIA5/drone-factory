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
        var generator = new SimplePart(recipe.Generator);
        var move = new SimplePart(recipe.Move);
        var processor = new SimplePart(recipe.Processor);

        foreach (var part in new IPart[] { hull, core, generator, move, processor })
            steps.Add($"GET_OUT_STOCK 1 {part.Label}");

        var coreWithSystem = new SimplePart(recipe.Core, recipe.System);
        steps.Add($"INSTALL {recipe.System} {recipe.Core}");


        var tmp1 = new CompositePart("TMP1", hull, generator);
        steps.Add(Assemble(tmp1, hull, generator));

        var tmp2 = new CompositePart("TMP2", tmp1, move);
        steps.Add(Assemble(tmp2, tmp1, move));

        var partial = new CompositePart(null, tmp2, coreWithSystem);
        steps.Add(Assemble(partial, tmp2, coreWithSystem));

        var final = new CompositePart(null, partial, processor);
        steps.Add(Assemble(final, partial, processor));

        steps.Add($"FINISHED {droneName}");

        return steps;
    }

    private static string Assemble(CompositePart result, IPart left, IPart right) =>
        result.Name is not null
            ? $"ASSEMBLE {result.Name} {left.Label} {right.Label}"
            : $"ASSEMBLE {left.Label} {right.Label}";
}
