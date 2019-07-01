using System;
using Authentication.Models;

namespace Authentication.Data
{
    public interface IAccountRepository
    {
        SkinnyProfile GetAccount(string email, string password);
        void DeleteAccount(Guid accountId);
        SkinnyProfile GetAccountByCommerceId(Guid commerceId);
    }
}