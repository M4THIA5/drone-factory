namespace DroneFactory;

public class Stock
{
    public Dictionary<string, int> Pieces { get; } = new();
    public Dictionary<string, int> Drones { get; } = new();

    public Stock()
    {
        foreach (var piece in Catalogue.Pieces)
            Pieces[piece] = 10;

        foreach (var system in Catalogue.Systems)
            Pieces[system] = 10;

        foreach (var drone in Catalogue.Drones.Keys)
            Drones[drone] = 0;
    }

    public int GetPiece(string name) => Pieces.TryGetValue(name, out var q) ? q : 0;

    public int GetDrone(string name) => Drones.TryGetValue(name, out var q) ? q : 0;

    public void ConsumePiece(string name, int qty) => Pieces[name] -= qty;

    public void AddPiece(string name, int qty) => Pieces[name] = GetPiece(name) + qty;

    public void AddDrone(string name, int qty) => Drones[name] = GetDrone(name) + qty;

    public void ConsumeDrone(string name, int qty) => Drones[name] = GetDrone(name) - qty;
}
