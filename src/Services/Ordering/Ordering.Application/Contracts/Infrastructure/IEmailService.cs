using Ordering.Application.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(Email email, CancellationToken cancellationToken = default);
        bool SendEmail(Email email, CancellationToken cancellationToken = default)
            => SendEmailAsync(email, cancellationToken).GetAwaiter().GetResult();
    }
}
