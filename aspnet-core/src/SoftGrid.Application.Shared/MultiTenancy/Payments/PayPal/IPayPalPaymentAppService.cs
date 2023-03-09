using System.Threading.Tasks;
using Abp.Application.Services;
using SoftGrid.MultiTenancy.Payments.PayPal.Dto;

namespace SoftGrid.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
