using System;
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

    public abstract class NinjectModule
    {
        public abstract void Load();

        protected BindingBuilder<T> Bind<T>()
        {
            return new BindingBuilder<T>();
        }
    }

    public class BindingBuilder<T> {
        public BindingBuilder<T> To<T>()
        {
            return new BindingBuilder<T>();
        }

        public BindingBuilder<T> Named(string name)
        {
            return new BindingBuilder<T>();
        }

        public void InRequestScope()
        {
            throw new NotImplementedException();
        }
    }
}
