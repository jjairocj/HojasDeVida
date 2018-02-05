using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(TrabajadorReferencia))]
    public class TrabajadorReferenciaDto : EntityDto
    {
        public string UsuarioId { get; set; }
        public string Descripcion { get; set; }
    }

    [AutoMapTo(typeof(TrabajadorReferencia))]
    public class TrabajadorReferenciaInput : IShouldNormalize
    {
        public int? Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Referencias")]
        public string Descripcion { get; set; }

        public void Normalize()
        {

        }

    }
}
