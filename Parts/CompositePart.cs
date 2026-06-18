namespace DroneFactory.Parts;

public class CompositePart : IPart
{
    public string? Name { get; }
    public IPart Left { get; }
    public IPart Right { get; }

    public CompositePart(string? name, IPart left, IPart right)
    {
        Name = name;
        Left = left;
        Right = right;
    }

    public string Label => Name ?? $"[{string.Join(", ", CollectLeafLabels(this))}]";

    private static IEnumerable<string> CollectLeafLabels(IPart part)
    {
        if (part is CompositePart { Name: null } composite)
        {
            foreach (var label in CollectLeafLabels(composite.Left))
                yield return label;
            foreach (var label in CollectLeafLabels(composite.Right))
                yield return label;
        }
        else
        {
            yield return part.Label;
        }
    }
}
