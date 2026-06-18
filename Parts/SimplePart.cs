namespace DroneFactory.Parts;

public class SimplePart : IPart
{
    private readonly string _piece;
    private readonly string? _system;

    public SimplePart(string piece, string? system = null)
    {
        _piece = piece;
        _system = system;
    }

    public string Label => _system is null ? _piece : $"{_piece}{{{_system}}}";
}
