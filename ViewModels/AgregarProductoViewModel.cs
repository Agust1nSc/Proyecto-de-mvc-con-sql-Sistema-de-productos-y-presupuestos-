using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MVC.ViewModels
{
    public class AgregarProductoViewModel
    {
        // ID del presupuesto
        public int IdPresupuesto { get; set; }

        // Producto seleccionado
        [Display(Name = "Producto a agregar")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto")]
        public int IdProducto { get; set; }

        // Cantidad
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        // SOLO PARA LA VISTA → NO SE VALIDA
        [ValidateNever]   // 🔥 ESTA LÍNEA ES LA CLAVE
        public SelectList ListaProductos { get; set; }
    }
}
