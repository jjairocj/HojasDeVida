using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace HojasDeVida.Trabajadores
{
    [Table("TrabajadorCualificacion")]
    public class TrabajadorCualificacion : FullAuditedEntity
    {
        [Index("IX_UsuarioId")]
        [StringLength(128)]
        public string UsuarioId { get; set; }
        
        [Display(Name = "Cualificaciones")]
        [StringLength(128)]
        public string Descripcion { get; set; }

        public virtual Trabajador Trabajador { get; set; }
    }
}
