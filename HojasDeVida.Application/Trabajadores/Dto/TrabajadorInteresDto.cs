using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(TrabajadorInteres))]
    public class TrabajadorInteresDto : EntityDto
    {
        public string UsuarioId { get; set; }
        public string Descripcion { get; set; }
    }

    [AutoMapTo(typeof(TrabajadorCualificacion))]
    public class TrabajadorInteresInput : IShouldNormalize
    {
        public int? Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Intereses")]
        public string Descripcion { get; set; }

        public void Normalize()
        {

        }

    }
}
