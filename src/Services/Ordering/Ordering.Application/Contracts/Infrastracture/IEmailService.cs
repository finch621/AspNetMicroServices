using System.Threading.Tasks;
using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastracture;
public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}
