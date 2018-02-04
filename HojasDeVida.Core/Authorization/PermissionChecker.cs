using Abp.Authorization;
using HojasDeVida.Authorization.Roles;
using HojasDeVida.Authorization.Users;

namespace HojasDeVida.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
