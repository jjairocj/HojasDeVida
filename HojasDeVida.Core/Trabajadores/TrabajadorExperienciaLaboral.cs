using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace HojasDeVida.Trabajadores
{
    [Table("TrabajadorExperienciaLaboral")]
    public class TrabajadorExperienciaLaboral : FullAuditedEntity
    {
        [Index("IX_UsuarioId")]
        [StringLength(128)]
        public string UsuarioId { get; set; }
        
        [Required]
        [StringLength(512)]
        public string NombreEmpresa { get; set; }

        [Required]
        [StringLength(512)]
        public string Cargo { get; set; }
        
        [StringLength(512)]
        public string Departamento { get; set; }

        [StringLength(512)]
        public string DepartamentoCodigo { get; set; }

        [StringLength(512)]
        public string Municipio { get; set; }

        [StringLength(512)]
        public string MunicipioCodigo { get; set; }

        [Required]
        public DateTime? FechaInicial { get; set; }

        public DateTime? FechaFinal { get; set; }
        
        [StringLength(8000)]
        public string Descripcion { get; set; }

        [StringLength(512)]
        public string NombreJefe { get; set; }

        [StringLength(512)]
        public string ContactoJefe { get; set; }

        public virtual Trabajador Trabajador { get; set; }
    }
}
