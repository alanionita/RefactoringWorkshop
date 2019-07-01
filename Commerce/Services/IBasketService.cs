using System;
using Commerce.Models;

namespace Commerce.Services
{
    public interface IBasketService
    {
        void TransferBasket(Guid fromCustomerId, Guid toCustomerId, bool emptyTargetBasket);
        Basket GetCustomerBasket(Guid commerceId);
    }
}
