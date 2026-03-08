using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList
using MVC.Interfaces;
public class PresupuestosController : Controller
{
    private readonly IPresupuestoRepository _presupuestoRepository;
    private readonly IProductoRepository _productoRepo;

    private readonly IAuthenticationService _authService;
    private readonly ILogger<PresupuestosController> _logger;

    public PresupuestosController(
       IPresupuestoRepository presupuestoRepository,
       IProductoRepository productoRepo, IAuthenticationService authService, ILogger<PresupuestosController> logger)
    {
        _presupuestoRepository = presupuestoRepository;
        _productoRepo = productoRepo;
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
    public IActionResult AgregarProducto(int id)
    {
        var securityCheck = CheckLecturaPermissions();
        if (securityCheck != null) return securityCheck;

        // 1. Obtener los productos para el SelectList
        List<Productos> productos = _productoRepo.Listar();

        // 2. Crear el ViewModel
        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            IdPresupuesto = id, // Pasamos el ID del presupuesto actual
                                // 3. Crear el SelectList
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };
        Console.WriteLine("Cantidad productos: " + productos.Count);
        return View("AgregarProducto", model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"CAMPO: {entry.Key} - ERROR: {error.ErrorMessage}");
                    }
                }

                var productos = _productoRepo.Listar();
                model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
                return View("AgregarProducto", model);
            }

            _presupuestoRepository.AgregarProductoAPresupuesto(
                model.IdPresupuesto,
                model.IdProducto,
                model.Cantidad
            );

            return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }



    [HttpGet]
    public IActionResult Index()
    {

        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        // Verifica Nivel de acceso que necesite validar
        if (_authService.HasAccessLevel("Administrador") ||
       _authService.HasAccessLevel("Cliente"))
        {
            //si es es valido entra sino vuelve a login
            var presupuestos = _presupuestoRepository.Listar();
            return View(presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    public IActionResult Details(int id)
    {

        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        // Verifica Nivel de acceso que necesite validar
        if (_authService.HasAccessLevel("Administrador") ||
       _authService.HasAccessLevel("Cliente"))
        {
            //si es es valido entra sino vuelve a login
            Presupuesto presupuestos = _presupuestoRepository.ObtenerPorId(id);
            return View(presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckEscrituraPermissions();
        if (securityCheck != null) return securityCheck;
        return View();
    }

    [HttpPost]
    public IActionResult Create(Presupuesto presupuesto)
    {
        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            _presupuestoRepository.Crear(presupuesto);
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

        var presupuesto = _presupuestoRepository.ObtenerPorId(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        var viewModel = new PresupuestoViewModel
        {
            IdPresupuesto = presupuesto.IdPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(PresupuestoViewModel model)
    {
        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            if (model.FechaCreacion.Date > DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(model.FechaCreacion),
                    "La fecha de creación no puede ser futura"
                );
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var presupuesto = new Presupuesto
            {
                IdPresupuesto = model.IdPresupuesto,
                NombreDestinatario = model.NombreDestinatario,
                FechaCreacion = model.FechaCreacion
            };

            _presupuestoRepository.Modificar(presupuesto.IdPresupuesto, presupuesto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult Delete(Presupuesto presupuesto)
    {
        try
        {
            var securityCheck = CheckEscrituraPermissions();
            if (securityCheck != null) return securityCheck;

            _presupuestoRepository.Eliminar(presupuesto.IdPresupuesto);
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

        var presupuesto = _presupuestoRepository.ObtenerPorId(id);

        if (presupuesto == null)
        {
            return NotFound();
        }

        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}