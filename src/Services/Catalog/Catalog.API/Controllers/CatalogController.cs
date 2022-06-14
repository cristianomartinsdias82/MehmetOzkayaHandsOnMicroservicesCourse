using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using static Catalog.API.ExceptionHelper;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
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

        private void LogInfo(string info, params object[] args)
        {
            Task.Run(() =>
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation(info, args);
            });            
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            LogInfo("Get method invoked. Url: {url}", ControllerContext.RouteData.Values["action"]);

            return Ok(await _productRepository.GetAll(cancellationToken));
        }

        [HttpGet("{id:Guid}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            LogInfo("GetById method invoked...");

            return Ok(await _productRepository.GetById(id, cancellationToken));
        }


        [HttpGet("get-by-title/{title}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByTitle(string title, CancellationToken cancellationToken)
        {
            LogInfo("GetByTitle method invoked...");

            return Ok(await _productRepository.GetByTitle(title, cancellationToken));
        }

        [HttpGet("get-by-category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCategory(string category, CancellationToken cancellationToken)
        {
            LogInfo("GetByCategory method invoked...");

            return Ok(await _productRepository.GetByCategory(category, cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Product product, CancellationToken cancellationToken)
        {
            LogInfo("Post method invoked...");

            await _productRepository.Create(product, cancellationToken);

            //return Ok(await _productRepository.Create(product, cancellationToken));
            return CreatedAtRoute(nameof(GetById), new { id = Guid.NewGuid() }, new Product());
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(Product product, CancellationToken cancellationToken)
        {
            LogInfo("Update method invoked...");

            return Ok(await _productRepository.Update(product, cancellationToken));
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            LogInfo("Delete method invoked...");

            return Ok(await _productRepository.Delete(id, cancellationToken));
        }
    }
}
