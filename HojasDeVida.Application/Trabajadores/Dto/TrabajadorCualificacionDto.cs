using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(TrabajadorCualificacion))]
    public class TrabajadorCualificacionDto : EntityDto
    {
        public string UsuarioId { get; set; }
        public string Descripcion { get; set; }
    }

    [AutoMapTo(typeof(TrabajadorCualificacion))]
    public class TrabajadorCualificacionInput : IShouldNormalize
    {
        public int? Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Cualificaciones")]
        public string Descripcion { get; set; }

        public void Normalize()
        {
            
        }

    }
}
