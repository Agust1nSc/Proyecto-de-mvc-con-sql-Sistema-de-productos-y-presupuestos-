namespace MVC.Interfaces;
using MVC.Models;
public interface IUserRepository
{
    // Retorna el objeto Usuario si las credenciales son válidas, sino null.
    Usuario GetUser(string username, string password);
}