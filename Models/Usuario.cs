namespace MVC.Models
{
    public class Usuario
    {
    
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? User{ get; set; }
        public string? Pass { get; set; } 
        public string? Rol { get; set; }
        public Usuario()
        {
        } 

        public Usuario(int id, string nombre, string user, string pass, string rol)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.User = user;
            this.Pass = pass;
            this.Rol = rol;
        }
    }
}