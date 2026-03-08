using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MVC.ViewModels;
using MVC.Interfaces;
public class ProductosController : Controller
{
    private IProductoRepository _productoRepository;
    private readonly IAuthenticationService _authService;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(IProductoRepository productoRepository, IAuthenticationService authService, ILogger<ProductosController> logger)
    {
        _productoRepository = productoRepository;
        _authService = authService;
        _logger = logger;
    }

    private IActionResult CheckLecturaPermissions()
    {
        if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");

        // Si NO es Admin Y NO es Cliente -> Fuera
        if (!_authService.HasAccessLevel("Administrador") && !_authService.HasAccessLevel("Cliente"))
        {
            return RedirectToAction("AccesoDenegado");
        }
        return null;
    }

    // --- VALIDACIÓN PARA MODIFICAR (Solo Admins) ---
    private IActionResult CheckEscrituraPermissions()
    {
        if (!_authService.IsAuthenticated()) return RedirectToAction("Index", "Login");

        // Solo Admin
        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction("AccesoDenegado");
        }
        return null;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckLecturaPermissions();
        if (securityCheck != null) return securityCheck;

        List<Productos> productos = _productoRepository.Listar();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {

        var securityCheck = CheckEscrituraPermissions();
        if (securityCheck != null) return securityCheck;

        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid)
            {
                // Si falla: Devolvemos el ViewModel con los datos y errores a la Vista
                return View(productoVM);
            }

            var nuevoProducto = new Productos
            {
                Descripcion = productoVM.Descripcion,
                Precio = productoVM.Precio
            };

            _productoRepository.Crear(nuevoProducto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }

    [HttpGet]
    public IActionResult Edit(int id)
    {

        var securityCheck = CheckEscrituraPermissions();
        if (securityCheck != null) return securityCheck;

        var producto = _productoRepository.ObtenerPorId(id);
        if (producto == null) return NotFound();

        var viewModel = new ProductoViewModel
        {
            IdProducto = producto.IdProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(ProductoViewModel productoVM)
    {

        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid)
            {
                // Si falla: Devolvemos el ViewModel con los datos y errores a la Vista
                return View(productoVM);
            }

            var nuevoProducto = new Productos
            {
                IdProducto = productoVM.IdProducto,
                Descripcion = productoVM.Descripcion,
                Precio = productoVM.Precio
            };


            _productoRepository.Modificar(nuevoProducto.IdProducto, nuevoProducto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var securityCheck = CheckEscrituraPermissions();
        if (securityCheck != null) return securityCheck;

        var producto = _productoRepository.ObtenerPorId(id);
        if (producto == null) return NotFound();

        return View(producto);
    }

    [HttpPost]
    public IActionResult Delete(Productos producto)
    {
        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            _productoRepository.Eliminar(producto.IdProducto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }

    }
    [HttpGet]
    public IActionResult AccesoDenegado()
    {

        return View();
    }
}
