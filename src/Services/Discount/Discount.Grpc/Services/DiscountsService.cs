using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Discount.Grpc.Protos.DiscountsServiceProto;

namespace Discount.Grpc.Services
{
    public class DiscountsService : DiscountsServiceProtoBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountsService> _logger;
        private readonly IMapper _mapper;

        public DiscountsService(
            IDiscountRepository discountRepository,
            ILogger<DiscountsService> logger,
            IMapper mapper)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException($"Argument {nameof(discountRepository)} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {nameof(logger)} cannot be null.");
            _mapper = mapper ?? throw new ArgumentNullException($"Argument {nameof(mapper)} cannot be null.");
        }

        public override async Task<GetDiscountByProductReply> GetDiscountByProduct(GetDiscountByProductRequest request, ServerCallContext context)
        {
            Log(_logger.Log, "Retrieving discount for Product {Product}...", args: request.ProductName);

            var coupon = await _discountRepository.GetDiscountByProductNameAsync(request.ProductName, context.CancellationToken);

            return new GetDiscountByProductReply
            {
                Coupon = _mapper.Map<DiscountCoupon>(coupon)
            };
        }

        public override async Task<GetDiscountsReply> GetDiscounts(GetDiscountsRequest request, ServerCallContext context)
        {
            Log(_logger.Log, "Retrieving discounts...");

            var coupons = await _discountRepository.GetDiscountsAsync(context.CancellationToken);
            var reply = new GetDiscountsReply();
            coupons?.ToList().ForEach(coupon => reply.Coupons.Add(_mapper.Map<DiscountCoupon>(coupon)));
            
            return reply;
        }

        public override async Task<AddDiscountReply> AddDiscount(AddDiscountRequest request, ServerCallContext context)
        {
            Log(_logger.Log, "Adding discount for Product {Product}...", args: request.Coupon.ProductName);

            var newlyCreatedCoupon = await _discountRepository.AddDiscountAsync(
                _mapper.Map<Coupon>(request.Coupon),
                context.CancellationToken);

            return new AddDiscountReply
            {
                Coupon = _mapper.Map<DiscountCoupon>(newlyCreatedCoupon)
            };
        }

        public override async Task<UpdateDiscountReply> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            Log(_logger.Log, "Updating discount for Product {Product}...", args: request.Coupon.ProductName);

            var updatedCreatedCoupon = await _discountRepository.UpdateDiscountAsync(
                _mapper.Map<Coupon>(request.Coupon),
                context.CancellationToken);

            return new UpdateDiscountReply
            {
                Coupon = _mapper.Map<DiscountCoupon>(updatedCreatedCoupon)
            };
        }

        public override async Task<RemoveDiscountReply> RemoveDiscount(RemoveDiscountRequest request, ServerCallContext context)
        {
            Log(_logger.Log, "Removing discount for Product {Product}...", args: request.ProductName);

            await _discountRepository.DeleteDiscountAsync(request.ProductName, context.CancellationToken);

            return new RemoveDiscountReply
            {
                ProductName = request.ProductName
            };
        }

        private static void Log(
            Action<LogLevel, string, object[]> logAction,
            string message,
            LogLevel logLevel = LogLevel.Information,
            params object[] args)
                => Task.Run(() => logAction(logLevel, message, args));
    }
}
