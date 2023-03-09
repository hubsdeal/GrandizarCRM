using System.Threading.Tasks;
using SoftGrid.Sessions.Dto;

namespace SoftGrid.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
