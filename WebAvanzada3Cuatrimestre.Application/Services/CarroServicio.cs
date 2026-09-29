using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Infrastructure.Repository;

namespace WebAvanzada3Cuatrimestre.Application.Services
{
    public class CarroServicio : ICarroServicio
    {

        private readonly ICarroRepository _carroRepository;
        private readonly AutoMapper.IMapper _mapper;


        public CarroServicio(ICarroRepository carroRepository, AutoMapper.IMapper mapper)
        {
            _carroRepository = carroRepository;
            _mapper = mapper;
        }


        public async Task<List<CarroDto>> ObtenerCarrosAsync(CancellationToken cancellationToken = default)
        {

            var carros = await _carroRepository.GetAllCarrosAsync(cancellationToken);
            var carrosDto = _mapper.Map<List<CarroDto>>(carros);

            return carrosDto;
        }

        public async Task<CarroDto?> ObtenerCarroPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var carro = await _carroRepository.GetCarroByIdAsync(id, cancellationToken);
            
            var carroDto = _mapper.Map<CarroDto>(carro);

            return carroDto;
        }

        public async Task<Respuesta<CarroDto>> CrearCarroAsync(CarroDto carro, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<CarroDto>();


            var carroEntity = _mapper.Map<Domain.Carro>(carro);

            // Validar la regla de negocio antes de crear el carro
            if (!carroEntity.ValidarReglaNegocioSoloFerrari()) // Si hay muchas reglas encapsular en un solo metodo ValidarReglasDeNegocio() y retornar un objeto con el resultado de la validación
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "Solo se permiten carros de la marca Ferrari.";
                respuesta.Codigo = 400; // Código de error (puedes ajustarlo según tus necesidades)
                return respuesta;
            }

            // Valida el proceso de creación del carro, si no se pudo crear, lanza una excepción
            if (!await _carroRepository.CreateCarroAsync(carroEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo crear el carro.";
                respuesta.Codigo = 500; // Código de error (puedes ajustarlo según tus necesidades)
            }

            respuesta.Dato = _mapper.Map<CarroDto>(carroEntity);

            return respuesta;
        }

        public async Task<Respuesta<CarroDto>> ActualizarCarroAsync(CarroDto carro, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<CarroDto>();

            var carroEntity = _mapper.Map<Domain.Carro>(carro);


            if (!carroEntity.ValidarReglaNegocioSoloFerrari())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "Solo se permiten carros de la marca Ferrari.";
                respuesta.Codigo = 400; // Código de error (puedes ajustarlo según tus necesidades)
                return respuesta;
            }

            if (!await _carroRepository.UpdateCarroAsync(carroEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo actualizar el carro.";
                respuesta.Codigo = 500; // Código de error (puedes ajustarlo según tus necesidades)
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<CarroDto>(carroEntity);
            return respuesta;
        }

        public async Task<Respuesta<CarroDto>> EliminarCarroAsync(int id, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<CarroDto>();

            if (!await _carroRepository.DeleteCarroAsync(id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo eliminar el carro.";
                respuesta.Codigo = 500; // Código de error (puedes ajustarlo según tus necesidades)
                return respuesta;
            }

            return respuesta;
        }
    }

    // No deberia utilizar Excepciones para manejar las reglas de negocio, deberia utilizar Result 

}
