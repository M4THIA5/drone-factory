namespace DroneFactory;

using DroneFactory.Categories;

public static class Categorizer
{
    private static readonly IReadOnlyList<ICategoryRule> Rules = new ICategoryRule[]
    {
        new AerialRule(),
        new MarineRule(),
        new TerrestrialRule(),
        new SubmersibleRule(),
    };

    public static IReadOnlyList<string> Categories(DroneRecipe r) =>
        Rules.Where(rule => rule.Matches(r))
             .Select(rule => rule.Name)
             .ToList();

    public static void Validate(DroneRecipe r)
    {
        if (r.Generators.Count is < 1 or > 2)
            throw new CommandException($"A drone must have 1 or 2 generators (got {r.Generators.Count})");

        if (r.Moves.Count is < 1 or > 3)
            throw new CommandException($"A drone must have between 1 and 3 movement modules (got {r.Moves.Count})");

        if (r.Moves.Count >= 2 && r.Generators.Count != 2)
            throw new CommandException("A drone with at least 2 movement modules must have exactly 2 generators");

        var core = Catalogue.TypesOf(r.Core);
        var processor = Catalogue.TypesOf(r.Processor);
        var system = Catalogue.TypesOf(r.System);

        if (!system.IsSubsetOf(core))
            throw new CommandException($"`{r.System}` cannot be installed on `{r.Core}`");

        if (!processor.Overlaps(system))
            throw new CommandException($"`{r.Processor}` is not compatible with `{r.System}`");

        var categories = Categories(r);
        if (categories.Count == 0)
        {
            var knownCategories = string.Join(", ", Rules.Select(rule => rule.Name));
            throw new CommandException($"Drone matches no category ({knownCategories})");
        }
    }
}
