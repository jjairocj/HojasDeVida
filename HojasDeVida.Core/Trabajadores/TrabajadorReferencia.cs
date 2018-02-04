using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace HojasDeVida.Trabajadores
{
    [Table("TrabajadorReferencia")]
    public class TrabajadorReferencia : FullAuditedEntity
    {
        [Index("IX_UsuarioId")]
        [StringLength(128)]
        public string UsuarioId { get; set; }
        
        [Display(Name = "Referencias")]
        [StringLength(8000)]
        public string Descripcion { get; set; }

        public virtual Trabajador Trabajador { get; set; }
    }
}
