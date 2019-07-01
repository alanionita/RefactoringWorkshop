using System;
using Authentication.Models;

namespace Authentication.Services
{
    public interface IAuthenticationClient
    {
        SkinnyProfile Login(string emailAddress, string password);
        Profile GetProfileByCommerceId(Guid commerceId);
    }
}
