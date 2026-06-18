namespace DroneFactory.Categories;

public class AerialRule : ICategoryRule
{
    public string Name => "Aérien";

    public bool Matches(DroneRecipe r) =>
        Catalogue.TypesOf(r.Move).Contains("F") &&
        Catalogue.TypesOf(r.System).Contains("3D");
}
