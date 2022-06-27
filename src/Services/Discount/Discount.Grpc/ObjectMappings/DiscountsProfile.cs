using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using System;

namespace Discount.Grpc.ObjectMappings
{
    public class DiscountsProfile : Profile
    {
        public DiscountsProfile()
        {
            CreateMap<Coupon, DiscountCoupon>()
                .ForMember(discountCoupon => discountCoupon.Id, coupon => coupon.MapFrom(it => $"{it.Id}"));

            CreateMap<DiscountCoupon, Coupon>()
                .ForMember(coupon => coupon.Id, discountCoupon => discountCoupon.MapFrom(it => Guid.Parse(it.Id)));
        }
    }
}