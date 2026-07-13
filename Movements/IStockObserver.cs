namespace DroneFactory;

// Observer contract: notified by Stock (the Subject) on every stock movement.
public interface IStockObserver
{
    void OnMovement(Movement movement);
}
