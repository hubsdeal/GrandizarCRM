using System.Threading.Tasks;

namespace SoftGrid.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}