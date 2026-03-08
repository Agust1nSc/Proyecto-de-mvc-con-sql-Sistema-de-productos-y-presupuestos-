using Microsoft.Data.Sqlite;
using MVC.Interfaces;
using System;
using System.Collections.Generic;

namespace MVC.Repositorios
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly string cadenaConexion;

        public PresupuestoRepository(string cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
        }

        public void Crear(Presupuesto presupuesto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)
                       VALUES (@nombre, @fecha)";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombre", presupuesto.NombreDestinatario);
            comando.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion.ToString("yyyy-MM-dd"));
            int filas = comando.ExecuteNonQuery();

            if (filas == 0)
            {
                throw new Exception("No se pudo crear el presupuesto");
            }
        }

        public List<Presupuesto> Listar()
        {
            var lista = new List<Presupuesto>();

            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion 
                       FROM Presupuestos";

            using var comando = new SqliteCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new Presupuesto
                {
                    IdPresupuesto = lector.GetInt32(0),
                    NombreDestinatario = lector.GetString(1),
                    FechaCreacion = DateTime.Parse(lector.GetString(2)),
                    Detalle = ObtenerDetalle(lector.GetInt32(0))
                });
            }
            if (lista.Count == 0)
            {
                throw new Exception("Lista inexistente");
            }
            return lista;
        }

        public Presupuesto ObtenerPorId(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion 
                   FROM Presupuestos WHERE idPresupuesto = @id";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@id", id);

            using var lector = comando.ExecuteReader();

            if (!lector.Read())
            {
                throw new Exception($"No existe presupuesto con id {id}");
            }

            return new Presupuesto
            {
                IdPresupuesto = lector.GetInt32(0),
                NombreDestinatario = lector.GetString(1),
                FechaCreacion = DateTime.Parse(lector.GetString(2)),
                Detalle = ObtenerDetalle(id)
            };
        }

        private List<PresupuestoDetalle> ObtenerDetalle(int idPresupuesto)
        {
            var lista = new List<PresupuestoDetalle>();

            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = @"SELECT d.idProducto, d.Cantidad,
                          p.Descripcion, p.Precio
                   FROM PresupuestosDetalle d
                   JOIN Productos p ON d.idProducto = p.IdProducto
                   WHERE d.idPresupuesto = @id";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@id", idPresupuesto);

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new PresupuestoDetalle
                {
                    Producto = new Productos
                    {
                        IdProducto = lector.GetInt32(0),
                        Descripcion = lector.GetString(2),
                        Precio = lector.GetDecimal(3)
                    },
                    Cantidad = lector.GetInt32(1)
                });
            }

            return lista;
        }

        public bool AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            // 1. Intentamos insertar el producto nuevo en el detalle
            string sql = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                   VALUES (@presupuestoId, @productoId, @cantidad)";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@presupuestoId", idPresupuesto);

            // CAMBIO IMPORTANTE: Usamos la variable int directa
            comando.Parameters.AddWithValue("@productoId", idProducto);

            comando.Parameters.AddWithValue("@cantidad", cantidad);

            try
            {
                int filas = comando.ExecuteNonQuery();

                if (filas == 0)
                {
                    throw new Exception("No se pudo agregar el producto al presupuesto");
                }

                return true;
            }
            catch
            {

                string sqlUpdate = @"UPDATE PresupuestosDetalle 
                             SET Cantidad = Cantidad + @cantidad
                             WHERE idPresupuesto = @presupuestoId AND idProducto = @productoId";

                using var comando2 = new SqliteCommand(sqlUpdate, conexion);
                comando2.Parameters.AddWithValue("@cantidad", cantidad);
                comando2.Parameters.AddWithValue("@presupuestoId", idPresupuesto);


                comando2.Parameters.AddWithValue("@productoId", idProducto);

                int filasUpdate = comando2.ExecuteNonQuery();

                if (filasUpdate == 0)
                {
                    throw new Exception("No se pudo agregar ni actualizar el producto en el presupuesto");
                }

                return true;
            }
        }

        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql =
            @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id;
          DELETE FROM Presupuestos WHERE idPresupuesto = @id;";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@id", id);

            int filas = comando.ExecuteNonQuery();
            if (filas == 0)
                throw new Exception("No se pudo eliminar el presupuestO");
            return true;
        }

        public bool Modificar(int id, Presupuesto presupuestoEditado)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            string sql = @"UPDATE Presupuestos 
                       SET NombreDestinatario = @nombre,
                           FechaCreacion = @fecha
                       WHERE idPresupuesto = @id";

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombre", presupuestoEditado.NombreDestinatario);
            comando.Parameters.AddWithValue("@fecha", presupuestoEditado.FechaCreacion.ToString("yyyy-MM-dd"));
            comando.Parameters.AddWithValue("@id", id);

            int filas = comando.ExecuteNonQuery();
            if (filas == 0)
                throw new Exception("No se pudo modificar el presupuesto");

            return true;
        }
    }
}