using System.Collections.Generic;
using MvvmHelpers;
using SoftGrid.Models.NavigationMenu;

namespace SoftGrid.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}