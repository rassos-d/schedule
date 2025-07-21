namespace Scheduler.Exceptions;

public class EntityNotFoundException : BaseException
{
    private readonly string message;

    public EntityNotFoundException(string message)
    {
        this.message = message;
    }
    
    public override string GetErrorData()
    {
        return message;
    }
}