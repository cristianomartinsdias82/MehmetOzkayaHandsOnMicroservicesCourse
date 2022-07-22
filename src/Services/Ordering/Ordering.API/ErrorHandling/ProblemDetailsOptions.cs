using System;

namespace Ordering.API.ErrorHandling
{
    /// <summary>
    /// Options for controlling the behavior of <see cref="IProblemDetailsService.WriteAsync(ProblemDetailsContext)"/>
    /// and similar methods.
    /// </summary>
    public class ProblemDetailsOptions
    {
        /// <summary>
        /// The operation that customizes the current <see cref="Mvc.ProblemDetails"/> instance.
        /// </summary>
        public Action<ProblemDetailsContext> CustomizeProblemDetails { get; set; }
    }
}
