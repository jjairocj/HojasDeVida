using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(TrabajadorAptitud))]
    public class TrabajadorAptitudDto : EntityDto
    {
        public string UsuarioId { get; set; }
        public string Aptitud { get; set; }
    }

    [AutoMapTo(typeof(TrabajadorAptitud))]
    public class TrabajadorAptitudInput : IShouldNormalize
    {
        public int? Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }
        
        [Required]
        [StringLength(128)]
        [Display(Name = "Aptitud")]
        public string Aptitud { get; set; }
        
        public void Normalize()
        {
            Aptitud = Aptitud?.Capitalizar();
        }
        
    }
}
