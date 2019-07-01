namespace AwesomeCorp.Services
{
    public interface ICookieService
    {
        void SetPropertyValue(string property, string value);
        void RemoveProperty(string property);
        string GetPropertyValue(string property);
    }
}
