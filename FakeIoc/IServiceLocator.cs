namespace FakeIoc
{
    public interface IServiceLocator
    {
        T GetInstance<T>(string name);
    }
}
