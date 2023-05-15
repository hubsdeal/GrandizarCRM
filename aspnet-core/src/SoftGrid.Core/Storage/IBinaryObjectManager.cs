using System;
using System.Threading.Tasks;

namespace SoftGrid.Storage
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id);
        Task SaveAsync(BinaryObject file);
        Task DeleteAsync(Guid id);
        string GetPictureUrl(Guid id, string extension);
        Task<string> GetProductPictureUrlAsync(Guid id, string extension);
        Task<string> GetStorePictureUrlAsync(Guid id, string extension);
        Task<string> GetOthersPictureUrlAsync(Guid id, string extension);
        Task<string> GetFileUrlAsync(Guid id, string fileName);
    }
}