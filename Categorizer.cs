namespace DroneFactory;

/// <summary>
/// Calcul des catégories d'un drone (Aérien/Marin/Terrestre/Submersible) et
/// validation des contraintes de compatibilité de son recette.
/// </summary>
public static class Categorizer
{
    /// <summary>Catégories auxquelles appartient le drone décrit par <paramref name="r"/>.</summary>
    public static IReadOnlyList<string> Categories(DroneRecipe r)
    {
        var hull = Catalogue.TypesOf(r.Hull);
        var generator = Catalogue.TypesOf(r.Generator);
        var move = Catalogue.TypesOf(r.Move);
        var system = Catalogue.TypesOf(r.System);

        var categories = new List<string>();

        // Aérien (F) : module de déplacement (F) + système (3D)
        if (move.Contains("F") && system.Contains("3D"))
            categories.Add("Aérien");

        // Marin (M) : coque étanche (S) + système (2D) + module de déplacement (M)
        if (hull.Contains("S") && system.Contains("2D") && move.Contains("M"))
            categories.Add("Marin");

        // Terrestre (L) : module de déplacement (L) + système (2D)
        if (move.Contains("L") && system.Contains("2D"))
            categories.Add("Terrestre");

        // Submersible (S) : coque, générateur et déplacement (S) + système (3D)
        if (hull.Contains("S") && generator.Contains("S") && move.Contains("S") && system.Contains("3D"))
            categories.Add("Submersible");

        return categories;
    }

    /// <summary>
    /// Vérifie les compatibilités (Core↔Système, Processeur↔Système) et l'appartenance
    /// à au moins une catégorie. Lève une <see cref="CommandException"/> si invalide.
    /// </summary>
    public static void Validate(DroneRecipe r)
    {
        var core = Catalogue.TypesOf(r.Core);
        var processor = Catalogue.TypesOf(r.Processor);
        var system = Catalogue.TypesOf(r.System);

        // Le système installé doit être supporté par le module principal.
        if (!system.IsSubsetOf(core))
            throw new CommandException($"`{r.System}` cannot be installed on `{r.Core}`");

        // Le module de contrôle doit être compatible avec le système installé.
        if (!processor.Overlaps(system))
            throw new CommandException($"`{r.Processor}` is not compatible with `{r.System}`");

        // Un drone doit appartenir à au moins une catégorie.
        if (Categories(r).Count == 0)
            throw new CommandException("drone matches no category (Aérien, Marin, Terrestre or Submersible)");
    }
}
