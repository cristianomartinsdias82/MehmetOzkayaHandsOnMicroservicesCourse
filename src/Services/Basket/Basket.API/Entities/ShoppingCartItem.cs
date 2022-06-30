using System;

namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public string Color { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; private set; }

        public void ApplyDiscount(decimal discount)
        {
            if (!DiscountApplied)
                Discount = discount;
        }

        public bool DiscountApplied
            => Discount > 0M;
    }
}