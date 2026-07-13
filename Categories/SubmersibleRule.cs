namespace DroneFactory.Categories;

public class SubmersibleRule : ICategoryRule
{
    public string Name => "Submersible";

    public bool Matches(DroneRecipe r) =>
        Catalogue.TypesOf(r.Hull).Contains("S") &&
        r.Generators.All(g => Catalogue.TypesOf(g).Contains("S")) &&
        r.Moves.All(m => Catalogue.TypesOf(m).Contains("S")) &&
        Catalogue.TypesOf(r.System).Contains("3D");
}
