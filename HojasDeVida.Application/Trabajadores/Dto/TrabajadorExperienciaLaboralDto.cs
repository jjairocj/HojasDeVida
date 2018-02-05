using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(TrabajadorExperienciaLaboral))]
    public class TrabajadorExperienciaLaboralDto : EntityDto
    {
        public string UsuarioId { get; set; }
        public string NombreEmpresa { get; set; }
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public string DepartamentoCodigo { get; set; }
        public string Municipio { get; set; }
        public string MunicipioCodigo { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string Descripcion { get; set; }
        public string NombreJefe { get; set; }
        public string ContactoJefe { get; set; }
    }

    [AutoMapTo(typeof(TrabajadorExperienciaLaboral))]
    public class TrabajadorExperienciaLaboralInput : IShouldNormalize, ICustomValidate
    {
        public int? Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Nombre de la empresa")]
        public string NombreEmpresa { get; set; }
        
        [Required]
        [StringLength(512)]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Departamento")]
        public string DepartamentoCodigo { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Municipio")]
        public string MunicipioCodigo { get; set; }

        [Required]
        [Display(Name = "Fecha inicial")]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Fecha final")]
        public DateTime? FechaFinal { get; set; }

        [Display(Name = "Actualmente trabajo aquí")]
        public bool ActualmenteTrabajoAqui { get; set; }

        [Required]
        [StringLength(8000)]
        [Display(Name = "Otra información")]
        public string Descripcion { get; set; }

        [StringLength(512)]
        [Display(Name = "Nombre del jefe directo")]
        public string NombreJefe { get; set; }

        [StringLength(512)]
        [Display(Name = "Teléfono o correo del jefe directo")]
        public string ContactoJefe { get; set; }

        public void Normalize()
        {
            NombreEmpresa = NombreEmpresa?.FormatearNombres();
            Cargo = Texto.PrimeraLetraEnMayuscula(Cargo?.Trim());
            NombreJefe = NombreJefe?.FormatearNombres();
            ContactoJefe = ContactoJefe?.FormatearNombres();
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
