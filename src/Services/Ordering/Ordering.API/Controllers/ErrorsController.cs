using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Exceptions;

namespace Ordering.API.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            var code = StatusCodes.Status500InternalServerError;

            if (exception is ValidationException valExc)
            {
                code = StatusCodes.Status400BadRequest;
                HttpContext.Items.Add("validationErrors", valExc.Errors); // >>> ApplicationProblemDetailsFactory.ApplyProblemDetailsDefaults method will consume this information and output it to the response
            }

            return Problem(
                title: exception.Message,
                type: exception.GetType().FullName,
                statusCode: code);
        }
    }
}