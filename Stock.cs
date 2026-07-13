namespace DroneFactory;

public class Stock
{
    public Dictionary<string, int> Pieces { get; } = new();
    public Dictionary<string, int> Drones { get; } = new();

    // Observer pattern: Stock is the Subject. Observers are notified on every
    // non-zero mutation. Initial seeding below writes the dictionaries directly,
    // so the starting stock is not recorded as movements.
    private readonly List<IStockObserver> _observers = new();

    public Stock()
    {
        foreach (var piece in Catalogue.Pieces)
            Pieces[piece] = 10;

        foreach (var system in Catalogue.Systems)
            Pieces[system] = 10;

        foreach (var drone in Catalogue.Drones.Keys)
            Drones[drone] = 0;
    }

    public void Subscribe(IStockObserver observer) => _observers.Add(observer);

    private void Notify(string source, MovementDirection direction, int qty, string name)
    {
        if (qty == 0) return; // a no-op is not a movement (e.g. ADD_TEMPLATE seeds a drone at 0)

        var movement = new Movement(source, direction, qty, name);
        foreach (var observer in _observers)
            observer.OnMovement(movement);
    }

    public int GetPiece(string name) => Pieces.TryGetValue(name, out var q) ? q : 0;

    public int GetDrone(string name) => Drones.TryGetValue(name, out var q) ? q : 0;

    public void ConsumePiece(string name, int qty, string source)
    {
        Pieces[name] -= qty;
        Notify(source, MovementDirection.Out, qty, name);
    }

    public void AddPiece(string name, int qty, string source)
    {
        Pieces[name] = GetPiece(name) + qty;
        Notify(source, MovementDirection.In, qty, name);
    }

    public void AddDrone(string name, int qty, string source)
    {
        Drones[name] = GetDrone(name) + qty;
        Notify(source, MovementDirection.In, qty, name);
    }

    public void ConsumeDrone(string name, int qty, string source)
    {
        Drones[name] = GetDrone(name) - qty;
        Notify(source, MovementDirection.Out, qty, name);
    }
}
