using AutoMapper;
using Moq;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Repository;

namespace WebAvanzada3Cuatrimestre.test
{
    public class DuennoServicioTest
    {
        // xUnit crea una instancia nueva de la clase por cada prueba, asi cada prueba tiene sus propios mocks
        private readonly Mock<IDuennoRepository> _repositorioMock = new();
        private readonly DuennoServicio _servicio;

        public DuennoServicioTest()
        {
            _servicio = new DuennoServicio(_repositorioMock.Object, MapperFactory.Crear());
        }

        private static DuennoDto CrearDuennoDto(int edad = 25) => new()
        {
            Id = 1,
            Nombre = "Juan",
            Apellido1 = "Perez",
            Apellido2 = "Mora",
            Edad = edad
        };

        //Funcion - que va a evaluar - su resultado

        [Fact]
        public async Task CrearDuenno_MayorDeEdad_Correcto()// Solo evaluar una cosa a la vez - no mezclar cosas - no mezclar logica de negocio con logica de infraestructura
        {
            // Arrange
            var repositorioMock = new Mock<IDuennoRepository>();
            var mapper = new Mock<IMapper>();

            var duennoServicio = new DuennoServicio(repositorioMock.Object, mapper.Object); // Ejecuta la accion - simulacion de mi servicio

            var dto = new DuennoDto
            {
                Nombre = "Juan",
                Apellido1 = "Perez",
                Edad = 25
            };

            var entity = new Duenno
            {
                Nombre = "Juan",
                Apellido1 = "Perez",
                Edad = 25
            };

            mapper.Setup(m => m.Map<Duenno>(dto)).Returns(entity);
            repositorioMock.Setup(r => r.CreateDuennoAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            //Puede que falte savechanges


            //Act
            var resultado = await duennoServicio.CrearDuennoAsync(dto);


            //Assert
            Assert.True(resultado.EsCorrecto);
        }

        // ---------- ObtenerDuennosAsync ----------

        [Fact]
        public async Task ObtenerDuennos_ExistenRegistros_RetornaListaMapeada()
        {
            // Arrange
            var duennos = new List<Duenno>
            {
                new() { Id = 1, Nombre = "Juan", Apellido1 = "Perez", Apellido2 = "Mora", Edad = 25 },
                new() { Id = 2, Nombre = "Ana", Apellido1 = "Rojas", Apellido2 = "Solis", Edad = 40 }
            };
            _repositorioMock.Setup(r => r.GetAllDuennosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(duennos);

            // Act
            var resultado = await _servicio.ObtenerDuennosAsync();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Juan", resultado[0].Nombre);
            Assert.Equal("Solis", resultado[1].Apellido2);
            Assert.Equal(40, resultado[1].Edad);
        }

        [Fact]
        public async Task ObtenerDuennos_SinRegistros_RetornaListaVacia()
        {
            // Arrange
            _repositorioMock.Setup(r => r.GetAllDuennosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Duenno>());

            // Act
            var resultado = await _servicio.ObtenerDuennosAsync();

            // Assert
            Assert.Empty(resultado);
        }

        // ---------- ObtenerDuennoPorIdAsync ----------

        [Fact]
        public async Task ObtenerDuennoPorId_Existe_RetornaDto()
        {
            // Arrange
            var duenno = new Duenno { Id = 7, Nombre = "Ana", Apellido1 = "Rojas", Apellido2 = "Solis", Edad = 40 };
            _repositorioMock.Setup(r => r.GetDuennoByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(duenno);

            // Act
            var resultado = await _servicio.ObtenerDuennoPorIdAsync(7);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(7, resultado.Id);
            Assert.Equal("Ana", resultado.Nombre);
        }

        [Fact]
        public async Task ObtenerDuennoPorId_NoExiste_RetornaNull()
        {
            // Arrange
            _repositorioMock.Setup(r => r.GetDuennoByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Duenno?)null);

            // Act
            var resultado = await _servicio.ObtenerDuennoPorIdAsync(99);

            // Assert
            Assert.Null(resultado);
        }

        // ---------- CrearDuennoAsync ----------

        [Fact]
        public async Task CrearDuenno_DatosValidos_LlamaRepositorioYRetornaDato()
        {
            // Arrange
            _repositorioMock.Setup(r => r.CreateDuennoAsync(It.IsAny<Duenno>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _servicio.CrearDuennoAsync(CrearDuennoDto());

            // Assert
            Assert.True(resultado.EsCorrecto);
            Assert.Equal(200, resultado.Codigo);
            Assert.Equal("Juan", resultado.Dato.Nombre);
            _repositorioMock.Verify(r => r.CreateDuennoAsync(
                It.Is<Duenno>(d => d.Nombre == "Juan" && d.Apellido1 == "Perez" && d.Edad == 25),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(17)]
        [InlineData(18)]
        public async Task CrearDuenno_MenorDeEdad_RetornaError400SinLlamarRepositorio(int edad)
        {
            // Act
            var resultado = await _servicio.CrearDuennoAsync(CrearDuennoDto(edad));

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(400, resultado.Codigo);
            Assert.Equal("El dueño debe ser mayor de 18 años.", resultado.Mensaje);
            _repositorioMock.Verify(r => r.CreateDuennoAsync(It.IsAny<Duenno>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CrearDuenno_RepositorioFalla_RetornaError500()
        {
            // Arrange
            _repositorioMock.Setup(r => r.CreateDuennoAsync(It.IsAny<Duenno>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var resultado = await _servicio.CrearDuennoAsync(CrearDuennoDto());

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(500, resultado.Codigo);
            Assert.Equal("No se pudo crear el dueño.", resultado.Mensaje);
        }

        [Fact]
        public async Task CrearDuenno_ConCancellationToken_PropagaElTokenAlRepositorio()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            _repositorioMock.Setup(r => r.CreateDuennoAsync(It.IsAny<Duenno>(), cts.Token)).ReturnsAsync(true);

            // Act
            await _servicio.CrearDuennoAsync(CrearDuennoDto(), cts.Token);

            // Assert
            _repositorioMock.Verify(r => r.CreateDuennoAsync(It.IsAny<Duenno>(), cts.Token), Times.Once);
        }

        // ---------- ActualizarDuennoAsync ----------

        [Fact]
        public async Task ActualizarDuenno_DatosValidos_RetornaCorrecto()
        {
            // Arrange
            _repositorioMock.Setup(r => r.UpdateDuennoAsync(It.IsAny<Duenno>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _servicio.ActualizarDuennoAsync(CrearDuennoDto());

            // Assert
            Assert.True(resultado.EsCorrecto);
            Assert.Equal(1, resultado.Dato.Id);
            _repositorioMock.Verify(r => r.UpdateDuennoAsync(
                It.Is<Duenno>(d => d.Id == 1 && d.Apellido2 == "Mora"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ActualizarDuenno_MenorDeEdad_RetornaError400SinLlamarRepositorio()
        {
            // Act
            var resultado = await _servicio.ActualizarDuennoAsync(CrearDuennoDto(edad: 15));

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(400, resultado.Codigo);
            _repositorioMock.Verify(r => r.UpdateDuennoAsync(It.IsAny<Duenno>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ActualizarDuenno_RepositorioFalla_RetornaError500()
        {
            // Arrange
            _repositorioMock.Setup(r => r.UpdateDuennoAsync(It.IsAny<Duenno>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var resultado = await _servicio.ActualizarDuennoAsync(CrearDuennoDto());

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(500, resultado.Codigo);
            Assert.Equal("No se pudo actualizar el dueño.", resultado.Mensaje);
        }

        // ---------- EliminarDuennoAsync ----------

        [Fact]
        public async Task EliminarDuenno_Existe_RetornaCorrecto()
        {
            // Arrange
            _repositorioMock.Setup(r => r.DeleteDuennoAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _servicio.EliminarDuennoAsync(3);

            // Assert
            Assert.True(resultado.EsCorrecto);
            Assert.Equal(200, resultado.Codigo);
        }

        [Fact]
        public async Task EliminarDuenno_RepositorioFalla_RetornaError500()
        {
            // Arrange
            _repositorioMock.Setup(r => r.DeleteDuennoAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var resultado = await _servicio.EliminarDuennoAsync(3);

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(500, resultado.Codigo);
            Assert.Equal("No se pudo eliminar el dueño.", resultado.Mensaje);
        }
    }
}
