using DroneFactory;

var stock = new Stock();

string? line;
while ((line = Console.ReadLine()) != null)
{
    var trimmed = line.Trim();
    if (trimmed.Length == 0) continue;

    string command;
    string commandArgs;
    var spaceIdx = trimmed.IndexOf(' ');
    if (spaceIdx < 0)
    {
        command = trimmed;
        commandArgs = "";
    }
    else
    {
        command = trimmed.Substring(0, spaceIdx);
        commandArgs = trimmed.Substring(spaceIdx + 1).Trim();
    }

    try
    {
        switch (command)
        {
            case "STOCKS":
                HandleStocks(stock);
                break;
            case "NEEDED_STOCKS":
                HandleNeededStocks(commandArgs);
                break;
            case "INSTRUCTIONS":
                HandleInstructions(commandArgs);
                break;
            case "VERIFY":
                HandleVerify(commandArgs, stock);
                break;
            case "PRODUCE":
                HandleProduce(commandArgs, stock);
                break;
            default:
                Console.WriteLine($"ERROR Unknown command `{command}`");
                break;
        }
    }
    catch (CommandException ex)
    {
        Console.WriteLine($"ERROR {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR {ex.Message}");
    }
}

static void HandleStocks(Stock stock)
{
    foreach (var drone in Catalogue.Drones.Keys)
        Console.WriteLine($"{stock.GetDrone(drone)} {drone}");

    foreach (var piece in Catalogue.Pieces)
        Console.WriteLine($"{stock.GetPiece(piece)} {piece}");

    foreach (var system in Catalogue.Systems)
        Console.WriteLine($"{stock.GetPiece(system)} {system}");
}

static void HandleNeededStocks(string args)
{
    var order = ParseArgs(args);

    var total = new Dictionary<string, int>();

    foreach (var (drone, qty) in order)
    {
        var recipe = Catalogue.Drones[drone];
        Console.WriteLine($"{qty} {drone} :");

        var pieces = new[]
        {
            recipe.Hull, recipe.Core, recipe.Generator,
            recipe.Move, recipe.Processor, recipe.System,
        };

        foreach (var piece in pieces)
        {
            Console.WriteLine($"{qty} {piece}");
            total[piece] = total.GetValueOrDefault(piece) + qty;
        }
    }

    Console.WriteLine("Total :");

    foreach (var piece in Catalogue.Pieces)
        if (total.TryGetValue(piece, out var q))
            Console.WriteLine($"{q} {piece}");

    foreach (var system in Catalogue.Systems)
        if (total.TryGetValue(system, out var q))
            Console.WriteLine($"{q} {system}");
}

static void HandleInstructions(string args)
{
    var order = ParseArgs(args);

    foreach (var (drone, qty) in order)
    {
        var recipe = Catalogue.Drones[drone];
        for (var i = 0; i < qty; i++)
        {
            Console.WriteLine($"PRODUCING {drone}");
            Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Hull}");
            Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Core}");
            Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Generator}");
            Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Move}");
            Console.WriteLine($"GET_OUT_STOCK 1 {recipe.Processor}");
            Console.WriteLine($"INSTALL {recipe.System} {recipe.Core}");
            Console.WriteLine($"ASSEMBLE TMP1 {recipe.Hull} {recipe.Generator}");
            Console.WriteLine($"ASSEMBLE TMP2 TMP1 {recipe.Move}");
            Console.WriteLine($"ASSEMBLE TMP2 {recipe.Core}{{{recipe.System}}}");
            Console.WriteLine($"ASSEMBLE [TMP2, {recipe.Core}{{{recipe.System}}}] {recipe.Processor}");
            Console.WriteLine($"FINISHED {drone}");
        }
    }
}

static void HandleVerify(string args, Stock stock)
{
    var order = ParseArgs(args);
    var needed = ComputeNeeded(order);

    foreach (var (piece, qty) in needed)
    {
        if (stock.GetPiece(piece) < qty)
        {
            Console.WriteLine("UNAVAILABLE");
            return;
        }
    }

    Console.WriteLine("AVAILABLE");
}

static void HandleProduce(string args, Stock stock)
{
    var order = ParseArgs(args);
    var needed = ComputeNeeded(order);

    foreach (var (piece, qty) in needed)
    {
        if (stock.GetPiece(piece) < qty)
            throw new CommandException("Insufficient stock");
    }

    foreach (var (piece, qty) in needed)
        stock.ConsumePiece(piece, qty);

    foreach (var (drone, qty) in order)
        stock.AddDrone(drone, qty);

    Console.WriteLine("STOCK_UPDATED");
}

static Dictionary<string, int> ParseArgs(string args)
{
    if (string.IsNullOrWhiteSpace(args))
        throw new CommandException("Empty argument list");

    var result = new Dictionary<string, int>();
    var parts = args.Split(',');

    foreach (var raw in parts)
    {
        var part = raw.Trim();
        if (part.Length == 0)
            throw new CommandException("Empty entry in argument list");

        var tokens = part.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length != 2)
            throw new CommandException($"Invalid entry `{part}`, expected `<qty> <DroneName>`");

        if (!int.TryParse(tokens[0], out var qty) || qty <= 0)
            throw new CommandException($"`{tokens[0]}` is not a valid quantity");

        var droneName = tokens[1];
        if (!Catalogue.Drones.ContainsKey(droneName))
            throw new CommandException($"`{droneName}` is not a recognized drone");

        result[droneName] = result.GetValueOrDefault(droneName) + qty;
    }

    return result;
}

static Dictionary<string, int> ComputeNeeded(Dictionary<string, int> order)
{
    var needed = new Dictionary<string, int>();

    foreach (var (drone, qty) in order)
    {
        var recipe = Catalogue.Drones[drone];
        var pieces = new[]
        {
            recipe.Hull, recipe.Core, recipe.Generator,
            recipe.Move, recipe.Processor, recipe.System,
        };

        foreach (var piece in pieces)
            needed[piece] = needed.GetValueOrDefault(piece) + qty;
    }

    return needed;
}
