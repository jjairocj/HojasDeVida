using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Abp.Extensions;

namespace HojasDeVida
{
    public static class Texto
    {
        public static string RemoverAcentos(this string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            texto = texto.Normalize(NormalizationForm.FormD);
            var chars = texto.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string RemoverEmojis(this string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? texto : Regex.Replace(texto, @"\p{Cs}", "");
        }

        public static string OcultarTexto(this string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            char[] temp = new char[texto.Length];
            for (int i = 0; i < texto.Length; i++)
            {
                temp[i] = texto[i] == ' ' ? ' ' : '*';
            }

            return new string(temp).Normalize(NormalizationForm.FormC);
        }

        public static string ObtenerDisplayName<T>()
        {
            var displayName = typeof(T).GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;

            return string.Empty;
        }

        public static string Truncar(this string texto, int limiteCaracteres)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            return texto.Length <= limiteCaracteres ? texto : texto.Substring(0, limiteCaracteres) + "...";
        }

        public static string PrimeraLetraEnMayuscula(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            return texto.First().ToString().ToUpper() + texto.Substring(1).ToLower();
        }

        public static string Capitalizar(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto);
        }

        public static string FormatearNombres(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            return regex.Replace(texto.Trim().RemoverEmojis().ToLower().Capitalizar(), " ");
        }

        public static string Codificar(this string guid)
        {
            if (guid.IsNullOrWhiteSpace())
                return null;

            return Codificar(new Guid(guid));
        }

        public static string Codificar(this Guid guid)
        {
            string temp = Convert.ToBase64String(guid.ToByteArray());
            temp = temp.Replace("/", "_");
            temp = temp.Replace("+", "-");
            return temp.Substring(0, 22);
        }

        public static Guid Decodificar(this string guidcodificado)
        {
            guidcodificado = guidcodificado.Replace("_", "/");
            guidcodificado = guidcodificado.Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(guidcodificado + "==");
            return new Guid(buffer);
        }

        public static string CrearSlugUrlOfertas(string codigoOferta, string nombreOferta, string municipio)
        {
            nombreOferta = nombreOferta.Substring(0, nombreOferta.Length <= 55 ? nombreOferta.Length : 55);
            nombreOferta = Regex.Replace(nombreOferta, "-", "");
            nombreOferta = Regex.Replace(nombreOferta, "  ", " ");
            string temp = RemoverAcentos($"{nombreOferta} en {municipio}").Trim().ToLower();
            temp = Regex.Replace(temp, @"[^a-z0-9\s-]", "");
            temp = Regex.Replace(temp, @"\s+", " ").Trim();
            temp = Regex.Replace(temp, @"\s", "-");
            temp = $"{temp}-{codigoOferta}";

            return temp;
        }

        public static string Sangria(int espacios)
        {
            return "".PadLeft(espacios);
        }
    }
}
