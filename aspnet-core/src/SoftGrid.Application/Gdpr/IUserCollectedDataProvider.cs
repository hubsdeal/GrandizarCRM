using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using SoftGrid.Dto;

namespace SoftGrid.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
