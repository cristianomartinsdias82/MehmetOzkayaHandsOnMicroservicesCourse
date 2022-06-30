using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
            => Items = new List<ShoppingCartItem>();

        public ShoppingCart(string userName) : this() { UserName = userName; }

        public string UserName { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; }

        public decimal CartTotal
        {
            get => Items?.Sum(it => it.Quantity * it.Price - it.Discount) ?? 0M;
        }

        public void AddDiscountForProduct(string productName, decimal discount)
        {
            var productItem = Items
                                .FirstOrDefault(x => (x.ProductName?.ToUpperInvariant() ?? string.Empty)
                                                     .Equals(productName, StringComparison.InvariantCultureIgnoreCase));

            productItem.ApplyDiscount(discount);
        }
    }
}