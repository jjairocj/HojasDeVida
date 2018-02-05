using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using HojasDeVida.Autorizacion.Roles;
using HojasDeVida.Usuarios.Dto;
using System;
using Microsoft.AspNet.Identity;
using HojasDeVida.Trabajadores;
using HojasDeVida.Trabajadores.Dto;
using HojasDeVida;
using HojasDeVida.Usuarios;

namespace TeRecomiendo.Usuarios
{
    public enum ListaPerfilesEstaticos
    {
        Trabajador,
        FullAdmin
    }

    public enum MedioContacto
    {
        CorreoElectronico
    }

    public interface IUsuarioAppService : IApplicationService
    {
        // Usuarios
        Task<PagedResultDto<UsuarioDto>> ListarUsuarios(ListarInput input);
        Task<UsuarioDto> ObtenerUsuario(string usuarioId);
        Task<long> ObtenerUsuarioRegistroId(string usuarioId);
        
        // Contraseñas
        Task<RespuestaDto> NuevaContrasena(NuevaContrasenaInput input);
        Task<RespuestaDto> EditarContrasena(EditarContrasenaInput input);
        Task<RespuestaDto> RestaurarContrasena(RestaurarContrasenaInput input);

        // Perfiles
        Task<List<PerfilDto>> ListarPerfiles();
        Task<PerfilDto> ObtenerPerfil(int id);
        Task<PerfilDto> ObtenerPerfilPorNombre(string nombre);
        Task CrearPerfil(CrearPerfilInput input);
        Task ActualizarPerfil(ActualizarPerfilInput input);
        Task EliminarPerfil(int id);

        // Permisos
        List<PermisoDto> ListarPermisos();
        Task<List<PermisoDto>> ListarPermisosPorPerfil(int id);
        PermisoDto ObtenerPermisoPorNombre(string nombre);
        Task ActualizarPermisos(PerfilDto inputPerfil, List<PermisoDto> inputPermisos);

        //Mensajes
       }

    public class UsuarioAppService : HojasDeVidaAppServiceBase, IUsuarioAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly ITrabajadorRepository _trabajadoRepository;
        
        public UsuarioAppService(UserManager userManager, RoleManager roleManager, IPermissionManager permissionManager, IRepository<User, long> userRepository, ITrabajadorRepository trabajadoRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _userRepository = userRepository;
            _trabajadoRepository = trabajadoRepository;
        }

        #region Usuarios

