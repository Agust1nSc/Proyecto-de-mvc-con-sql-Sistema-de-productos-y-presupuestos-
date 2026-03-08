using System.ComponentModel.DataAnnotations;

public class PresupuestoDetalle
{
    Productos producto;
    int cantidad;

    public PresupuestoDetalle(Productos producto, int cantidad)
    {
        this.producto = producto;
        this.cantidad = cantidad;
    }
    public PresupuestoDetalle()
    {

    }

    public Productos Producto { get => producto; set => producto = value; }
    public int Cantidad { get => cantidad; set => cantidad = value; }
}