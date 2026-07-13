namespace DroneFactory;

// Observer of Stock. Keeps every movement in chronological (insertion) order so
// GET_MOVEMENTS can replay the full history.
public class MovementLog : IStockObserver
{
    private readonly List<Movement> _movements = new();

    public IReadOnlyList<Movement> Movements => _movements;

    public bool IsEmpty => _movements.Count == 0;

    public void OnMovement(Movement movement) => _movements.Add(movement);
}
