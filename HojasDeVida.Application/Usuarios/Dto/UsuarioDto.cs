using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using TeRecomiendo.Usuarios;

namespace HojasDeVida.Usuarios.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UsuarioDto
    {
        public long Id { get; set; }
        public string UsuarioGuid { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreUsuario { get; set; }
        public string Perfil { get; set; }
        public string Tipo { get; set; }
        public string CorreoElectronico { get; set; }
        public string Celular { get; set; }
        public DateTime? UltimoInicioSesion { get; set; }
        public DateTime Registro { get; set; }
        public DateTime? Actualizacion { get; set; }
    }

    [AutoMapTo(typeof(User))]
    public class EditarContrasenaInput : IShouldNormalize
    {
        [Required]
        [StringLength(128)]
        public string UsuarioId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Contrasena", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmarContrasena { get; set; }

        public void Normalize()
        {
            Contrasena = Contrasena?.Trim();
            ConfirmarContrasena = ConfirmarContrasena?.Trim();
        }
    }

    [AutoMapTo(typeof(User))]
    public class NuevaContrasenaInput : IShouldNormalize
    {
        [Required]
        [StringLength(128)]
        public string UsuarioId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Display(Name = "Nueva contraseña")]
        public string NuevaContrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("NuevaContrasena", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmarContrasena { get; set; }

        public void Normalize()
        {
            NuevaContrasena = NuevaContrasena?.Trim();
            ConfirmarContrasena = ConfirmarContrasena?.Trim();
        }
    }

    [AutoMapTo(typeof(User))]
    public class RestaurarContrasenaInput : IShouldNormalize
    {
        [Required]
        [StringLength(128)]
        public string Token { get; set; }

        [Required]
        public MedioContacto MedioSeleccionado { get; set; }

        [Required]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Display(Name = "Nueva contraseña")]
        public string NuevaContrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("NuevaContrasena", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmarContrasena { get; set; }

        public void Normalize()
        {
            NuevaContrasena = NuevaContrasena?.Trim();
            ConfirmarContrasena = ConfirmarContrasena?.Trim();
        }
    }

    public class ListarInput : PagedAndSortedResultRequest, IShouldNormalize
    {
        public DateTime? FiltroFechaInicioSesionInicial { get; set; }
        public DateTime? FiltroFechaInicioSesionFinal { get; set; }
        public DateTime? FiltroFechaRegistroInicial { get; set; }
        public DateTime? FiltroFechaRegistroFinal { get; set; }
        public DateTime? FiltroFechaActualizacionInicial { get; set; }
        public DateTime? FiltroFechaActualizacionFinal { get; set; }
        public bool? FiltroEsActivo { get; set; }
        public string FiltroTipo { get; set; }
        public string Filtro { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Name, Surname";
            }

            FiltroFechaInicioSesionInicial = FiltroFechaInicioSesionInicial?.AddHours(-5);
            FiltroFechaInicioSesionFinal = FiltroFechaInicioSesionFinal?.AddHours(-5);
            FiltroFechaRegistroInicial = FiltroFechaRegistroInicial?.AddHours(-5);
            FiltroFechaRegistroFinal = FiltroFechaRegistroFinal?.AddHours(-5);
            FiltroFechaActualizacionInicial = FiltroFechaActualizacionInicial?.AddHours(-5);
            FiltroFechaActualizacionFinal = FiltroFechaActualizacionFinal?.AddHours(-5);
        }
    }
}
