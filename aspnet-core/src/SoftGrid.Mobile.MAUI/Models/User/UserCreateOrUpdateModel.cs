using Abp.AutoMapper;
using SoftGrid.Authorization.Users.Dto;

namespace SoftGrid.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
