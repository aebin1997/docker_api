namespace Infrastructure.Services.DI;

public interface IDiService
{
    Task GetData();
}

public class DiService : IDiService
{
    private readonly ISingletonService _singleton;
    private readonly IScopedService _scoped;
    private readonly ITransientService _transient;
    
    public DiService(ISingletonService singleton, IScopedService scoped, ITransientService transient)
    {
        _singleton = singleton;
        _scoped = scoped;
        _transient = transient;
    }

    public async Task GetData()
    {
        Console.WriteLine("Di Service");
        Console.WriteLine($"Singleton: {_singleton.GetGuid()}");
        Console.WriteLine($"Scoped: {_scoped.GetGuid()}");
        Console.WriteLine($"Transient: {_transient.GetGuid()}");
        Console.WriteLine("========================================================================");
    }
}