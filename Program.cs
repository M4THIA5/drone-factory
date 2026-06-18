using DroneFactory;

var stock = new Stock();

var commands = new Dictionary<string, ICommand>
{
    ["STOCKS"]         = new StocksCommand(stock),
    ["NEEDED_STOCKS"]  = new NeededStocksCommand(),
    ["INSTRUCTIONS"]   = new InstructionsCommand(),
    ["VERIFY"]         = new VerifyCommand(stock),
    ["PRODUCE"]        = new ProduceCommand(stock),
    ["ADD_TEMPLATE"]   = new AddTemplateCommand(stock),
};

string? line;
while ((line = Console.ReadLine()) != null)
{
    var trimmed = line.Trim();
    if (trimmed.Length == 0) continue;

    var spaceIdx = trimmed.IndexOf(' ');
    var command     = spaceIdx < 0 ? trimmed : trimmed[..spaceIdx];
    var commandArgs = spaceIdx < 0 ? ""      : trimmed[(spaceIdx + 1)..].Trim();

    try
    {
        if (commands.TryGetValue(command, out var cmd))
            cmd.Execute(commandArgs);
        else
            Console.WriteLine($"ERROR Unknown command `{command}`");
    }
    catch (CommandException ex)
    {
        Console.WriteLine($"ERROR {ex.Message}");
    }
    catch (Exception ex) {
        Console.WriteLine($"ERROR {ex.Message}");
    }
}
