using System.ComponentModel.DataAnnotations;

public class Productos
{

    int idProducto;
    string descripcion;
    decimal precio;

    public Productos()
    {
    }

    public Productos(int idProducto, string descripcion, decimal precio)
    {
        this.descripcion = descripcion;
        this.IdProducto = idProducto;
        this.precio = precio;
    }

   
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public decimal Precio { get => precio; set => precio = value; }
    public int IdProducto { get => idProducto; set => idProducto = value; }

}