namespace DroneFactory.Categories;

public class SubmersibleRule : ICategoryRule
{
    public string Name => "Submersible";

    public bool Matches(DroneRecipe r) =>
        Catalogue.TypesOf(r.Hull).Contains("S") &&
        Catalogue.TypesOf(r.Generator).Contains("S") &&
        Catalogue.TypesOf(r.Move).Contains("S") &&
        Catalogue.TypesOf(r.System).Contains("3D");
}
