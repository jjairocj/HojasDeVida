using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace HojasDeVida.Usuarios
{
    public enum TiposUsuarios
    {
        Trabajador,
        FullAdmin,
    }

    public class User : AbpUser<User>
    {
        public string UsuarioGuid { get; set; }

        [Required]
        [StringLength(2048)]
        public override string Name { get; set; }

        [Required]
        [StringLength(2048)]
        public override string Surname { get; set; }

        [StringLength(2048)]
        public override string Password { get; set; }

        [Required]
        [StringLength(2048)]
        public override string UserName { get; set; }

        [StringLength(256)]
        public string Tipo { get; set; }

        public bool AceptoTerminosCondiciones { get; set; }

        public bool AceptoPoliticaDatos { get; set; }

        public bool AceptoPoliticaPrivacidad { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public User()
        {
            UsuarioGuid = Guid.NewGuid().ToString();
        }
    }
}