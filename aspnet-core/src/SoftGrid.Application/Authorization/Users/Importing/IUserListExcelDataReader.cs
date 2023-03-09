using System.Collections.Generic;
using SoftGrid.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace SoftGrid.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
