namespace Infrastructure.Services.DI;

public interface IDiTwoService
{
    Task GetData2();
}

public class DiTwoService : IDiTwoService
{
    private readonly ISingletonService _singleton;
    private readonly IScopedService _scoped;
    private readonly ITransientService _transient;
    
    public DiTwoService(ISingletonService singleton, IScopedService scoped, ITransientService transient)
    {
        _singleton = singleton;
        _scoped = scoped;
        _transient = transient;
    }

    public async Task GetData2()
    {
        Console.WriteLine("Di Two Service");
        Console.WriteLine($"Singleton: {_singleton.GetGuid()}");
        Console.WriteLine($"Scoped: {_scoped.GetGuid()}");
        Console.WriteLine($"Transient: {_transient.GetGuid()}");
        Console.WriteLine("========================================================================");
    }
}