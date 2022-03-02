using System;
using Proxoft.Redux.Core.ExceptionHandling;

namespace MusicStore.Redux;

public class ApplicationStoreExceptionHandler : IExceptionHandler
{
    public void OnException(Exception exception)
    {
        Console.WriteLine(exception.ToString());
    }
}
