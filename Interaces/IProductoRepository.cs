namespace MVC.Interfaces
{
    public interface IProductoRepository
    {
        List<Productos> Listar();
        Productos ObtenerPorId(int id);
        void Crear(Productos producto);
        bool Modificar(int id, Productos nuevoProducto);
        bool Eliminar(int id);
    }
}