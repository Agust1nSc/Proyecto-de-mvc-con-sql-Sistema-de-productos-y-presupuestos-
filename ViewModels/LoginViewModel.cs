namespace MVC.ViewModels;

public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }

    // Este campo es necesario para mostrar los errores que aparecen en tu imagen
    // como "Credenciales inválidas" o "Debe ingresar usuario..."
    public string ErrorMessage { get; set; }
}