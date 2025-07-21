namespace Scheduler.Exceptions;

public class EntityAlreadyExistExceptions : BaseException
{
    private readonly string message;

    public EntityAlreadyExistExceptions(string message)
    {
        this.message = message;
    }
    
    public override string GetErrorData()
    {
        return message;
    }
}