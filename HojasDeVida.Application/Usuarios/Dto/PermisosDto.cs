using System.ComponentModel.DataAnnotations;

namespace HojasDeVida.Usuarios.Dto
{
    public class PermisoDto
    {
        [Display(Name = "Nombre del permiso")]
        public string Nombre { get; set; }

        [Display(Name = "Etiqueta del permiso")]
        public string Etiqueta { get; set; }
    }
}
