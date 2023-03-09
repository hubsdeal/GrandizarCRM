using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using SoftGrid.MultiTenancy.Accounting.Dto;

namespace SoftGrid.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
