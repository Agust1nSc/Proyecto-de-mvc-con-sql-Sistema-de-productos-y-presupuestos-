using MVC.Interfaces;
using MVC.Models;
using Microsoft.AspNetCore.Http;

namespace MVC.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Login(string username, string password)
        {
            var context = _httpContextAccessor.HttpContext;
            var user = _userRepository.GetUser(username, password);

            if (user != null)
            {
                if (context == null)
                {
                    throw new InvalidOperationException("HttpContext no está disponible.");
                }

                
                context.Session.SetString("IsAuthenticated", "true");
                context.Session.SetString("User", user.User);
                context.Session.SetString("UserNombre", user.Nombre);
                context.Session.SetString("Rol", user.Rol);

                return true;
            }

            return false;
        }

        public void Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }

            context.Session.Clear(); // Borra todas las variables de sesión
        }

        public bool IsAuthenticated()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }

            // Verifica si la variable "IsAuthenticated" es "true"
            return context.Session.GetString("IsAuthenticated") == "true";
        }

        public bool HasAccessLevel(string requiredAccessLevel)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }

            // Compara el Rol guardado con el requerido
            return context.Session.GetString("Rol") == requiredAccessLevel;
        }
    }
}