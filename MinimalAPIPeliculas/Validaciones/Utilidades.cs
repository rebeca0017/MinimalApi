﻿namespace MinimalAPIPeliculas.Validaciones
{
    public static class Utilidades
    {
        public static string CampoRequeridoMensaje = "El campo {PropertyName} es requerido";
        public static string MaximumLengthMensaje = "El campo {PropertyName} debe tener menos de {MaxLength} caracteres";
        public static string PrimeraLetraMayusculaMensaje = "El canpo {PropertyName} debe comenzar con mayusculas";
        public static string EmailMensaje = "El campo {PropertyName} debe ser un email válido";
        public static string GreaterThanEqualToMensaje(DateTime fechaMinima)
        {
            return "el campo {PropertyName} debe ser posterior a " + fechaMinima.ToString("yyyy-MM-dd");
        }

        public static bool PrimeraLetraMayusculas(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return true;
            }
            var primeraLetra = valor[0].ToString();
            return primeraLetra == primeraLetra.ToUpper();
        }
    }
}
