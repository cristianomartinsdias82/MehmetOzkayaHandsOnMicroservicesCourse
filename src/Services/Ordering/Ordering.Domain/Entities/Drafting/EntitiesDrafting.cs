using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.Entities.Drafting
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class Customer : Person
    {
        public string UserName { get; set; }
        public Address HomeAddress { get; set; }
        public virtual IReadOnlyCollection<Card> Cards { get; private set; }
    }

    public class Card
    {
        public CardType CardType { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expires { get; set; }
        public string Cvv { get; set; }
    }

    public enum CardType
    {
        CreditCard = 1,
        DebitCard = 2,
        DebitAndCreditCard = 3
    }

    public class Address
    {
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public AddressType AddressType { get; set; } = AddressType.Billing;
    }

    public enum AddressType
    {
        Billing = 1
    }

    public enum PaymentMethod
    {
        CreditCard = 1
    }

    public class Product
    {
        public string ProductName { get; set; }
        public string Color { get; set; }
        public decimal ListPrice { get; set; }
    }

    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPerUnit { get; set; }
        public decimal SubTotal => (Product.ListPrice - DiscountPerUnit) * Quantity;
    }

    public class Order : Entity
    {
        public virtual IReadOnlyCollection<OrderItem> OrderItems { get; private set; }

        public decimal OrderTotal => OrderItems?.Sum(it => it.SubTotal) ?? 0M;

        public PaymentMethod PaymentMethod { get; private set; }

        public Customer Customer { get; private set; }

        //Billing address
        public Address BillingAddress { get; private set; }

        //Shipping address
        public Address ShippingAddress { get; private set; }

        //Payment data
        public Card Card { get; private set; }

        public Guid OrderNumber { get; private set; }

        public Order(
            Customer customer,
            Address billingAddress,
            Address shippingAddress,
            IEnumerable<OrderItem> orderItems,
            Card card,
            PaymentMethod paymentMethod)
        {
            Customer = customer;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            OrderItems = new List<OrderItem>(orderItems);
            Card = card;
            PaymentMethod = PaymentMethod;
            OrderNumber = Guid.NewGuid();
        }
    }
}