using System.Collections.Generic;

namespace FakeStuff
{
    public class FakeCookie
    {
        public Dictionary<string, string> Values { get; set; }
    }

    public interface IContext {
        Dictionary<string, FakeCookie> Cookies { get; set; }
    }

    public class FakeContext : IContext
    {
        public Dictionary<string, FakeCookie> Cookies { get; set; }
    }

    public interface IServiceLocator
    {
        T GetInstance<T>(string name);
    }

    public static class ServiceLocator
    {
        public static IServiceLocator Current { get; set; }
    }
}
