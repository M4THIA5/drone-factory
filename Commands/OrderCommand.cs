namespace DroneFactory;

public class OrderCommand : DroneCommand
{
    private readonly OrderBook _orders;
    public OrderCommand(OrderBook orders) => _orders = orders;

    public override void Execute(string args)
    {
        var order = ParseArgs(args);

        foreach (var (drone, qty) in order)
            _orders.Add(drone, qty);

        Console.WriteLine("ORDER_REGISTERED");
    }
}
