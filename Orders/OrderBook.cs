namespace DroneFactory;

public class OrderBook
{
    private readonly Dictionary<string, int> _pending = new();

    public IReadOnlyDictionary<string, int> Pending => _pending;

    public bool IsEmpty => _pending.Count == 0;

    public void Add(string drone, int qty) =>
        _pending[drone] = _pending.GetValueOrDefault(drone) + qty;

    public void Fulfill(string drone, int qty)
    {
        var remaining = _pending.GetValueOrDefault(drone) - qty;
        if (remaining > 0)
            _pending[drone] = remaining;
        else
            _pending.Remove(drone);
    }
}
