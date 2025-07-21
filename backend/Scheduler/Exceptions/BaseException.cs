namespace Scheduler.Exceptions;

public abstract class BaseException : Exception
{
     public abstract string GetErrorData();
}