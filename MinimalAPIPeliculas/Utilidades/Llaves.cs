using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIPeliculas.Utilidades
{
    public static class Llaves
    {
        // Constante que define el nombre del emisor por defecto de los tokens JWT.
        public const string IssuerPropio = "nuestra-app";

        // Ruta dentro del archivo de configuración (appsettings.json) donde están las llaves para firmar tokens.
        private const string SeccionLlaves = "Authentication:Schemes:Bearer:SigningKeys";

        // Nombre de la propiedad que contiene el emisor dentro de cada llave de firma.
        private const string SeccionLlaves_Emisor = "Issuer";

        // Nombre de la propiedad que contiene el valor base64 de la llave.
        private const string SeccionLlaves_Valor = "Value";


        // Método público para obtener la(s) llave(s) de firma para un emisor específico (por defecto "nuestra-app").
        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration)
            => ObtenerLlave(configuration, IssuerPropio);


        // Método que busca dentro de la configuración la llave asociada a un emisor (issuer) específico.
        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration, string issuer)
        {
            // Busca una sola llave de firma cuya propiedad "Issuer" coincida con el valor recibido.
            var signingKey = configuration.GetSection(SeccionLlaves)
                .GetChildren()
                .SingleOrDefault(key => key[SeccionLlaves_Emisor] == issuer);

            // Si encuentra una clave válida (que tenga valor), la convierte desde base64 y la retorna como llave simétrica.
            if (signingKey is not null && signingKey[SeccionLlaves_Valor] is string keyValue)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(keyValue));
            }
        }


        // Método que obtiene todas las llaves de firma definidas en la configuración, sin importar el emisor.
        public static IEnumerable<SecurityKey> ObtenerTodasLasLlave(IConfiguration configuration)
        {
            // Lee todas las llaves bajo la sección Authentication:Schemes:Bearer:SigningKeys
            var signingKeys = configuration.GetSection(SeccionLlaves)
                .GetChildren();

            // Recorre cada una, y si tiene un valor, la convierte desde base64 y la retorna como llave simétrica.
            foreach (var signingKey in signingKeys)
            {
                if (signingKey[SeccionLlaves_Valor] is string keyValue)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(keyValue));
                }
            }
        }

    }
}
