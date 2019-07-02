using AwesomeCorp.Constants;
using AwesomeCorp.Services;
using AwesomeCorp.Services.Impl;
using FakeStuff;

namespace AwesomeCorp.Ioc
{
    public class AwesomeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICookieService>().To<CookieService>().Named(ShopCookieInfo.AuthCookieName).InRequestScope();
            Bind<ICookieService>().To<CookieService>().Named(ProfileCookieInfo.ProfileCookieName).InRequestScope();
        }
    }
}
