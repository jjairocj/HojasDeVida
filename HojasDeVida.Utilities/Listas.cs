using System;
using System.Collections.Generic;

namespace HojasDeVida
{
    public static class Listas
    {
        private static readonly Random Random = new Random();

        public static void OrdenarAleatoriamente<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string ObtenerParametrosSql(this List<string> lista)
        {
            List<string> temp = new List<string>(lista.Count);
            foreach (var item in lista)
            {
                temp.Add($"'{item}'");
            }

            return string.Join(", ", temp.ToArray());
        }
    }
}
