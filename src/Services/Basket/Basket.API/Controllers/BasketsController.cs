using Basket.API.Entities;
using Basket.API.Integrations.DiscountServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BasketsController : ControllerBase
    {
        private readonly ILogger<BasketsController> _logger;
        private readonly IBasketRepository _basketRepository;
        private readonly IDiscountServiceClient _discountService;

        public BasketsController(
            ILogger<BasketsController> logger,
            IBasketRepository basketRepository,
            IDiscountServiceClient discountService)
        {
            _logger = logger ?? throw new ArgumentNullException($"Argument {logger} cannot be null.");
            _basketRepository = basketRepository ?? throw new ArgumentNullException($"Argument {basketRepository} cannot be null.");
            _discountService = discountService ?? throw new ArgumentNullException($"Argument {discountService} cannot be null.");
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string userName, CancellationToken cancellationToken)
        {
            LogInfo("Get basket method invoked...");

            var cart = await _basketRepository.GetBasket(userName, cancellationToken);

            return StatusCode(StatusCodes.Status200OK, cart);
        }

        [HttpPost(Name = "Save basket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(ShoppingCart cart, CancellationToken cancellationToken)
        {
            LogInfo("Save basket method invoked...");

            await _discountService.ApplyDiscountsAsync(cart, cancellationToken);

            var persistedCart = await _basketRepository.SaveBasket(cart, cancellationToken);

            return StatusCode(StatusCodes.Status200OK, persistedCart);
        }

        [HttpDelete("{userName}", Name = "RemoveBasket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string userName, CancellationToken cancellationToken)
        {
            LogInfo("Remove basket method invoked...");

            var removedCart = await _basketRepository.RemoveBasket(userName, cancellationToken);

            return StatusCode(StatusCodes.Status200OK, removedCart);
        }

        private void LogInfo(string info, params object[] args)
        {
            Task.Run(() =>
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation(info, args);
            });
        }
    }
}
