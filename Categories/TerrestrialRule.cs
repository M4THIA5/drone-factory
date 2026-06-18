namespace DroneFactory.Categories;

public class TerrestrialRule : ICategoryRule
{
    public string Name => "Terrestre";

    public bool Matches(DroneRecipe r) =>
        Catalogue.TypesOf(r.Move).Contains("L") &&
        Catalogue.TypesOf(r.System).Contains("2D");
}
