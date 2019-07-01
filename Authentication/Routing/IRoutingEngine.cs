using Authentication.Models;

namespace Authentication.Routing
{
    public interface IRoutingEngine
    {
        Profile RouteGetProfileRequest(SkinnyProfile skinnyProfile);
    }
}
