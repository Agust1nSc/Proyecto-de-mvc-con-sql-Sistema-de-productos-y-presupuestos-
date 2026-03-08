namespace MVC.Interfaces
{
    public interface IPresupuestoRepository
    {
        void Crear(Presupuesto presupuesto);
        List<Presupuesto> Listar();
        Presupuesto ObtenerPorId(int id);
        bool Eliminar(int id);
        bool Modificar(int id, Presupuesto presupuestoEditado);
        bool AgregarProductoAPresupuesto(int idPresupuesto, int IdProducto, int cantidad);
    }
    
}