namespace DroneFactory;

// GET_MOVEMENTS — prints the stock movement history, one line per movement:
// `<SOURCE> <IN|OUT> <qty> <name>`, in chronological order. Empty -> NO_MOVEMENT.
public class GetMovementsCommand : ICommand
{
    private readonly MovementLog _log;
    public GetMovementsCommand(MovementLog log) => _log = log;

    public void Execute(string args)
    {
        if (_log.IsEmpty)
        {
            Console.WriteLine("NO_MOVEMENT");
            return;
        }

        foreach (var m in _log.Movements)
        {
            var direction = m.Direction == MovementDirection.In ? "IN" : "OUT";
            Console.WriteLine($"{m.Source} {direction} {m.Qty} {m.Name}");
        }
    }
}
