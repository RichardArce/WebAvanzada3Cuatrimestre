using Microsoft.AspNetCore.Mvc;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;

namespace WebAvanzada3Cuatrimestre.Controllers
{
    // Controlador del modulo de administracion de duennos. Solo coordina las peticiones HTTP,
    // toda la logica de negocio vive en la capa de aplicacion (IDuennoServicio)
    public class DuennoController : Controller
    {
        private readonly IDuennoServicio _duennoServicio;

        public DuennoController(IDuennoServicio duennoServicio) // Inyección de dependencias
        {
            _duennoServicio = duennoServicio;
        }

        /// <summary>
        /// Muestra la pantalla de administración. Los datos de la tabla se cargan por AJAX desde ObtenerDuennos.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene todos los duennos en formato JSON para la tabla.
        /// </summary>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Lista de duennos.</returns>
        public async Task<IActionResult> ObtenerDuennos(CancellationToken cancellationToken)
        {
            var duennos = await _duennoServicio.ObtenerDuennosAsync(cancellationToken);
            return Json(duennos);
        }

        /// <summary>
        /// Obtiene un duenno por su id en formato JSON para cargar el formulario de edición.
        /// </summary>
        /// <param name="id">Id del duenno.</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>El duenno encontrado o null.</returns>
        public async Task<IActionResult> ObtenerDuennoPorId(int id, CancellationToken cancellationToken)
        {
            var duenno = await _duennoServicio.ObtenerDuennoPorIdAsync(id, cancellationToken);
            return Json(duenno);
        }

        /// <summary>
        /// Crea un nuevo duenno aplicando las reglas de negocio del servicio.
        /// </summary>
        /// <param name="duenno">Datos del formulario (binding model).</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Objeto Respuesta con el resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> CrearDuenno(DuennoDto duenno, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _duennoServicio.CrearDuennoAsync(duenno, cancellationToken);
            return Json(respuesta);
        }

        /// <summary>
        /// Actualiza un duenno existente aplicando las reglas de negocio del servicio.
        /// </summary>
        /// <param name="duenno">Datos del formulario (binding model), incluye el Id.</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Objeto Respuesta con el resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> ActualizarDuenno(DuennoDto duenno, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _duennoServicio.ActualizarDuennoAsync(duenno, cancellationToken);
            return Json(respuesta);
        }

        /// <summary>
        /// Elimina un duenno. La base de datos elimina en cascada los carros asociados.
        /// </summary>
        /// <param name="id">Id del duenno.</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Objeto Respuesta con el resultado de la operación.</returns>
        [HttpDelete]
        public async Task<IActionResult> EliminarDuenno(int id, CancellationToken cancellationToken)
        {
            var respuesta = await _duennoServicio.EliminarDuennoAsync(id, cancellationToken);
            return Json(respuesta);
        }
    }
}
