using System.Collections.Generic;
using SoftGrid.Authorization.Users.Dto;
using SoftGrid.Dto;

namespace SoftGrid.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}