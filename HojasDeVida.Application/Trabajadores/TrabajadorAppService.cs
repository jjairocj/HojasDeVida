using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using HojasDeVida.Autorizacion.Roles;
using HojasDeVida.Trabajadores.Dto;
using HojasDeVida.Usuarios;
using TeRecomiendo.Usuarios;

namespace HojasDeVida.Trabajadores
{
    public interface ITrabajadorAppService : IApplicationService
    {
        // Trabajador
        Task<TrabajadorDto> ObtenerTrabajador(string usuarioId);
        
        // Crear trabajador
     
     
        // Editar trabajador
        Task<RespuestaDto> EditarPerfilTrabajador(EditarPerfilTrabajadorInput input);
        Task<RespuestaDto> EditarFotoPerfilTrabajador(EditarFotoPerfilTrabajadorInput input);
        Task<RespuestaDto> EliminarFotoPerfilTrabajador(string usuarioId);
     
        // Hoja de vida 
        //Task<Tuple<MemoryStream, string>> DescargarHojaDeVidaTrabajador(string usuarioId, bool mostrarIneducacionPersonal = false);
        //void GuardarHojaDeVidaTrabajador(string directorio, string nombreArchivo, TrabajadorDto trabajador, bool mostrarIneducacionPersonal = false);

        // Experiencia laboral
        Task<List<TrabajadorExperienciaLaboralDto>> ListarExperienciaLaboral(string usuarioId);
        Task<TrabajadorExperienciaLaboralDto> ObtenerExperienciaLaboral(int experienciaId, string usuarioId = null);
        Task<RespuestaDto> CrearExperienciaLaboral(TrabajadorExperienciaLaboralInput input);
        Task<RespuestaDto> EditarExperienciaLaboral(TrabajadorExperienciaLaboralInput input);
        Task<RespuestaDto> EliminarExperienciaLaboral(int experienciaId, string usuarioId);
        
        // Educacion
        Task<List<TrabajadorEducacionDto>> ListarEducacion(string usuarioId);
        Task<TrabajadorEducacionDto> ObtenerEducacion(int educacionId, string usuarioId = null);
        Task<RespuestaDto> CrearEducacion(TrabajadorEducacionInput input);
        Task<RespuestaDto> EditarEducacion(TrabajadorEducacionInput input);
        Task<RespuestaDto> EliminarEducacion(int educacionId, string usuarioId);

        // Referencia
        Task<List<TrabajadorReferenciaDto>> ListarReferencia(string usuarioId);
        Task<TrabajadorReferenciaDto> ObtenerReferencia(int referenciaId, string usuarioId = null);
        Task<RespuestaDto> CrearReferencia(TrabajadorReferenciaInput input);
        Task<RespuestaDto> EditarReferencia(TrabajadorReferenciaInput input);
        Task<RespuestaDto> EliminarReferencia(int referenciaId, string usuarioId);
       
     }

    public class TrabajadorAppService : HojasDeVidaAppServiceBase, ITrabajadorAppService
    {
        private readonly ITrabajadorRepository _trabajadoRepository;
        private readonly IRepository<TrabajadorExperienciaLaboral> _experienciaLaboralRepository;
        private readonly IRepository<TrabajadorEducacion> _educacionRepository;
        private readonly IRepository<TrabajadorReferencia> _referenciaRepository;
        private readonly IRepository<TrabajadorAptitud> _aptitudRepository;
        private readonly IRepository<TrabajadorCualificacion> _cualificacionRepository;
        private readonly IRepository<TrabajadorInteres> _interesRepository;
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly TrabajadorManager _trabajadorManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public TrabajadorAppService(ITrabajadorRepository trabajadoRepository,
            IRepository<TrabajadorExperienciaLaboral> experienciaLaboralRepository,
            IRepository<TrabajadorEducacion> educacionRepository,
            IRepository<TrabajadorReferencia> referenciaRepository,
            IRepository<TrabajadorAptitud> aptitudRepository,
            IRepository<TrabajadorCualificacion> cualificacionRepository,
            IRepository<TrabajadorInteres> interesRepository,
            IUsuarioAppService usuarioAppService,
            TrabajadorManager trabajadorManager,
            UserManager userManager,
            RoleManager roleManager)
        {
            _trabajadoRepository = trabajadoRepository;
            _experienciaLaboralRepository = experienciaLaboralRepository;
            _educacionRepository = educacionRepository;
            _referenciaRepository = referenciaRepository;
            _aptitudRepository = aptitudRepository;
            _cualificacionRepository = cualificacionRepository;
            _interesRepository = interesRepository;
            _usuarioAppService = usuarioAppService;
            _trabajadorManager = trabajadorManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Trabajador
        public async Task<TrabajadorDto> ObtenerTrabajador(string usuarioId)
        {
            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.ExperienciaLaboral)
                .Include(x => x.Educacion)
                .Include(x => x.Interes)
                .Include(x => x.Aptitud)
                .Include(x => x.Cualificacion)
                .Include(x => x.Referencia)
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            return trabajador.MapTo<TrabajadorDto>();
        }
      
