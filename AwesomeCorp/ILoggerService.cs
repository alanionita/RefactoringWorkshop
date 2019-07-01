using System;

namespace BigMess
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogError(Exception exception);
    }
}