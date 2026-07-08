namespace DroneFactory;

public class SendCommand : ICommand
{
    private readonly Stock _stock;
    private readonly OrderBook _orders;

    public SendCommand(Stock stock, OrderBook orders)
    {
        _stock = stock;
        _orders = orders;
    }

    public void Execute(string args)
    {
        var shipped = false;

        // Snapshot the pending orders so we can fulfill (mutate) them while iterating.
        foreach (var (drone, pending) in _orders.Pending.ToList())
        {
            var qty = Math.Min(pending, _stock.GetDrone(drone));
            if (qty <= 0) continue;

            _stock.ConsumeDrone(drone, qty);
            _orders.Fulfill(drone, qty);
            Console.WriteLine($"SENT {qty} {drone}");
            shipped = true;
        }

        if (!shipped)
            Console.WriteLine("NOTHING_TO_SEND");
    }
}
