using System;

namespace Discount.Grpc.Entities
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }

        public static Coupon None
        {
            get => new Coupon { ProductName = "No discount.", Description = "No discount." };
        }
    }
}
