using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.ViewModels;
using MvcTaller.Models;
public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IAuthenticationService authenticationService, ILogger<LoginController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }


    // [HttpGet] Muestra la vista de login
    public IActionResult Index()
    {
        // ... (Crear LoginViewModel)
        return View(new LoginViewModel());
    }
    // [HttpPost] Procesa el login
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Debe ingresar usuario y contraseña.";
                return View("Index", model);
            }
            if (_authenticationService.Login(model.Username, model.Password))
            {
                _logger.LogInformation($"El usuario {model.Username} ingresó correctamente");
                return RedirectToAction("Index", "Home");
            }
            _logger.LogWarning($"Intento de acceso inválido - Usuario: {model.Username} + Clave ingresada: {model.Password}");
            model.ErrorMessage = "Credenciales inválidas.";
            return View("Index", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            model.ErrorMessage = "Ocurrio un error inesperado.";
            return View("Index", model);
        }
    }
    // [HttpGet] Cierra sesión
    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Index");
    }
}