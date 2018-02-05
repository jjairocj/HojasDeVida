using System;

namespace HojasDeVida
{
    public static class Fechas
    {
        public static int ObtenerEdad(this DateTime? fechaNacimiento)
        {
            if (!fechaNacimiento.HasValue)
                return 0;

            int edad = DateTime.UtcNow.AddHours(-5).Year - fechaNacimiento.Value.Year;
            if (DateTime.UtcNow.AddHours(-5) < fechaNacimiento.Value.AddYears(edad)) edad--;

            return edad;
        }

        public static string MostrarFecha(this DateTime? fecha)
        {
            if (!fecha.HasValue)
                return string.Empty;

            return fecha.Value.ToString("dd/MM/yyyy");
        }

        public static string MostrarFechaMesAno(this DateTime? fecha)
        {
            if (!fecha.HasValue)
                return string.Empty;

            return fecha.Value.ToString("MM/yyyy");
        }

        public static string MostrarFecha(this DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        public static string MostrarFechaHora(this DateTime? fecha)
        {
            if (!fecha.HasValue)
                return string.Empty;

            return fecha.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string MostrarFechaHora(this DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}