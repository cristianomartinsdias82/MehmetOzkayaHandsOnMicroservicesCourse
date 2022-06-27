using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountsController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException($"Argument {discountRepository} cannot be null.");
        }

        [ProducesResponseType(typeof(IEnumerable<Coupon>), StatusCodes.Status200OK)]
        [HttpGet(Name = "GetDiscounts")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
            => Ok(await _discountRepository.GetDiscountsAsync(cancellationToken));

        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{productName}", Name = "GetDiscount")]
        public async Task<IActionResult> Get(string productName, CancellationToken cancellationToken)
            => Ok(await _discountRepository.GetDiscountByProductNameAsync(productName, cancellationToken));

        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        [HttpPost(Name = "AddDiscount")]
        public async Task<IActionResult> Post(Coupon coupon, CancellationToken cancellationToken)
        {
            var newlyCreatedDiscount = await _discountRepository.AddDiscountAsync(coupon, cancellationToken);

            return CreatedAtRoute(
                "GetDiscount",
                new { productName = coupon.ProductName },
                newlyCreatedDiscount);
        }

        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [HttpPut(Name = "UpdateDiscount")]
        public async Task<IActionResult> Put(Coupon coupon, CancellationToken cancellationToken)
            => Ok(await _discountRepository.UpdateDiscountAsync(coupon, cancellationToken));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        public async Task<IActionResult> Delete(string productName, CancellationToken cancellationToken)
        {
            await _discountRepository.DeleteDiscountAsync(productName, cancellationToken);

            return NoContent();
        }
    }
}
