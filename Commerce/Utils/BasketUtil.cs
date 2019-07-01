using System.Linq;
using Commerce.Models;

namespace Commerce.Utils
{
    public static class BasketUtil
    {
        public static int BasketUserCount(Basket basket)
        {
            return basket.LineItems
                .Where(l => l.CatalogName == Constants.CatalogNames.Unwrapped
                            || l.CatalogName == Constants.CatalogNames.HighStNew
                            || l.CatalogName == Constants.CatalogNames.HighStDonated
                            || l.CatalogName == Constants.CatalogNames.LittleExtras
                            || l.CatalogName == Constants.CatalogNames.Festivals)
                .Sum(item => (int)item.Quantity);
        }
    }
}
