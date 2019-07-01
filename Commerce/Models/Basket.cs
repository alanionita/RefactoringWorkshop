using System.Collections.Generic;

namespace Commerce.Models
{
    public class Basket
    {
        public List<LineItem> LineItems { get; set; }
    }
}