using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace HojasDeVida.Trabajadores
{
    [Table("TrabajadorAptitud")]
    public class TrabajadorAptitud : FullAuditedEntity
    {
        [Index("IX_UsuarioId")]
        [StringLength(128)]
        public string UsuarioId { get; set; }
        
        [Display(Name = "Aptitud")]
        [StringLength(128)]
        public string Aptitud { get; set; }

        public virtual Trabajador Trabajador { get; set; }
    }
}
