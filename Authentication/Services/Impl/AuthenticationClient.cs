using System;
using Authentication.Data;
using Authentication.Models;
using Authentication.Routing;
using Commerce.Services;

namespace Authentication.Services.Impl
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IProfileService _profileService;
        private readonly IRoutingEngine _routingEngine;

        public AuthenticationClient(
            IAccountRepository accountRepository,
            IProfileService profileService,
            IRoutingEngine routingEngine)
        {
            _accountRepository = accountRepository;
            _profileService = profileService;
            _routingEngine = routingEngine;
        }

        public SkinnyProfile Login(string email, string password)
        {
            // Try and retrieve the account based on the email and password provided
            var profile = _accountRepository.GetAccount(email, password);

            // If profile found
            if (profile != null)
            {
                // Check the profile has the necessary profiles
                if (CheckProfile(ref profile))
                {
                    // If a match not found in the CRM
                    if (profile.Status != AccountStatus.RequiresMatching)
                        profile.Status = AccountStatus.Success;
                }
                else
                {
                    // Return blank profile project with a failure message
                    profile = new SkinnyProfile
                    {
                        Result = new Result { Success = false, ErrorMessage = "CheckProfile returned false" }
                    };
                }
            }
            else
            {
                // Return blank profile project with a failure message
                profile = new SkinnyProfile { Result = new Result { Success = false, ErrorMessage = "Profile is null" } };
            }

            return profile;
        }

        public Profile GetProfileByCommerceId(Guid commerceId)
        {
            // Try and retrieve the account based on the email and password provided
            var skinnyProfile = _accountRepository.GetAccountByCommerceId(commerceId);

            if (skinnyProfile == null)
            {
                // Return blank profile project with a failure message
                return new Profile { Result = new Result { Success = false, ErrorMessage = "SkinnyProfile is null" } };
            }

            var profile = _routingEngine.RouteGetProfileRequest(skinnyProfile);

            // Does this account require matching?
            if (profile.CrmId == Guid.Empty)
                profile.Status = AccountStatus.RequiresMatching;

            return profile;
        }

        private bool CheckProfile(ref SkinnyProfile skinnyProfile)
        {
            var isValid = true;

            // Check commerce is null
            if (skinnyProfile.CommerceId == Guid.Empty)
            {
                // Bad account - delete it
                _accountRepository.DeleteAccount(skinnyProfile.AccountId);
                isValid = false;
            }
            else
            {
                var userProfile = _profileService.GetUserProfile(skinnyProfile.CommerceId);
                if (userProfile == null)
                {
                    // Could not load the commerce profile, an error may have occurred, or worse the account no longer exists...
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
