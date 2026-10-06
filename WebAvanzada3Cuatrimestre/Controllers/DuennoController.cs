using Microsoft.AspNetCore.Mvc;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;

namespace WebAvanzada3Cuatrimestre.Controllers
{
    public class DuennoController : Controller
    {
        private readonly IDuennoServicio _duennoServicio;

        public DuennoController(IDuennoServicio duennoServicio)
        {
            _duennoServicio = duennoServicio;
        }

        public async Task<IActionResult> Index()
        {
            var duennos = await _duennoServicio.ObtenerDuennosAsync();
            return View(duennos);
        }

        public async Task<IActionResult> ObtenerDuennos()
        {
            var duennos = await _duennoServicio.ObtenerDuennosAsync();
            return Json(duennos);
        }

        public async Task<IActionResult> ObtenerDuennoPorId(int id)
        {
            var duenno = await _duennoServicio.ObtenerDuennoPorIdAsync(id);
            return Json(duenno);
        }

        public async Task<IActionResult> CrearDuenno(DuennoDto duenno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _duennoServicio.CrearDuennoAsync(duenno);
            return Json(respuesta);
        }

        public async Task<IActionResult> ActualizarDuenno(DuennoDto duenno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _duennoServicio.ActualizarDuennoAsync(duenno);
            return Json(respuesta);
        }

        public async Task<IActionResult> EliminarDuenno(int id)
        {
            var respuesta = await _duennoServicio.EliminarDuennoAsync(id);
            return Json(respuesta);
        }
    }
}
