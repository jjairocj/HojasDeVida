using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace HojasDeVida.Autorizacion
{
    public class HojasDeVidaAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var trabajadores = context.GetPermissionOrNull(PermissionNames.Trabajadores) ??
                            context.CreatePermission(PermissionNames.Trabajadores, L("PermisoTrabajadores"));
            
            var usuarios = context.GetPermissionOrNull(PermissionNames.Usuarios) ??
                           context.CreatePermission(PermissionNames.Usuarios, L("PermisoUsuarios"));

            var configuracion = context.GetPermissionOrNull(PermissionNames.Configuracion) ??
                                context.CreatePermission(PermissionNames.Configuracion, L("PermisoConfiguracion"));

            configuracion.CreateChildPermission(PermissionNames.ConfiguracionCargos, L("PermisoConfiguracionCargo"));
            configuracion.CreateChildPermission(PermissionNames.ConfiguracionCatalogo, L("PermisoConfiguracionCatalogo"));

            // Portal Trabajadores - Empresas

            var perfilTrabajador = context.GetPermissionOrNull(PermissionNames.PerfilTrabajador) ??
                                   context.CreatePermission(PermissionNames.PerfilTrabajador,
                                       L("PermisoPerfilTrabajador"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, HojasDeVidaConsts.LocalizationSourceName);
        }
    }
}
