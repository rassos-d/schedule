namespace Scheduler.Exceptions;

public class BadRequestException : BaseException
{
    private readonly string message;

    public BadRequestException(string message)
    {
        this.message = message;
    }
    
    public override string GetErrorData()
    {
        return message;
    }
}