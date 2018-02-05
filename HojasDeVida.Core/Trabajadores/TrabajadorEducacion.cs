using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace HojasDeVida.Trabajadores
{
    [Table("TrabajadorEducacion")]
    public class TrabajadorEducacion : FullAuditedEntity
    {
        [Index("IX_UsuarioId")]
        [StringLength(128)]
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
        [Display(Name = "Nombre del programa o título")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Fecha inicial")]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Fecha final")]
        public DateTime? FechaFinal { get; set; }

        [Display(Name = "Otra información")]
        [StringLength(8000)]
        public string Descripcion { get; set; }


        public virtual Trabajador Trabajador { get; set; }
    }
}
