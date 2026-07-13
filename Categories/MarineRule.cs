namespace DroneFactory.Categories;

public class MarineRule : ICategoryRule
{
    public string Name => "Marin";

    public bool Matches(DroneRecipe r) =>
        Catalogue.TypesOf(r.Hull).Contains("S") &&
        Catalogue.TypesOf(r.System).Contains("2D") &&
        r.Moves.Any(m => Catalogue.TypesOf(m).Contains("M"));
}
