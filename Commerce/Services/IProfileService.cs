using System;
using Commerce.Models;

namespace Commerce.Services
{
    public interface IProfileService
    {
        UserProfile GetUserProfile(Guid commerceId);
    }
}
