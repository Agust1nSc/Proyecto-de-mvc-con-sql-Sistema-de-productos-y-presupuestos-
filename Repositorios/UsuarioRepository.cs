using Microsoft.Data.Sqlite;
using MVC.Interfaces;
using MVC.Models;


namespace MVC.Repositorios
{
    public class UsuarioRepository : IUserRepository
    {
        private string cadenaConexion = "Data Source=DB/Tienda.db";

        public Usuario GetUser(string usuario, string contrasena)
        {
            Usuario user = null;

            const string sql = @"
                SELECT Id, Nombre, User, Pass, Rol
                FROM Usuarios
                WHERE User = @Usuario AND Pass = @Contrasena;
            ";

            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();

                using (var comando = new SqliteCommand(sql, conexion))
                {
                    // Parámetros para evitar inyección SQL
                    comando.Parameters.AddWithValue("@Usuario", usuario);
                    comando.Parameters.AddWithValue("@Contrasena", contrasena);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Usuario
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                User = reader.GetString(2),
                                Pass = reader.GetString(3),
                                Rol = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}