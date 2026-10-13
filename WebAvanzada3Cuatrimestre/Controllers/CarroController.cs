using Microsoft.AspNetCore.Mvc;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;

namespace WebAvanzada3Cuatrimestre.Controllers
{

    // Un controlador, como dice su nombre controla el flujo de las peticion http, es decir que hace con ellas
    public class CarroController : Controller
    {
        private readonly ICarroServicio _carroServicio;
        private readonly IDuennoServicio _duennoServicio;

        //n servicios

        //Si tenemos muchos servicios ponemos usar un mas personalizado y hacer la carga en infraestructura

        // Cuando hayan muchas cosas repetidas es muy problable que se necesita extraer hacia un servicio, una funcion, un procedimiento que realice esa tarea, para no repetir codigo y tener un codigo mas limpio y mantenible


        public CarroController(ICarroServicio carroServicio, IDuennoServicio duennoServicio) // Inyección de dependencias, es decir que el controlador depende del servicio de carro para funcionar
        {
            _carroServicio = carroServicio;
            _duennoServicio = duennoServicio;
        }

        public async Task<IActionResult> Index()
        {
            var duenno = await _duennoServicio.ObtenerDuennosAsync(); // Ejemplo de cómo obtener un dueño por su ID
            ViewBag.Duennos = duenno; // Pasamos la lista de duennos a la vista para que se pueda mostrar en un select
            return View();
        }
        public async Task<IActionResult> ObtenerCarros() // Este metodo es para obtener los carros en formato json, es decir que no retorna una vista sino un json
        {
            var carros = await _carroServicio.ObtenerCarrosAsync();
            return Json(carros);
        }
        public async Task<IActionResult> ObtenerCarroPorId(int id) // Este metodo es para obtener un carro por su id en formato json
        {
            var carro = await _carroServicio.ObtenerCarroPorIdAsync(id);
            return Json(carro);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCarro(CarroDto carro)// Binding model, es decir que el parametro carro es un objeto que se llena con los datos del formulario que se envia desde la vista
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var respuesta = await _carroServicio.CrearCarroAsync(carro);
            return Json(respuesta);
        }

        public async Task<IActionResult> ActualizarCarro(CarroDto carro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var respuesta = await _carroServicio.ActualizarCarroAsync(carro);
            return Json(respuesta);
        }

        public async Task<IActionResult> EliminarCarro(int id)
        {
            var respuesta = await _carroServicio.EliminarCarroAsync(id);
            return Json(respuesta);
        }
    }
}
