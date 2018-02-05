using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HojasDeVida.Trabajadores.Dto
{
    [AutoMapFrom(typeof(Trabajador))]
    public class TrabajadorDto : EntityDto<Guid>
    {
        // Datos usuario trabajador
        public string UsuarioId { get; set; }
        public string FotoPerfil { get; set; }
        public string UrlFoto { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string IdentificacionTipo { get; set; }
        public string IdentificacionNumero { get; set; }
        public string Identificacion { get; set; }
        // Datos ubicación
        public string Departamento { get; set; }
        public string DepartamentoCodigo { get; set; }
        public string Municipio { get; set; }
        public string MunicipioCodigo { get; set; }
        public string Direccion { get; set; }
   
        // Datos complementarios
        public DateTime? FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public int? NumeroHijosDependientes { get; set; }
        public string NivelEducativo { get; set; }
        public string AspiracionSalarial { get; set; }
        public string MonedaAspiracionSalarial { get; set; }

        // Datos generales
        public DateTime Registro { get; set; }
        public DateTime? Actualizacion { get; set; }
        // Experiencia y formación
        public IEnumerable<TrabajadorExperienciaLaboralDto> ExperienciaLaboral { get; set; }
        public IEnumerable<TrabajadorEducacionDto> Formacion { get; set; }
        public IEnumerable<TrabajadorCualificacionDto> Cualificacion { get; set; }
        public IEnumerable<TrabajadorInteresDto> Interes { get; set; }
        public IEnumerable<TrabajadorReferenciaDto> Referencia { get; set; }

        public string ObtenerPrimerNombre()
        {
            if (Nombre.IsNullOrWhiteSpace())
                return string.Empty;

            var nombre = Nombre.Split(' ')[0];
            nombre = Texto.PrimeraLetraEnMayuscula(nombre);

            return nombre;
        }
    }
    
    [AutoMapTo(typeof(Trabajador))]
    public class CrearTrabajadorInput : IShouldNormalize, ICustomValidate
    {
        [StringLength(128)]
        public string UsuarioId { get; set; }

        // Datos usuario 

        [Required]
        [StringLength(256)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Tipo de identificación")]
        public string IdentificacionTipo { get; set; }

        [Required]
        [RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "Introduce un número de identificación válido.")]
        [StringLength(50)]
        [Display(Name = "Número de identificación")]
        public string IdentificacionNumero { get; set; }

        [Required]
        [RegularExpression(@"^3[0-9]{9}$", ErrorMessage = "Introduce un número de celular válido.")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [StringLength(512)]
        [Display(Name = "Otro teléfono de contacto")]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Introduce una dirección válida de correo electrónico.")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = "Introduce una dirección válida de correo electrónico")]
        [StringLength(512)]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Crear contraseña")]
        //public string Contrasena { get; set; }

        public bool AceptaCondiciones { get; set; }

        public string ReturnUrl { get; set; }

        public void Normalize()
        {
            Nombre = Nombre?.FormatearNombres();
            Apellidos = Apellidos?.FormatearNombres();
            IdentificacionNumero = IdentificacionNumero?.Trim();
        }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (AceptaCondiciones == false)
            {
                context.Results.Add(new ValidationResult("Debes aceptar nuestros términos y condiciones y política de privacidad para poder continuar el proceso de registro."));
            }
        }
    }
    
    [AutoMapTo(typeof(Trabajador))]
    public class EditarPerfilTrabajadorInput : IShouldNormalize
    {
        [Required]
        [StringLength(128)]
        public string UsuarioId { get; set; }

        // Datos usuario 

        [Required]
        [StringLength(256)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Tipo de identificación")]
        public string IdentificacionTipo { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Introduce un número de identificación válido.")]
        [StringLength(50)]
        [Display(Name = "Número de identificación")]
        public string IdentificacionNumero { get; set; }

        [Required]
        [RegularExpression(@"^3[0-9]{9}$", ErrorMessage = "Introduce un número de celular válido.")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [StringLength(512)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Introduce una dirección válida de correo electrónico.")]
        [StringLength(512)]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        // Datos ubicación

        [Required]
        [StringLength(512)]
        [Display(Name = "Departamento")]
        public string DepartamentoCodigo { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Municipio")]
        public string MunicipioCodigo { get; set; }

        [StringLength(512)]
        [Display(Name = "Dirección - Campo 1")]
        public string DireccionCampo1 { get; set; }

        [StringLength(10)]
        [Display(Name = "Dirección - Campo 2")]
        public string DireccionCampo2 { get; set; }

        [StringLength(10)]
        [Display(Name = "Dirección - Campo 3")]
        public string DireccionCampo3 { get; set; }

        [StringLength(10)]
        [Display(Name = "Dirección - Campo 4")]
        public string DireccionCampo4 { get; set; }

        [StringLength(50)]
        [Display(Name = "Dirección - Campo 5")]
        public string DireccionCampo5 { get; set; }

        [StringLength(512)]
        [Display(Name = "Localidad")]
        public string Localidad { get; set; }

        [Display(Name = "Estrato")]
        public int Estrato { get; set; }

        // Áreas de interes

        [Display(Name = "Áreas de interes")]
        public List<string> AreasDeInteres { get; set; }

        // Datos complementarios

        [Required]
        [Display(Name = "Día")]
        public int FechaNacimientoDia { get; set; }

        [Required]
        [Display(Name = "Mes")]
        public int FechaNacimientoMes { get; set; }

        [Required]
        [Display(Name = "Año")]
        public int FechaNacimientoAno { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        [StringLength(512)]
        [Display(Name = "Estado civil")]
        public string EstadoCivil { get; set; }

        [Display(Name = "Número de hijos o dependientes")]
        public int? NumeroHijosDependientes { get; set; }

        [StringLength(512)]
        [Display(Name = "Nivel educativo")]
        public string NivelEducativo { get; set; }

        [StringLength(512)]
        [Display(Name = "Aspiración salarial")]
        public string AspiracionSalarial { get; set; }

        [StringLength(512)]
        [Display(Name = "Horario disponible")]
        public string HorarioDisponible { get; set; }

        [StringLength(512)]
        [Display(Name = "Situación laboral actual")]
        public string SituacionLaboralActual { get; set; }

        [StringLength(512)]
        [Display(Name = "Licencia de conducción")]
        public string LicenciaConduccion { get; set; }

        [Display(Name = "Vehículo propio")]
        public List<string> VehiculoPropio { get; set; }

        [StringLength(512)]
        [Display(Name = "Idiomas")]
        public string Idiomas { get; set; }

        [StringLength(512)]
        [Display(Name = "Discapacidades")]
        public string Discapacidades { get; set; }

        [StringLength(512)]
        [Display(Name = "Cómo se enteró de TeRecomiendo.com")]
        public string ComoSeEnteroTeRecomiendo { get; set; }

        // Notificaciones

        [Display(Name = "No recibir ofertas vía SMS")]
        public bool NoRecibirOfertasSms { get; set; }

        [Display(Name = "No recibir ofertas vía correo electrónico")]
        public bool NoRecibirOfertasEmail { get; set; }

        public void Normalize()
        {
            Nombre = Nombre?.FormatearNombres();
            Apellidos = Apellidos?.FormatearNombres();
            IdentificacionNumero = IdentificacionNumero?.Trim();
        }
    }

    [AutoMapTo(typeof(Trabajador))]
    public class EditarFotoPerfilTrabajadorInput
    {
        [Required]
        [StringLength(128)]
        public string UsuarioId { get; set; }

        [Required]
        [Display(Name = "Foto")]
        public HttpPostedFileBase Foto { get; set; }
    }
}