namespace DroneFactory.Categories;

public class AerialRule : ICategoryRule
{
    public string Name => "Aérien";

    public bool Matches(DroneRecipe r) =>
        r.Moves.Any(m => Catalogue.TypesOf(m).Contains("F")) &&
        Catalogue.TypesOf(r.System).Contains("3D");
}
