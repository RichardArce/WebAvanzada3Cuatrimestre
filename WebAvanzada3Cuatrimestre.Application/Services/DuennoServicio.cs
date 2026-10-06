using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Infrastructure.Repository;

namespace WebAvanzada3Cuatrimestre.Application.Services
{
    public class DuennoServicio : IDuennoServicio
    {
        private readonly IDuennoRepository _duennoRepository;
        private readonly AutoMapper.IMapper _mapper;

        public DuennoServicio(IDuennoRepository duennoRepository, AutoMapper.IMapper mapper)
        {
            _duennoRepository = duennoRepository;
            _mapper = mapper;
        }

        public async Task<List<DuennoDto>> ObtenerDuennosAsync(CancellationToken cancellationToken = default)
        {
            var duennos = await _duennoRepository.GetAllDuennosAsync(cancellationToken);
            return _mapper.Map<List<DuennoDto>>(duennos);
        }

        public async Task<DuennoDto?> ObtenerDuennoPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var duenno = await _duennoRepository.GetDuennoByIdAsync(id, cancellationToken);
            return _mapper.Map<DuennoDto>(duenno);
        }

        public async Task<Respuesta<DuennoDto>> CrearDuennoAsync(DuennoDto duenno, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<DuennoDto>();
            var duennoEntity = _mapper.Map<Domain.Duenno>(duenno);

            if (!duennoEntity.ValidarReglaNegocioMayorEdad())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "El dueño debe ser mayor de 18 años.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _duennoRepository.CreateDuennoAsync(duennoEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo crear el dueño.";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<DuennoDto>(duennoEntity);
            return respuesta;
        }

        public async Task<Respuesta<DuennoDto>> ActualizarDuennoAsync(DuennoDto duenno, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<DuennoDto>();
            var duennoEntity = _mapper.Map<Domain.Duenno>(duenno);

            if (!duennoEntity.ValidarReglaNegocioMayorEdad())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "El dueño debe ser mayor de 18 años.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _duennoRepository.UpdateDuennoAsync(duennoEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo actualizar el dueño.";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<DuennoDto>(duennoEntity);
            return respuesta;
        }

        public async Task<Respuesta<DuennoDto>> EliminarDuennoAsync(int id, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<DuennoDto>();

            if (!await _duennoRepository.DeleteDuennoAsync(id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo eliminar el dueño.";
                respuesta.Codigo = 500;
            }

            return respuesta;
        }
    }
}
