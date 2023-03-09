using Abp.Authorization;
using SoftGrid.Authorization.Roles;
using SoftGrid.Authorization.Users;

namespace SoftGrid.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
