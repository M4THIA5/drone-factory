namespace DroneFactory.Categories;

public class TerrestrialRule : ICategoryRule
{
    public string Name => "Terrestre";

    public bool Matches(DroneRecipe r) =>
        r.Moves.Any(m => Catalogue.TypesOf(m).Contains("L")) &&
        Catalogue.TypesOf(r.System).Contains("2D");
}
