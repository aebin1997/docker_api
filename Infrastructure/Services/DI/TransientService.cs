namespace Infrastructure.Services.DI;

public interface ITransientService
{
    string GetGuid();
}

public class TransientService : ITransientService
{
    private readonly string _guid;
    
    public TransientService()
    {
        _guid = Guid.NewGuid().ToString();
    }

    public string GetGuid()
    {
        return _guid;
    }
}