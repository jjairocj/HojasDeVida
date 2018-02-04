using System.Collections.Generic;
using HojasDeVida.Roles.Dto;

namespace HojasDeVida.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }

        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}