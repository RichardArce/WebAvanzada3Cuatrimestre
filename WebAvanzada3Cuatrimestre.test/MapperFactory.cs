using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using WebAvanzada3Cuatrimestre.Application.Mapping;

namespace WebAvanzada3Cuatrimestre.test
{
    // Crea el mapper real con el perfil de la aplicacion, asi las pruebas tambien validan el mapeo
    // en lugar de simularlo con un mock
    public static class MapperFactory
    {
        /// <summary>
        /// Construye un IMapper configurado con el perfil MapeoClases.
        /// </summary>
        /// <returns>Instancia de IMapper lista para usar en las pruebas.</returns>
        public static IMapper Crear()
        {
            var configuracion = new MapperConfiguration(cfg => cfg.AddProfile<MapeoClases>(), NullLoggerFactory.Instance);
            return configuracion.CreateMapper();
        }
    }
}
