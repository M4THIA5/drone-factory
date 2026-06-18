namespace DroneFactory;

using DroneFactory.Builders;

public class InstructionsCommand : DroneCommand
{
    private readonly DroneAssemblyBuilder _builder = new();

    public override void Execute(string args)
    {
        var order = ParseArgs(args);

        foreach (var (drone, qty) in order)
        {
            var recipe = Catalogue.Drones[drone];
            for (var i = 0; i < qty; i++)
                foreach (var line in _builder.Build(drone, recipe))
                    Console.WriteLine(line);
        }
    }
}