        public async Task<PagedResultDto<UsuarioDto>> ListarUsuarios(ListarInput input)
        {
            var total =
                await
                    _userRepository.GetAll()
                        .WhereIf(input.FiltroFechaInicioSesionInicial != null, x => x.LastLoginTime >= input.FiltroFechaInicioSesionInicial.Value)
                        .WhereIf(input.FiltroFechaInicioSesionFinal != null, x => x.LastLoginTime <= input.FiltroFechaInicioSesionFinal)
                        .WhereIf(input.FiltroFechaRegistroInicial != null, x => x.CreationTime >= input.FiltroFechaRegistroInicial.Value)
                        .WhereIf(input.FiltroFechaRegistroFinal != null, x => x.CreationTime <= input.FiltroFechaRegistroFinal)
                        .WhereIf(input.FiltroFechaActualizacionInicial != null, x => x.CreationTime >= input.FiltroFechaActualizacionInicial)
                        .WhereIf(input.FiltroFechaActualizacionFinal != null, x => x.CreationTime <= input.FiltroFechaActualizacionFinal)
                        .WhereIf(input.FiltroEsActivo != null, x => x.IsActive == input.FiltroEsActivo)
                        .WhereIf(!input.FiltroTipo.IsNullOrWhiteSpace(), x => x.Tipo == input.FiltroTipo)
                        .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filtro) || x.Surname.Contains(input.Filtro) || x.EmailAddress.Contains(input.Filtro) || x.PhoneNumber.Contains(input.Filtro))
                        .CountAsync();

            var lista =
                await
                    _userRepository.GetAll().Include(x => x.Roles)
                        .WhereIf(input.FiltroFechaInicioSesionInicial != null, x => x.LastLoginTime >= input.FiltroFechaInicioSesionInicial.Value)
                        .WhereIf(input.FiltroFechaInicioSesionFinal != null, x => x.LastLoginTime <= input.FiltroFechaInicioSesionFinal)
                        .WhereIf(input.FiltroFechaRegistroInicial != null, x => x.CreationTime >= input.FiltroFechaRegistroInicial.Value)
                        .WhereIf(input.FiltroFechaRegistroFinal != null, x => x.CreationTime <= input.FiltroFechaRegistroFinal)
                        .WhereIf(input.FiltroFechaActualizacionInicial != null, x => x.CreationTime >= input.FiltroFechaActualizacionInicial)
                        .WhereIf(input.FiltroFechaActualizacionFinal != null, x => x.CreationTime <= input.FiltroFechaActualizacionFinal)
                        .WhereIf(input.FiltroEsActivo != null, x => x.IsActive == input.FiltroEsActivo)
                        .WhereIf(!input.FiltroTipo.IsNullOrWhiteSpace(), x => x.Tipo == input.FiltroTipo)
                        .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filtro) || x.Surname.Contains(input.Filtro) || x.EmailAddress.Contains(input.Filtro) || x.PhoneNumber.Contains(input.Filtro))
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

            var resultado = new PagedResultDto<UsuarioDto>
            {
                TotalCount = total,
                Items = lista.MapTo<List<UsuarioDto>>()
            };

            var listaPerfiles = await _roleManager.Roles.ToListAsync();

            foreach (var item in resultado.Items)
            {
                var temp = new List<string>();
                foreach (var roleId in item.Perfil.Split(','))
                {
                    int id;
                    if (int.TryParse(roleId, out id))
                    {
                        var role = listaPerfiles.FirstOrDefault(x => x.Id == id);
                        if (role != null)
                        {
                            temp.Add(role.DisplayName);
                        }
                    }
                }

                item.Perfil = string.Join(",", temp);
            }

            return resultado;
        }

        public async Task<UsuarioDto> ObtenerUsuario(string usuarioId)
        {
            var entidad = await _userRepository.FirstOrDefaultAsync(x => x.UsuarioGuid == usuarioId);
            return entidad.MapTo<UsuarioDto>();
        }

        public async Task<long> ObtenerUsuarioRegistroId(string usuarioId)
        {
            var usuario = await _userRepository.FirstOrDefaultAsync(x => x.UsuarioGuid == usuarioId);
            return usuario.Id;
        }
        
        #endregion

        #region Contraseñas

        public async Task<RespuestaDto> NuevaContrasena(NuevaContrasenaInput input)
        {
            RespuestaDto respuestaDto = new RespuestaDto();

            // Obtengo el usuario

            var usuario = await _userRepository.FirstOrDefaultAsync(x => x.UsuarioGuid == input.UsuarioId);

            if (usuario == null)
                throw new ArgumentException();

            // Actualiza la contraseña del usuario
            var identityResult = await _userManager.ChangePasswordAsync(usuario.Id, input.Contrasena, input.NuevaContrasena);

            if (identityResult.Succeeded)
            {
                respuestaDto.Exitoso = true;
                respuestaDto.Mensaje = L("MensajeCambiosGuardados");
            }
            else
            {
                respuestaDto.Exitoso = false;
                if (identityResult.Errors.Any() && identityResult.Errors.Contains("Incorrect password."))
                {
                    respuestaDto.Errores.Add(L("ErrorContrasenaIncorrecta"));
                }
                else
                {
                    respuestaDto.Errores.Add(L("Error"));
                }
                Logger.Error("Error cambio de contraseña" + input);
            }

            return respuestaDto;
        }

        public async Task<RespuestaDto> EditarContrasena(EditarContrasenaInput input)
        {
            RespuestaDto respuestaDto = new RespuestaDto();

            // Obtengo el usuario

            var usuario = await _userRepository.FirstOrDefaultAsync(x => x.UsuarioGuid == input.UsuarioId);

            if (usuario == null)
                throw new ArgumentException();

            // Actualiza la contraseña del usuario
            var identityResult = await _userManager.ChangePasswordAsync(usuario, input.Contrasena);

            if (identityResult.Succeeded)
            {
                respuestaDto.Exitoso = true;
                respuestaDto.Mensaje = L("MensajeCambiosGuardados");
            }
            else
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Errores.Add(L("Error"));

                Logger.Error("Error cambio de contraseña" + input);
            }

            return respuestaDto;
        }

        public async Task<RespuestaDto> RestaurarContrasena(RestaurarContrasenaInput input)
        {
            RespuestaDto respuestaDto = new RespuestaDto();

            // Obtengo el usuario

            var usuario = await _userRepository.FirstOrDefaultAsync(x => x.UsuarioGuid == input.Token);

            if (usuario == null)
                throw new ArgumentException();

            // Valida el código

            bool resultado;
            if (input.MedioSeleccionado == MedioContacto.CorreoElectronico)
            {
                resultado = await UserManager.VerifyChangePhoneNumberTokenAsync(usuario.Id, input.Codigo.ToString(), usuario.EmailAddress);
            }
            else
            {
                resultado = await UserManager.VerifyChangePhoneNumberTokenAsync(usuario.Id, input.Codigo.ToString(), usuario.PhoneNumber);
            }

            if (resultado)
            {
                // Cambia la contraseña

                await UserManager.RemovePasswordAsync(usuario.Id);
                await UserManager.AddPasswordAsync(usuario.Id, input.NuevaContrasena);
                await UserManager.UpdateAsync(usuario);

                respuestaDto.Exitoso = true;
                respuestaDto.Mensaje = L("MensajeContrasenaCambiada");
            }
            else
            {
                respuestaDto.Exitoso = false;
                respuestaDto.Mensaje = L("ErrorCodigoVerificacionNoValido");
            }

            return respuestaDto;
        }

        #endregion

        #region Perfiles

        public async Task<List<PerfilDto>> ListarPerfiles()
        {
            var lista = await
                _roleManager.Roles.OrderBy(x => x.DisplayName)
                    .ToListAsync();

            return lista.Select(item => new PerfilDto { Id = item.Id, Nombre = item.DisplayName }).ToList();
        }

        public async Task<PerfilDto> ObtenerPerfil(int id)
        {
            var entidad = await _roleManager.GetRoleByIdAsync(id);
            return entidad.MapTo<PerfilDto>();
        }

        public async Task<PerfilDto> ObtenerPerfilPorNombre(string nombre)
        {
            nombre = nombre.Replace(" ", "");
            var entidad = await _roleManager.GetRoleByNameAsync(nombre);

            PerfilDto perfilDto = new PerfilDto
            {
                Id = entidad.Id,
                Nombre = entidad.DisplayName
            };

            return perfilDto;
        }

        public async Task CrearPerfil(CrearPerfilInput input)
        {
            var nombre = Regex.Replace(input.Nombre, @"\s+", "");

            // Válida que no exista otro perfil igual

            var band = await _roleManager.RoleExistsAsync(nombre);
            if (band)
            {
                throw new UserFriendlyException($"Ya existe el perfil {input.Nombre}.");
            }

            Role role = new Role
            {
                Name = nombre,
                DisplayName = input.Nombre,
                IsStatic = false,
                IsDefault = false
            };

            await _roleManager.CreateAsync(role);
        }

        public async Task ActualizarPerfil(ActualizarPerfilInput input)
        {
            var nombre = Regex.Replace(input.Nombre, @"\s+", "");

            // Válida que no exista otro perfil igual

            var cantidad = await _roleManager.Roles.CountAsync(x => x.Id != input.Id && x.Name == nombre);
            if (cantidad > 0)
            {
                throw new UserFriendlyException($"Ya existe el perfil {input.Nombre}.");
            }

            Role role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == input.Id);
            role.Name = nombre;
            role.DisplayName = input.Nombre;

            await _roleManager.UpdateAsync(role);
        }

        public async Task EliminarPerfil(int id)
        {
            Role role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            await _roleManager.DeleteAsync(role);
        }

        #endregion

        #region Permisos

        public List<PermisoDto> ListarPermisos()
        {
            var lista = _permissionManager.GetAllPermissions(false);
            return lista.Select(item => new PermisoDto { Nombre = item.Name, Etiqueta = item.DisplayName.ToString() }).OrderBy(x => x.Nombre).ToList();
        }

        public async Task<List<PermisoDto>> ListarPermisosPorPerfil(int id)
        {
            var permisos = await _roleManager.GetGrantedPermissionsAsync(id);

            var lista =
                permisos.Select(item => new PermisoDto { Nombre = item.Name, Etiqueta = item.DisplayName.ToString() })
                    .OrderBy(x => x.Nombre)
                    .ToList();

            return lista;
        }

        public PermisoDto ObtenerPermisoPorNombre(string nombre)
        {
            var permiso = _permissionManager.GetPermission(nombre);
            return new PermisoDto { Nombre = permiso.Name, Etiqueta = permiso.DisplayName.ToString() };
        }

        public async Task ActualizarPermisos(PerfilDto inputPerfil, List<PermisoDto> inputPermisos)
        {
            var perfil = await _roleManager.GetRoleByIdAsync(inputPerfil.Id);
            List<Permission> permisos = new List<Permission>();

            foreach (PermisoDto item in inputPermisos)
            {
                permisos.Add(_permissionManager.GetPermission(item.Nombre));
            }

            await _roleManager.ResetAllPermissionsAsync(perfil);
            await _roleManager.SetGrantedPermissionsAsync(perfil, permisos);
        }

        #endregion

   }
}
