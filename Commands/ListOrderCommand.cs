namespace DroneFactory;

public class ListOrderCommand : ICommand
{
    private readonly OrderBook _orders;
    public ListOrderCommand(OrderBook orders) => _orders = orders;

    public void Execute(string args)
    {
        if (_orders.IsEmpty)
        {
            Console.WriteLine("NO_ORDER");
            return;
        }

        foreach (var (drone, qty) in _orders.Pending)
            Console.WriteLine($"{qty} {drone}");
    }
}
