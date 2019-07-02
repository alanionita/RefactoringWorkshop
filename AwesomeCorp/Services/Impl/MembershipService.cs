using System;
using Authentication.Models;
using Authentication.Services;
using AwesomeCorp.Constants;
using Commerce;
using Commerce.Models;
using Commerce.Services;
using Commerce.Utils;
using FakeStuff;

namespace AwesomeCorp.Services.Impl
{
    public class MembershipService : IMembershipService
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly ILoggerService _loggerService;
        private readonly IBasketService _basketService;

        private readonly ICookieService _shopCookieService;
        private readonly ICookieService _profileCookieService;

        public MembershipService(
            IAuthenticationClient authenticationClient,
            ILoggerService loggerService,
            IBasketService basketService)
        {
            _authenticationClient = authenticationClient;
            _loggerService = loggerService;
            _basketService = basketService;

            _shopCookieService = ServiceLocator.Current.GetInstance<ICookieService>(ShopCookieInfo.AuthCookieName);
            _profileCookieService =
                ServiceLocator.Current.GetInstance<ICookieService>(ProfileCookieInfo.ProfileCookieName);
        }

        public bool AuthenticateUser(string emailAddress, string password)
        {
            var profile = _authenticationClient.Login(emailAddress, password);

            var loginWasUnsuccessful = profile == null || profile.Result.Success == false;

            if (loginWasUnsuccessful)
            {
                if (profile != null && profile.Result != null && !string.IsNullOrEmpty(profile.Result.ErrorMessage))
                {
                    _loggerService.LogInfo(profile.Result.ErrorMessage);
                }

                return false;
            }

            SetUser(profile, true);

            return true;
        }

        private void SetUser(SkinnyProfile profile, bool login)
        {
            SetAccountId(profile.AccountId);
            SetCrmId(profile.CrmId);
            SetUserIsLoggedIn(login);

            var userProfile = GetProfile(profile.CommerceId);
            SetUserName(userProfile.FirstName, userProfile.Surname);

            var newCustomerId = profile.CommerceId;
            if (newCustomerId != GetCommerceId())
            {
                _basketService.TransferBasket(GetCommerceId(), newCustomerId, false);
                SetCommerceCustomerId(newCustomerId);
            }

            SetBasketCount(profile.CommerceId);
        }

        private void SetAccountId(Guid accountId)
        {
            _shopCookieService.SetPropertyValue(ShopCookieInfo.AccountIdValue, accountId.ToString());
        }

        private void SetCrmId(Guid crmId)
        {
            _shopCookieService.SetPropertyValue(ShopCookieInfo.CrmIdValue, crmId.ToString());
        }

        private void SetUserIsLoggedIn(bool loggedIn)
        {
            _shopCookieService.SetPropertyValue(ShopCookieInfo.Authenticated, loggedIn.ToString());

            if (loggedIn)
                SetProfileActiveInCookie();
            else
                SetProfileNotActiveInCookie();
        }

        private void SetProfileActiveInCookie()
        {
            _shopCookieService.SetPropertyValue(ShopCookieInfo.HasProfile, "true");
        }

        private void SetProfileNotActiveInCookie()
        {
            _profileCookieService.RemoveProperty(ShopCookieInfo.HasProfile);
            _shopCookieService.RemoveProperty(ShopCookieInfo.CustomerIdValue);
            _profileCookieService.RemoveProperty(ProfileCookieInfo.ProfileIdCookieValue);
        }

        private UserProfile GetProfile(Guid commerceId)
        {
            var profile = _authenticationClient.GetProfileByCommerceId(commerceId);

            if (profile == null || !profile.Result.Success)
                return null;

            return new UserProfile
                   {
                       Title = GetActiveTitle(profile),
                       FirstName = GetActiveFirstName(profile),
                       Surname = GetActiveLastName(profile),
                       EmailAddress = profile.Email,
                       NonRegisteredEmailAddress = profile.NonRegisteredEmail,
                       UserType = ConvertProfileToUserType(profile),
                       MatchStatus = ConvertAccountStatusToRegistrationStatus(profile)
                   };
        }

        private static string GetActiveTitle(Profile profile)
        {
            return !string.IsNullOrWhiteSpace(profile.CRMTitle)
                ? profile.CRMTitle
                : profile.CommerceTitle;
        }

        private string GetActiveFirstName(Profile profile)
        {
            return !string.IsNullOrWhiteSpace(profile.CRMFirstName)
                ? profile.CRMFirstName
                : profile.CommerceFirstName;
        }

        private string GetActiveLastName(Profile profile)
        {
            return !string.IsNullOrWhiteSpace(profile.CRMLastName)
                ? profile.CRMLastName
                : profile.CommerceLastName;
        }

        private UserTypes ConvertProfileToUserType(Profile profile)
        {
            return profile.IsRegistered ? UserTypes.Registered : UserTypes.Anonymous;
        }

        private RegistrationStatus ConvertAccountStatusToRegistrationStatus(Profile profile)
        {
            var status = profile.Status == AccountStatus.RequiresMatching
                ? RegistrationStatus.RequiresMatching
                : RegistrationStatus.Registered;
            return status;
        }

        private void SetUserName(string firstName, string surname)
        {
            _shopCookieService.SetPropertyValue(
                ShopCookieInfo.UserName, string.Format("{0} {1}", firstName, surname));
        }

        private Guid GetCommerceId()
        {
            var customerId = _profileCookieService.GetPropertyValue(ProfileCookieInfo.ProfileIdCookieValue);

            if (string.IsNullOrWhiteSpace(customerId))
            {
                customerId = Guid.NewGuid().ToString();
                _profileCookieService.SetPropertyValue(ProfileCookieInfo.ProfileIdCookieValue, customerId);
            }

            return Guid.Parse(customerId);
        }

        private void SetCommerceCustomerId(Guid customerId)
        {
            _shopCookieService.SetPropertyValue(ShopCookieInfo.CustomerIdValue, customerId.ToString());
            _profileCookieService.SetPropertyValue(ProfileCookieInfo.ProfileIdCookieValue, customerId.ToString());
        }

        private void SetBasketCount(Guid commerceId)
        {
            try
            {
                var basket = _basketService.GetCustomerBasket(commerceId);
                var count = BasketUtil.BasketUserCount(basket);
                _shopCookieService.SetPropertyValue(ShopCookieInfo.BasketCount, count.ToString());
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex);
                _shopCookieService.SetPropertyValue(ShopCookieInfo.BasketCount, "0");
            }
        }
    }
}
