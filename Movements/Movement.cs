namespace DroneFactory;

public enum MovementDirection { In, Out }

// One recorded stock movement. No timestamp on purpose: output stays
// deterministic for golden-style verification. No "kind" field: the name
// already distinguishes a piece from a drone.
public record Movement(string Source, MovementDirection Direction, int Qty, string Name);
