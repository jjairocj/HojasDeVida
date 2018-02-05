using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HojasDeVida.Autorizacion.Roles;

namespace HojasDeVida.Usuarios.Dto
{
    [AutoMapFrom(typeof(Role))]
    public class PerfilDto : EntityDto
    {
        [Display(Name = "Nombre del perfil")]
        public string Nombre { get; set; }
    }

    [AutoMapTo(typeof(Role))]
    public class CrearPerfilInput
    {
        [Required]
        [Display(Name = "Nombre del perfil")]
        public string Nombre { get; set; }
    }

    [AutoMapTo(typeof(Role))]
    public class ActualizarPerfilInput : CrearPerfilInput
    {
        public int Id { get; set; }
    }
}
