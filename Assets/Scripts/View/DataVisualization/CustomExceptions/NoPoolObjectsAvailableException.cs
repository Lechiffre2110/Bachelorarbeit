using System;

public class NoPoolObjectsAvailableException : Exception
{
    public NoPoolObjectsAvailableException() 
        : base("No pool objects available in the pool")
    {
    }

    public NoPoolObjectsAvailableException(string message) 
        : base(message)
    {
    }

    public NoPoolObjectsAvailableException(string message, Exception inner) 
        : base(message, inner)
    {
    }
}