        #region Crear

        #endregion
        
        #region Editar

        public async Task<RespuestaDto> EditarPerfilTrabajador(EditarPerfilTrabajadorInput input)
        {
            var respuestaDto = new RespuestaDto();

            DateTime fechaNacimiento;
            if (!DateTime.TryParse($"{input.FechaNacimientoDia}/{input.FechaNacimientoMes}/{input.FechaNacimientoAno}", out fechaNacimiento))
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(L("ErrorRegistroTrabajadorFechaNacimiento"));
            }

            // Obtengo el trabajador y el usuario

            var trabajador = await _trabajadoRepository.GetAll()
                .Include(x => x.ExperienciaLaboral)
                .Include(x => x.Educacion)
                .Include(x => x.Interes)
                .Include(x => x.Aptitud)
                .Include(x => x.Cualificacion)
                .Include(x => x.Referencia).FirstOrDefaultAsync(x => x.UsuarioId == input.UsuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == input.UsuarioId);

            if (trabajador == null || usuario == null)
                throw new ArgumentException();

            usuario.UserName = input.IdentificacionNumero;
            usuario.Name = input.Nombre;
            usuario.Surname = input.Apellidos;

            if (usuario.IsEmailConfirmed && usuario.EmailAddress != input.CorreoElectronico)
                usuario.IsEmailConfirmed = false;

            if (usuario.IsPhoneNumberConfirmed && usuario.PhoneNumber != input.Celular)
                usuario.IsPhoneNumberConfirmed = false;

            usuario.EmailAddress = input.CorreoElectronico;
            usuario.PhoneNumber = input.Celular;

            // Válida que el número de identificación, celular y correo electrónico no estén registrados por otro trabajador

            if (await _userManager.Users.AnyAsync(x => x.UserName == usuario.UserName && x.UsuarioGuid != usuario.UsuarioGuid))
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(L("ErrorRegistroTrabajadorUserName", input.IdentificacionTipo, input.IdentificacionNumero));
            }

