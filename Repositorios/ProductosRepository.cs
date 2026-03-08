using Microsoft.Data.Sqlite;
using MVC.Interfaces;
using SQLitePCL;

namespace MVC.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string cadenaConexion;

        public ProductoRepository(string cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
        }

        public List<Productos> Listar()
        {
            var lista = new List<Productos>();

            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos";

            using var comando = new SqliteCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new Productos
                {
                    IdProducto = lector.GetInt32(0),
                    Descripcion = lector.GetString(1),
                    Precio = lector.GetInt32(2)
                });
            }
            if (lista.Count == 0)
            {
                throw new Exception("Lista inexistente");
            }

            return lista;
        }

        public Productos ObtenerPorId(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @id";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@id", id);

            using var lector = comando.ExecuteReader();


            if (!lector.Read())
                throw new Exception($"No existe producto con id {id}");

            return new Productos
            {
                IdProducto = lector.GetInt32(0),
                Descripcion = lector.GetString(1),
                Precio = lector.GetInt32(2)
            };
        }

        public void Crear(Productos producto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@descripcion, @precio)";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            comando.Parameters.AddWithValue("@precio", producto.Precio);

            if (comando.ExecuteNonQuery() == 0)
                throw new Exception("No se pudo crear el producto");
        }

        public bool Modificar(int id, Productos nuevoProducto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = "UPDATE Productos SET Descripcion=@desc, Precio=@precio WHERE IdProducto=@id";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@desc", nuevoProducto.Descripcion);
            comando.Parameters.AddWithValue("@precio", nuevoProducto.Precio);
            comando.Parameters.AddWithValue("@id", id);

            int filas = comando.ExecuteNonQuery();
            if (filas == 0)
                throw new Exception("No se pudo modificar el producto");

            return true;
        }

        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();


            string sqlDetalle = @"DELETE FROM PresupuestosDetalle WHERE idProducto = @id";

            using (var cmdDetalle = new SqliteCommand(sqlDetalle, conexion))
            {
                cmdDetalle.Parameters.AddWithValue("@id", id);
                cmdDetalle.ExecuteNonQuery();
            }


            string sqlProducto = @"DELETE FROM Productos WHERE IdProducto = @id";

            using var cmdProducto = new SqliteCommand(sqlProducto, conexion);
            cmdProducto.Parameters.AddWithValue("@id", id);
            int filas = cmdProducto.ExecuteNonQuery();
            if (filas == 0)
                throw new Exception("No se pudo eliminar el producto");
            return true;
        }
    }
}