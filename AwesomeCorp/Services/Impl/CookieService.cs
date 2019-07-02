using FakeStuff;

namespace AwesomeCorp.Services.Impl
{
    public class CookieService : ICookieService
    {
        private readonly string _name;
        private readonly IContext _context;

        public CookieService(string name, IContext context)
        {
            _name = name;
            _context = context;
        }

        public void SetPropertyValue(string property, string value)
        {
            var cookie = _context.Cookies[_name];
            cookie.Values[property] = value;
        }

        public void RemoveProperty(string property)
        {
            var cookie = _context.Cookies[_name];
            cookie.Values.Remove(property);
        }

        public string GetPropertyValue(string property)
        {
            var cookie = _context.Cookies[_name];
            return cookie.Values[property] ?? string.Empty;
        }
    }
}