            if (await _userManager.Users.AnyAsync(x => x.EmailAddress == input.CorreoElectronico && x.UsuarioGuid != usuario.UsuarioGuid))
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(L("ErrorRegistroTrabajadorCorreoElectronico", input.CorreoElectronico));
            }

            if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == input.Celular && x.UsuarioGuid != usuario.UsuarioGuid))
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(L("ErrorRegistroTrabajadorCelular", input.Celular));
            }

            if (respuestaDto.Errores.Any())
                return respuestaDto;

            // Actualiza el usuario
            var identityResult = await _userManager.UpdateAsync(usuario);

            if (identityResult.Succeeded)
            {
                // Mapea los datos del trabajador
                input.MapTo(trabajador);

                trabajador.NombreCompleto = $"{trabajador.Nombre} {trabajador.Apellidos}";
                trabajador.Departamento = _configuracionAppService.ObtenerDepartamento(input.DepartamentoCodigo).Etiqueta;
                trabajador.Municipio = _configuracionAppService.ObtenerMunicipio(input.MunicipioCodigo).Etiqueta;
                trabajador.FechaNacimiento = fechaNacimiento;
                
                if (!input.DireccionCampo5.IsNullOrWhiteSpace())
                {
                    trabajador.Direccion = input.DireccionCampo5;
                }
                else
                {
                    trabajador.Direccion = $"{input.DireccionCampo1} {input.DireccionCampo2?.ToUpper()} # {input.DireccionCampo3?.ToUpper()} - {input.DireccionCampo4?.ToUpper()}";
                }

                
                await _trabajadorManager.ActualizarAsync(trabajador);
                respuestaDto.Exitoso = true;
                respuestaDto.Mensaje = L("MensajeCambiosGuardados");
            }
            else
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(L("ErrorRegistroTrabajador"));

                Logger.Error(L("ErrorRegistroTrabajador") + input);
            }

            return respuestaDto;
        }

        public async Task<RespuestaDto> EditarFotoPerfilTrabajador(EditarFotoPerfilTrabajadorInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo el trabajador y usuario

            var trabajador = await _trabajadoRepository.GetAll().FirstOrDefaultAsync(x => x.UsuarioId == input.UsuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == input.UsuarioId);

            if (trabajador == null || usuario == null)
                throw new ArgumentException();

            // Cargo la imagen

            var error = Imagenes.CargarImagen(trabajador.UsuarioId, input.Foto, ContenedoresImagenes.ImagenPerfil);
            if (!error.IsNullOrWhiteSpace())
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(error);

                return respuestaDto;
            }

            trabajador.FotoPerfil = $"{trabajador.UsuarioId}{Path.GetExtension(input.Foto.FileName)}";
            
            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EliminarFotoPerfilTrabajador(string usuarioId)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo el trabajador y usuario

            var trabajador = await _trabajadoRepository.GetAll()
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == usuarioId);

            if (trabajador == null || usuario == null)
                throw new ArgumentException();

            // Elimina la imagen

            Imagenes.EliminarImagen(trabajador.UsuarioId, ContenedoresImagenes.ImagenPerfil);
            trabajador.FotoPerfil = null;
            
            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

    
        #endregion

        //#region Hoja de vida

        //public async Task<Tuple<MemoryStream, string>> DescargarHojaDeVidaTrabajador(string usuarioId, bool mostrarIneducacionPersonal = false)
        //{
        //    // Obtengo el trabajador

        //    var trabajador = await ObtenerTrabajador(usuarioId);
        //    if (trabajador == null)
        //        throw new ArgumentException();

        //    var contenidoHojaDeVida = GenerarHojaDeVidaTrabajador(trabajador, mostrarIneducacionPersonal);

        //    var stream = Pdf.CrearPdf(contenidoHojaDeVida);
        //    var nombreArchivo = $"{usuarioId}.pdf";

        //    return new Tuple<MemoryStream, string>(stream, nombreArchivo);
        //}

        //public void GuardarHojaDeVidaTrabajador(string directorio, string nombreArchivo, TrabajadorDto trabajador, bool mostrarIneducacionPersonal = false)
        //{
        //    // Obtengo el trabajador

        //    var contenidoHojaDeVida = GenerarHojaDeVidaTrabajador(trabajador, mostrarIneducacionPersonal);
        //    if (nombreArchivo.IsNullOrWhiteSpace())
        //        nombreArchivo = trabajador.UsuarioId;

        //    nombreArchivo = $"{nombreArchivo}.pdf";

        //    Pdf.GuardarPdf(Path.Combine(directorio, nombreArchivo), contenidoHojaDeVida);
        //}

        //private string GenerarHojaDeVidaTrabajador(TrabajadorDto trabajador, bool mostrarIneducacionPersonal = false)
        //{
        //    if (!mostrarIneducacionPersonal)
        //    {
        //        trabajador.Apellidos = trabajador.Apellidos.OcultarTexto();
        //        trabajador.NombreCompleto = $"{trabajador.Nombre} {trabajador.Apellidos}";
        //        trabajador.Celular = $"{trabajador.Celular.Substring(0, 3)}*******";
        //        if (!trabajador.CorreoElectronico.IsNullOrWhiteSpace())
        //            trabajador.CorreoElectronico = $"*************{trabajador.CorreoElectronico.Substring(trabajador.CorreoElectronico.IndexOf("@", StringComparison.Ordinal))}";
        //        trabajador.IdentificacionTipo = trabajador.IdentificacionTipo;
        //        trabajador.IdentificacionNumero = trabajador.IdentificacionNumero.OcultarTexto();
        //        trabajador.Direccion = trabajador.Direccion.OcultarTexto();
        //        trabajador.DireccionCampo1 = string.Empty;
        //        trabajador.DireccionCampo2 = string.Empty;
        //        trabajador.DireccionCampo3 = string.Empty;
        //        trabajador.DireccionCampo4 = string.Empty;
        //        trabajador.DireccionCampo5 = string.Empty;
        //        trabajador.Barrio = string.Empty;
        //        trabajador.Latitud = 0;
        //        trabajador.Longitud = 0;
        //        trabajador.Telefono = trabajador.Telefono.OcultarTexto();
        //    }

        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.AppendLine("<html>");
        //    stringBuilder.AppendLine("<head>");
        //    stringBuilder.AppendLine("</head>");
        //    stringBuilder.AppendLine("<body>");
        //    //--------------------------------------------------------------------------------------------------------------
        //    stringBuilder.AppendLine($"<h2 style=\"text-align: center;\">{trabajador.NombreCompleto}</h2>");
        //    //--------------------------------------------------------------------------------------------------------------
        //    stringBuilder.AppendLine("<div id=\"fotoPerfil\" style=\"text-align: center; padding-bottom: 15px;\">");
        //    //if (!trabajador.FotoPerfil.IsNullOrWhiteSpace())
        //    //    stringBuilder.AppendLine($"<img src=\"{Imagenes.ObtenerThumbnail(trabajador.UsuarioId)}\" />");
        //    if (!trabajador.FotoPerfil.IsNullOrWhiteSpace())
        //        stringBuilder.AppendLine($"<img src=\"{trabajador.UrlFoto}\" width=\"120px\"/>");
        //    stringBuilder.AppendLine("</div>");
        //    //--------------------------------------------------------------------------------------------------------------
        //    stringBuilder.AppendLine("<div id=\"datosPersonsales\" style=\"padding-bottom: 15px;\">");
        //    stringBuilder.AppendLine("<table style=\"width: 100%\">");
        //    stringBuilder.AppendLine("<tr>");
        //    stringBuilder.AppendLine("<td style=\"width: 50%;\" valign=\"top\">");
        //    stringBuilder.AppendLine("<strong>Datos personales</strong>");
        //    stringBuilder.AppendLine("<p style=\"margin: 6px 0 6px 0;\">");
        //    stringBuilder.AppendLine($"{trabajador.IdentificacionTipo}: {trabajador.IdentificacionNumero} <br />");
        //    if (!trabajador.Sexo.IsNullOrWhiteSpace())
        //        stringBuilder.AppendLine($"Sexo: {trabajador.Sexo} <br />");
        //    if (!trabajador.EstadoCivil.IsNullOrWhiteSpace())
        //        stringBuilder.AppendLine($"Estado civil: {trabajador.EstadoCivil} <br />");
        //    if (!trabajador.NivelEducativo.IsNullOrWhiteSpace())
        //        stringBuilder.AppendLine($"Nivel educativo: {trabajador.NivelEducativo} <br />");
        //    if (!trabajador.LicenciaConduccion.IsNullOrWhiteSpace())
        //        stringBuilder.AppendLine($"Licencia de conducción: {trabajador.LicenciaConduccion} <br />");
        //    if (trabajador.FechaNacimiento != null)
        //    {
        //        stringBuilder.AppendLine($"Fecha de nacimiento: {trabajador.FechaNacimiento.Value:dd/MM/yyyy} <br />");
        //        stringBuilder.AppendLine($"Edad: {trabajador.FechaNacimiento.ObtenerEdad()}");
        //    }
        //    stringBuilder.AppendLine("</p>");
        //    stringBuilder.AppendLine("</td>");
        //    stringBuilder.AppendLine("<td style=\"width: 50%;\" valign=\"top\">");
        //    stringBuilder.AppendLine("<strong>Ineducación de contacto</strong>");
        //    stringBuilder.AppendLine("<p style=\"margin: 6px 0 6px 0;\">");
        //    if (!trabajador.Municipio.IsNullOrWhiteSpace())
        //    {
        //        stringBuilder.AppendLine($"Dirección: {trabajador.Direccion}<br />");
        //        if (mostrarIneducacionPersonal && !trabajador.Localidad.IsNullOrWhiteSpace() && trabajador.Localidad != "--- Todas las localidades ---")
        //            stringBuilder.AppendLine($"{trabajador.Localidad}<br />");

        //        stringBuilder.AppendLine($"{trabajador.Municipio} ({trabajador.Departamento})<br />");
        //    }
        //    stringBuilder.AppendLine($"Celular: {trabajador.Celular}<br />");
        //    if (!trabajador.Telefono.IsNullOrWhiteSpace() && trabajador.Celular != trabajador.Telefono)
        //        stringBuilder.AppendLine($"Teléfono: {trabajador.Telefono}<br />");

        //    if (!trabajador.CorreoElectronico.IsNullOrWhiteSpace())
        //        stringBuilder.AppendLine($"{trabajador.CorreoElectronico}<br />");

        //    stringBuilder.AppendLine("</p>");
        //    stringBuilder.AppendLine("</td>");
        //    stringBuilder.AppendLine("</tr>");
        //    stringBuilder.AppendLine("</table>");
        //    stringBuilder.AppendLine("</div>");
        //    stringBuilder.AppendLine("<hr />");
        //    //--------------------------------------------------------------------------------------------------------------
        //    stringBuilder.AppendLine("<div id=\"experienciaLaboral\" style=\"padding-top: 30px; padding-bottom: 15px;\">");
        //    stringBuilder.AppendLine("<table style=\"width: 100%\">");
        //    stringBuilder.AppendLine("<tr>");
        //    stringBuilder.AppendLine("<td style=\"width: 25%;\" valign=\"top\">");
        //    stringBuilder.AppendLine("<strong>Experiencia laboral</strong>");
        //    stringBuilder.AppendLine("</td>");
        //    stringBuilder.AppendLine("<td style=\"width: 75%;\">");
        //    foreach (var experienciaLaboral in trabajador.ExperienciaLaboral.ToList().OrderByDescending(x => x.FechaInicial))
        //    {
        //        stringBuilder.AppendLine("<p style=\"padding-bottom: 15px;\">");
        //        if (!experienciaLaboral.NombreEmpresa.IsNullOrWhiteSpace())
        //            stringBuilder.AppendLine($"<b>{experienciaLaboral.NombreEmpresa.ToUpper()}</b><br />");
        //        if (!experienciaLaboral.CargoAbierto.IsNullOrWhiteSpace())
        //            stringBuilder.AppendLine($"{experienciaLaboral.CargoAbierto}<br />");
        //        if (!experienciaLaboral.Area.IsNullOrWhiteSpace() && !experienciaLaboral.Cargo.IsNullOrWhiteSpace())
        //        {
        //            stringBuilder.AppendLine(experienciaLaboral.Cargo == "Sin especificar" ? $"Categoría: {experienciaLaboral.Area}<br />" : $"Categoría: {experienciaLaboral.Cargo}<br />");
        //        }

        //        if (experienciaLaboral.FechaInicial != null)
        //            stringBuilder.AppendLine(
        //                $"{char.ToUpper(experienciaLaboral.FechaInicial.Value.ToString("MMMM yyyy", new CultureInfo("es-CO"))[0]) + experienciaLaboral.FechaInicial.Value.ToString("MMMM yyyy", new CultureInfo("es-CO")).Substring(1)}{(experienciaLaboral.FechaFinal != null ? $" - {char.ToUpper(experienciaLaboral.FechaFinal.Value.ToString("MMMM yyyy", new CultureInfo("es-CO"))[0]) + experienciaLaboral.FechaFinal.Value.ToString("MMMM yyyy", new CultureInfo("es-CO")).Substring(1)} ({experienciaLaboral.NumeroMeses} meses)" : " - Actualmente.")}<br />");
        //        if (!experienciaLaboral.Municipio.IsNullOrWhiteSpace())
        //            stringBuilder.AppendLine($"{experienciaLaboral.Municipio} ({experienciaLaboral.Departamento})<br />");

        //        if (!experienciaLaboral.NombreJefe.IsNullOrWhiteSpace())
        //        {
        //            stringBuilder.AppendLine(mostrarIneducacionPersonal
        //                ? $"Jefe directo: {experienciaLaboral.NombreJefe}{(!experienciaLaboral.ContactoJefe.IsNullOrWhiteSpace() ? $" - {experienciaLaboral.ContactoJefe}" : string.Empty)}<br />"
        //                : $"Jefe directo: {experienciaLaboral.NombreJefe.OcultarTexto()}{(!experienciaLaboral.ContactoJefe.IsNullOrWhiteSpace() ? $" - {experienciaLaboral.ContactoJefe.OcultarTexto()}" : string.Empty)}<br />");
        //        }

        //        stringBuilder.AppendLine("<br />");
        //        if (!experienciaLaboral.Descripcion.IsNullOrWhiteSpace())
        //            stringBuilder.AppendLine($"<i>Funciones:</i><br />{experienciaLaboral.Descripcion}");
        //        stringBuilder.AppendLine("</p>");
        //    }
        //    stringBuilder.AppendLine("</td>");
        //    stringBuilder.AppendLine("</tr>");
        //    stringBuilder.AppendLine("</table>");
        //    stringBuilder.AppendLine("</div>");
        //    stringBuilder.AppendLine("<hr />");
        //    //--------------------------------------------------------------------------------------------------------------
        //    stringBuilder.AppendLine("<div id=\"educacion\" style=\"padding-top: 30px; padding-bottom: 15px;\">");
        //    stringBuilder.AppendLine("<table style=\"width: 100%\">");
        //    stringBuilder.AppendLine("<tr>");
        //    stringBuilder.AppendLine("<td style=\"width: 25%;\" valign=\"top\">");
        //    stringBuilder.AppendLine("<strong>educación</strong>");
        //    stringBuilder.AppendLine("</td>");
        //    stringBuilder.AppendLine("<td style=\"width: 75%;\">");
        //    foreach (var educacion in trabajador.educacion.ToList().OrderByDescending(x => x.FechaInicial))
        //    {
        //        stringBuilder.AppendLine("<p style=\"padding-bottom: 15px;\">");
        //        stringBuilder.AppendLine($"<b>{educacion.Nombre.ToUpper()}</b><br />");
        //        stringBuilder.AppendLine($"{educacion.Institucion}<br />");
        //        stringBuilder.AppendLine($"{educacion.Tipoeducacion}<br />");
        //        if (educacion.FechaInicial != null)
        //            stringBuilder.AppendLine(
        //                $"{char.ToUpper(educacion.FechaInicial.Value.ToString("MMMM yyyy", new CultureInfo("es-CO"))[0]) + educacion.FechaInicial.Value.ToString("MMMM yyyy", new CultureInfo("es-CO")).Substring(1)}{(educacion.FechaFinal != null ? $" - {char.ToUpper(educacion.FechaFinal.Value.ToString("MMMM yyyy", new CultureInfo("es-CO"))[0]) + educacion.FechaFinal.Value.ToString("MMMM yyyy", new CultureInfo("es-CO")).Substring(1)}" : string.Empty)}<br />");
        //        stringBuilder.AppendLine("</p>");
        //    }
        //    stringBuilder.AppendLine("</td>");
        //    stringBuilder.AppendLine("</tr>");
        //    stringBuilder.AppendLine("</table>");
        //    stringBuilder.AppendLine("</div>");
        //    //--------------------------------------------------------------------------------------------------------------
        //    if (!trabajador.Idiomas.IsNullOrWhiteSpace()) // Cuando existan otras habilidades se validaran aquí
        //    {
        //        stringBuilder.AppendLine("<hr />");
        //        stringBuilder.AppendLine("<div id=\"habilidades\" style=\"padding-top: 30px; padding-bottom: 15px;\">");
        //        stringBuilder.AppendLine("<table style=\"width: 100%\">");
        //        stringBuilder.AppendLine("<tr>");
        //        stringBuilder.AppendLine("<td style=\"width: 25%;\" valign=\"top\">");
        //        stringBuilder.AppendLine("<strong>Habilidades</strong>");
        //        stringBuilder.AppendLine("</td>");
        //        stringBuilder.AppendLine("<td style=\"width: 75%;\">");

        //        if (!trabajador.Idiomas.IsNullOrWhiteSpace())
        //        {
        //            stringBuilder.AppendLine("<p style=\"padding-bottom: 15px;\">");
        //            stringBuilder.AppendLine($"<b>{trabajador.Idiomas}</b>");
        //            stringBuilder.AppendLine("</p>");
        //        }

        //        stringBuilder.AppendLine("</td>");
        //        stringBuilder.AppendLine("</tr>");
        //        stringBuilder.AppendLine("</table>");
        //        stringBuilder.AppendLine("</div>");
        //    }
        //    //--------------------------------------------------------------------------------------------------------------
        //    stringBuilder.AppendLine("</body>");
        //    stringBuilder.AppendLine("</html>");

        //    stringBuilder.Replace("&", " ");

        //    return stringBuilder.ToString();
        //}

        //#endregion

        #endregion

        #region Experiencia laboral

        public async Task<List<TrabajadorExperienciaLaboralDto>> ListarExperienciaLaboral(string usuarioId)
        {
            // Obtengo el trabajador

            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.ExperienciaLaboral).FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            if (trabajador == null)
                throw new ArgumentException();

            trabajador.ExperienciaLaboral = trabajador.ExperienciaLaboral.OrderByDescending(x => x.FechaInicial).ToList();

            return trabajador.ExperienciaLaboral.MapTo<List<TrabajadorExperienciaLaboralDto>>();
        }

        public async Task<TrabajadorExperienciaLaboralDto> ObtenerExperienciaLaboral(int experienciaId, string usuarioId = null)
        {
            // Obtengo la experiencia

            var experiencia = usuarioId.IsNullOrWhiteSpace()
                ? await _experienciaLaboralRepository.FirstOrDefaultAsync(x => x.Id == experienciaId)
                : await _experienciaLaboralRepository.FirstOrDefaultAsync(x => x.Id == experienciaId && x.UsuarioId == usuarioId);

            if (experiencia == null)
                throw new ArgumentException();

            return experiencia.MapTo<TrabajadorExperienciaLaboralDto>();
        }

        public async Task<RespuestaDto> CrearExperienciaLaboral(TrabajadorExperienciaLaboralInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo el trabajador y el usuario

            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.ExperienciaLaboral).FirstOrDefaultAsync(x => x.UsuarioId == input.UsuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == input.UsuarioId);

            if (trabajador == null || usuario == null)
                throw new ArgumentException();

            // Mapea los datos de la experiencia

            var experienciaLaboral = new TrabajadorExperienciaLaboral();
            input.MapTo(experienciaLaboral);

            experienciaLaboral.Departamento = input.DepartamentoCodigo == "5000" ? "En el exterior" : _configuracionAppService.ObtenerDepartamento(input.DepartamentoCodigo).Etiqueta;
            experienciaLaboral.Municipio = input.DepartamentoCodigo == "5000"
                ? _configuracionAppService.ObtenerPais(input.MunicipioCodigo).Etiqueta
                : _configuracionAppService.ObtenerMunicipio(input.MunicipioCodigo).Etiqueta;
            
            trabajador.ExperienciaLaboral.Add(experienciaLaboral);

            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EditarExperienciaLaboral(TrabajadorExperienciaLaboralInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo la experiencia laboral

            var experiencia = await _experienciaLaboralRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.UsuarioId == input.UsuarioId);

            if (experiencia == null)
                throw new ArgumentException();

            // Mapea los datos de la experiencia

            input.MapTo(experiencia);

            experiencia.Departamento = input.DepartamentoCodigo == "5000" ? "En el exterior" : _configuracionAppService.ObtenerDepartamento(input.DepartamentoCodigo).Etiqueta;
            experiencia.Municipio = input.DepartamentoCodigo == "5000" ? _configuracionAppService.ObtenerPais(input.MunicipioCodigo).Etiqueta : _configuracionAppService.ObtenerMunicipio(input.MunicipioCodigo).Etiqueta;

            // Actualiza la experiencia

            await _experienciaLaboralRepository.UpdateAsync(experiencia);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EliminarExperienciaLaboral(int experienciaId, string usuarioId)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo la experiencia laboral, el trabajador y el usuario

            var experiencia = await _experienciaLaboralRepository.FirstOrDefaultAsync(x => x.Id == experienciaId && x.UsuarioId == usuarioId);
            var trabajador = await _trabajadoRepository.GetAll().FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == usuarioId);

            if (experiencia == null || trabajador == null || usuario == null)
                throw new ArgumentException();

            // Elimina la experiencia

            await _experienciaLaboralRepository.DeleteAsync(experienciaId);
            
            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);

            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeRegistroEliminado");

            return respuestaDto;
        }
        
        #endregion

        #region educación

        public async Task<List<TrabajadorEducacionDto>> ListarEducacion(string usuarioId)
        {
            // Obtengo el trabajador

            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.Educacion).FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            if (trabajador == null)
                throw new ArgumentException();

            trabajador.Educacion = trabajador.Educacion.OrderByDescending(x => x.FechaInicial).ToList();

            return trabajador.Educacion.MapTo<List<TrabajadorEducacionDto>>();
        }

        public async Task<TrabajadorEducacionDto> ObtenerEducacion(int educacionId, string usuarioId = null)
        {
            // Obtengo la educación

            var educacion = usuarioId.IsNullOrWhiteSpace()
                ? await _educacionRepository.FirstOrDefaultAsync(x => x.Id == educacionId)
                : await _educacionRepository.FirstOrDefaultAsync(x => x.Id == educacionId  && x.UsuarioId == usuarioId);

            if (educacion == null)
                throw new ArgumentException();

            return educacion.MapTo<TrabajadorEducacionDto>();
        }

        public async Task<RespuestaDto> CrearEducacion(TrabajadorEducacionInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo el trabajador y el usuario

            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.ExperienciaLaboral).Include(x => x.Educacion).FirstOrDefaultAsync(x => x.UsuarioId == input.UsuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == input.UsuarioId);

            if (trabajador == null || usuario == null)
                throw new ArgumentException();

            // Mapea los datos de la educación

            var educacion = new TrabajadorEducacion();
            input.MapTo(educacion);

            trabajador.Educacion.Add(educacion);
            
            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EditarEducacion(TrabajadorEducacionInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo la educación

            var educacion = await _educacionRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.UsuarioId == input.UsuarioId);

            if (educacion == null)
                throw new ArgumentException();

            // Mapea los datos de la educación

            input.MapTo(educacion);

            // Actualiza la educación

            await _educacionRepository.UpdateAsync(educacion);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EliminarEducacion(int educacionId, string usuarioId)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo la educación, el trabajador y el usuario

            var educacion = await _educacionRepository.FirstOrDefaultAsync(x => x.Id == educacionId && x.UsuarioId == usuarioId);
            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.ExperienciaLaboral).Include(x => x.Educacion).FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.UsuarioGuid == usuarioId);

            if (educacion == null || trabajador == null || usuario == null)
                throw new ArgumentException();

            // Elimina la educacion

            await _educacionRepository.DeleteAsync(educacionId);
            
            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);

            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeRegistroEliminado");

            return respuestaDto;
        }

        #endregion

        #region Referencia

        public async Task<List<TrabajadorReferenciaDto>> ListarReferencia(string usuarioId)
        {
            // Obtengo el trabajador

            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.Referencia).FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            if (trabajador == null)
                throw new ArgumentException();

            trabajador.Referencia = trabajador.Referencia.OrderBy(x => x.Descripcion).ToList();

            return trabajador.Referencia.MapTo<List<TrabajadorReferenciaDto>>();
        }

        public async Task<TrabajadorReferenciaDto> ObtenerReferencia(int referenciaId, string usuarioId = null)
        {
            // Obtengo la referencia

            var referencia = usuarioId.IsNullOrWhiteSpace()
                ? await _referenciaRepository.FirstOrDefaultAsync(x => x.Id == referenciaId)
                : await _referenciaRepository.FirstOrDefaultAsync(x => x.Id == referenciaId && x.UsuarioId == usuarioId);

            if (referencia == null)
                throw new ArgumentException();

            return referencia.MapTo<TrabajadorReferenciaDto>();
        }

        public async Task<RespuestaDto> CrearReferencia(TrabajadorReferenciaInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo el trabajador y el usuario

            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.Referencia).FirstOrDefaultAsync(x => x.UsuarioId == input.UsuarioId);

            if (trabajador == null)
                throw new ArgumentException();

            // Mapea los datos de la referencia

            var referencia = new TrabajadorReferencia();
            input.MapTo(referencia);

            trabajador.Referencia.Add(referencia);

            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);
            CurrentUnitOfWork.SaveChanges();

            await _trabajadorManager.ActualizarAsync(trabajador);

            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EditarReferencia(TrabajadorReferenciaInput input)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo la referencia

            var referencia = await _referenciaRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.UsuarioId == input.UsuarioId);

            if (referencia == null)
                throw new ArgumentException();

            // Mapea los datos de la referencia

            input.MapTo(referencia);

            // Actualiza la referencia

            await _referenciaRepository.UpdateAsync(referencia);
            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeCambiosGuardados");

            return respuestaDto;
        }

        public async Task<RespuestaDto> EliminarReferencia(int referenciaId, string usuarioId)
        {
            var respuestaDto = new RespuestaDto();

            // Obtengo la referencia y el trabajador 

            var referencia = await _referenciaRepository.FirstOrDefaultAsync(x => x.Id == referenciaId && x.UsuarioId == usuarioId);
            var trabajador = await _trabajadoRepository.GetAll().Include(x => x.Referencia).FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            if (referencia == null || trabajador == null)
                throw new ArgumentException();

            // Elimina la referencia

            await _referenciaRepository.DeleteAsync(referenciaId);

            // Actualiza el trabajador

            await _trabajadorManager.ActualizarAsync(trabajador);

            respuestaDto.Exitoso = true;
            respuestaDto.Mensaje = L("MensajeRegistroEliminado");

            return respuestaDto;
        }
   
        #endregion
        
    }
}
