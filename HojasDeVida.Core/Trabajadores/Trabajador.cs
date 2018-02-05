using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace HojasDeVida.Trabajadores
{
    [Table("Trabajador")]
    public class Trabajador : FullAuditedEntity<Guid>
    {
        public Trabajador()
        {
            ExperienciaLaboral = new List<TrabajadorExperienciaLaboral>();
            Educacion = new List<TrabajadorEducacion>();
            Cualificacion = new List<TrabajadorCualificacion>();
            Interes = new List<TrabajadorInteres>();
            Referencia = new List<TrabajadorReferencia>();
            Aptitud = new List<TrabajadorAptitud>();
            //Logros = new List<>();
            //Otros = new List<>();
        }
        
        [Required]
        [Index("IX_UsuarioId", IsUnique = true)]
        [StringLength(128)]
        public string UsuarioId { get; set; }

        public string FotoPerfil { get; set; }

        [StringLength(512)]
        public string Nombre { get; set; }

        [StringLength(512)]
        public string Apellidos { get; set; }

        [StringLength(1024)]
        public string NombreCompleto { get; set; }

        [StringLength(50)]
        public string Celular { get; set; }

        [StringLength(512)]
        public string CorreoElectronico { get; set; }

        [StringLength(512)]
        public string IdentificacionTipo { get; set; }

        [StringLength(50)]
        public string IdentificacionNumero { get; set; }

        [StringLength(512)]
        public string Departamento { get; set; }

        [StringLength(512)]
        public string DepartamentoCodigo { get; set; }

        [StringLength(512)]
        public string Municipio { get; set; }

        [StringLength(512)]
        public string MunicipioCodigo { get; set; }
        
        [StringLength(512)]
        public string Direccion { get; set; }
 
        [StringLength(512)]
        public string Telefono { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(512)]
        public string Sexo { get; set; }

        [StringLength(512)]
        public string EstadoCivil { get; set; }

        public int? NumeroHijosDependientes { get; set; }

        [StringLength(512)]
        public string NivelEducativo { get; set; }

        public int? AspiracionSalarial { get; set; }
        
        [StringLength(512)]
        public string MonedaAspiracionSalarial { get; set; }
        
        public ICollection<TrabajadorExperienciaLaboral> ExperienciaLaboral { get; set; }

        public ICollection<TrabajadorEducacion> Educacion { get; set; }

        public ICollection<TrabajadorCualificacion> Cualificacion { get; set; }

        public ICollection<TrabajadorInteres> Interes { get; set; }

        public ICollection<TrabajadorReferencia> Referencia { get; set; }
        public ICollection<TrabajadorAptitud> Aptitud { get; set; }

        // Métodos

    }
}
