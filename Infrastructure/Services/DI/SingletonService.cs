namespace Infrastructure.Services.DI;

public interface ISingletonService
{
    string GetGuid();
}

public class SingletonService : ISingletonService
{
    private readonly string _guid;
    
    public SingletonService()
    {
        _guid = Guid.NewGuid().ToString();
    }

    public string GetGuid()
    {
        return _guid;
    }
}