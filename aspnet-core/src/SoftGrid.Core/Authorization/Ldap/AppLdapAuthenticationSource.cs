using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using SoftGrid.Authorization.Users;
using SoftGrid.MultiTenancy;

namespace SoftGrid.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}