using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(TrabajadorEducacion))]
    public class TrabajadorEducacionDto : EntityDto
    {
        public string UsuarioId { get; set; }
        public string TipoFormacion { get; set; }
        public string Institucion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
    }

    [AutoMapTo(typeof(TrabajadorEducacion))]
    public class TrabajadorEducacionInput : IShouldNormalize, ICustomValidate
    {
        public int? Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Tipo de formación")]
        public string TipoFormacion { get; set; }

        [StringLength(512)]
        [Display(Name = "Nombre de la institución")]
        public string Institucion { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Nombre del programa, título o curso")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Fecha inicial")]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Fecha final")]
        public DateTime? FechaFinal { get; set; }

        [Required]
        [StringLength(8000)]
        [Display(Name = "Otra información")]
        public string Descripcion { get; set; }


        public void Normalize()
        {
            Institucion = Institucion?.FormatearNombres();
            Nombre = Nombre?.FormatearNombres();
        }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (FechaFinal != null && FechaFinal < FechaInicial)
            {
                context.Results.Add(new ValidationResult("La fecha final no puede ser menor que la fecha inicial"));
            }

            if (FechaFinal != null && FechaFinal > DateTime.UtcNow.AddHours(-5).AddDays(1))
            {
                context.Results.Add(new ValidationResult("La fecha final no puede ser superior a la fecha actual"));
            }
        }
    }
}
