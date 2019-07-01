using System;

namespace AwesomeCorp
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogError(Exception exception);
    }
}
