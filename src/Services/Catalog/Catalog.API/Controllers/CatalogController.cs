using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using static Catalog.API.ExceptionHelper;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(
            IProductRepository productRepository,
            ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw NullArgumentException(nameof(productRepository));
            _logger = logger ?? throw NullArgumentException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            LogInfo("Get method invoked. Url: {url}", ControllerContext.RouteData.Values["action"]);

            return Ok(await _productRepository.GetAll(cancellationToken));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            LogInfo("GetById method invoked...");

            return Ok(await _productRepository.GetById(id, cancellationToken));
        }


        [HttpGet("get-by-title/{title}")]
        public async Task<IActionResult> GetByTitle(string title, CancellationToken cancellationToken)
        {
            LogInfo("GetByTitle method invoked...");

            return Ok(await _productRepository.GetByTitle(title, cancellationToken));
        }

        [HttpGet("get-by-category/{category}")]
        public async Task<IActionResult> GetByCategory(string category, CancellationToken cancellationToken)
        {
            LogInfo("GetByCategory method invoked...");

            return Ok(await _productRepository.GetByCategory(category, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product, CancellationToken cancellationToken)
        {
            LogInfo("Post method invoked...");

            await _productRepository.Create(product, cancellationToken);

            return Ok(new { product, successful = true });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Product product, CancellationToken cancellationToken)
        {
            LogInfo("Update method invoked...");

            return Ok(await _productRepository.Update(product, cancellationToken));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            LogInfo("Delete method invoked...");

            return Ok(await _productRepository.Delete(id, cancellationToken));
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
