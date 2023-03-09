using System.Threading.Tasks;
using Abp.Application.Services;
using SoftGrid.MultiTenancy.Payments.Dto;
using SoftGrid.MultiTenancy.Payments.Stripe.Dto;

namespace SoftGrid.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}