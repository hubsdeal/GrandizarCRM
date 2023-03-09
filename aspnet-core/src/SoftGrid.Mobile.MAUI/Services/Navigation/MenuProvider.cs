using Abp.Dependency;
using SoftGrid.Localization;
using SoftGrid.Models.NavigationMenu;
using SoftGrid.Services.Permission;

namespace SoftGrid.Services.Navigation
{
    public class MenuProvider : ISingletonDependency, IMenuProvider
    {
        /* For more icons:
            https://material.io/icons/
        */
        private List<NavigationMenuItem> _menuItems;

        public void InitializeMenuItems()
        {
            _menuItems = new List<NavigationMenuItem>
            {
                new NavigationMenuItem
                {
                    Title = L.Localize("Tenants"),
                    Icon = "fa-solid fa-list",
                    NavigationUrl = NavigationUrlConsts.Tenants,
                    RequiredPermissionName = PermissionKey.Tenants,
                },
                new NavigationMenuItem
                {
                    Title = L.Localize("Users"),
                    Icon = "fa-solid fa-filter",
                    NavigationUrl= NavigationUrlConsts.User,
                    RequiredPermissionName = PermissionKey.Users,
                },
                new NavigationMenuItem
                {
                    Title = L.Localize("MySettings"),
                    Icon = "fa-solid fa-cog",
                    NavigationUrl  = NavigationUrlConsts.Settings
                }

                /*This is a sample menu item to guide how to add a new item.
                    ,new NavigationMenuItem
                    {
                        Title = "Sample View",
                        Icon = "MyIcon.png",
                        TargetType = typeof(_SampleView),
                        Order = 10
                    }
                */
            };
        }

        public List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions)
        {
            InitializeMenuItems();

            var authorizedMenuItems = new List<NavigationMenuItem>();

            foreach (var menuItem in _menuItems)
            {
                if (menuItem.RequiredPermissionName == null)
                {
                    authorizedMenuItems.Add(menuItem);
                    continue;
                }

                if (grantedPermissions != null &&
                    grantedPermissions.ContainsKey(menuItem.RequiredPermissionName))
                {
                    authorizedMenuItems.Add(menuItem);
                }
            }

            return authorizedMenuItems;
        }
    }
}