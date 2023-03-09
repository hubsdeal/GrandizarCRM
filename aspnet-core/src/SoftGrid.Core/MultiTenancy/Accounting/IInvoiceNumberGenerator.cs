using System.Threading.Tasks;
using Abp.Dependency;

namespace SoftGrid.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}