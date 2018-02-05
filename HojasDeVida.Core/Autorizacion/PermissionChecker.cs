using Abp.Authorization;
using HojasDeVida.Autorizacion.Roles;
using HojasDeVida.Usuarios;

namespace HojasDeVida.Autorizacion
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
