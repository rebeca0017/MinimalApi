using Microsoft.AspNetCore.Identity;
using MinimalAPIPeliculas.Repositorios;
using System.Security.Claims;

namespace MinimalAPIPeliculas.Servicios
{
    // Esta clase es una implementación personalizada de un "almacén de usuarios" (UserStore)
    // para ASP.NET Identity, usando un repositorio personalizado llamado IRepositorioUsuarios.
    // Implementa varias interfaces que permiten manejar propiedades clave de los usuarios:
    // - Email
    // - Contraseña
    // - Claims (permisos o roles)
    // - Nombre de usuario
    public class UsuarioStore : IUserStore<IdentityUser>,
                                IUserEmailStore<IdentityUser>,
                                IUserPasswordStore<IdentityUser>,
                                IUserClaimStore<IdentityUser>
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        // Constructor que recibe el repositorio mediante inyección de dependencias.
        public UsuarioStore(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        // Agrega claims (derechos/roles) al usuario.
        public async Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            await repositorioUsuarios.AsignarClaims(user, claims);
        }

        // Crea un nuevo usuario en el sistema a través del repositorio.
        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            user.Id = await repositorioUsuarios.Crear(user); // Se asigna el ID generado.
            return IdentityResult.Success;
        }

        // Método para eliminar un usuario (aún no implementado).
        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Método de limpieza de recursos (no hace nada en este caso).
        public void Dispose()
        {
        }

        // Busca un usuario por su email (ya normalizado).
        public async Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await repositorioUsuarios.BuscarUsuarioPorEmail(normalizedEmail);
        }

        // Busca un usuario por su ID (aún no implementado).
        public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Busca un usuario por su nombre de usuario normalizado (usando el mismo método que para email).
        public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await repositorioUsuarios.BuscarUsuarioPorEmail(normalizedUserName);
        }

        // Obtiene la lista de claims asociados al usuario.
        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return await repositorioUsuarios.ObtenerClaims(user);
        }

        // Devuelve el email actual del usuario.
        public Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        // Devuelve si el email del usuario está confirmado (no implementado aún).
        public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Devuelve el email normalizado (no implementado aún).
        public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Devuelve el nombre de usuario normalizado (no implementado aún).
        public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Devuelve el hash de la contraseña del usuario.
        public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        // Devuelve el ID del usuario.
        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        // Devuelve el nombre de usuario (aquí se usa el email como nombre).
        public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        // Devuelve una lista de usuarios que tienen un claim específico (no implementado aún).
        public Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Indica si el usuario tiene una contraseña (no implementado aún).
        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Elimina claims del usuario.
        public async Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            await repositorioUsuarios.RemoverClaims(user, claims);
        }

        // Reemplaza un claim por otro (no implementado aún).
        public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Establece el email del usuario (no implementado aún).
        public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Establece si el email ha sido confirmado (no implementado aún).
        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Establece el email normalizado del usuario.
        public Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        // Establece el nombre de usuario normalizado.
        public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        // Establece el hash de la contraseña del usuario.
        public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        // Establece el nombre de usuario (no implementado aún).
        public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Actualiza el usuario (aquí simplemente retorna Success sin hacer cambios).
        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
