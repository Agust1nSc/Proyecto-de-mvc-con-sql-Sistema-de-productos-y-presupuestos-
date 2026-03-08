public class Presupuesto
{
    public int IdPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }

    public List<PresupuestoDetalle> Detalle { get; set; } = new();

    public decimal MontoPresupuesto()
    {
        decimal subtotal = 0;
        foreach (var aux in Detalle)
        {
            subtotal += aux.Producto.Precio * aux.Cantidad;
        }
        return subtotal;
    }

    public decimal MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21m;
    }

    public int CantidadProductos()
    {
        int cantidad = 0;
        foreach (var aux in Detalle)
        {
            cantidad += aux.Cantidad;
        }
        return cantidad;
    }
}
