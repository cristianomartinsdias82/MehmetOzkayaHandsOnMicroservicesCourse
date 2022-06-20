using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart() { Items = new List<ShoppingCartItem>(); }

        public ShoppingCart(string userName) : this() { UserName = userName; }

        public string UserName { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; }        

        public decimal CartTotal
        {
            get => Items?.Sum(it => it.Quantity * it.Price) ?? 0M;
        }
    }
}
