using System.Collections.Generic;
using SoftGrid.Authorization.Users.Importing.Dto;
using SoftGrid.Dto;

namespace SoftGrid.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
