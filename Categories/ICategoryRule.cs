namespace DroneFactory.Categories;

public interface ICategoryRule
{
    string Name { get; }

    bool Matches(DroneRecipe recipe);
}
