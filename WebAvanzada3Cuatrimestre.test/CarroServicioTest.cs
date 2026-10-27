using Moq;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Repository;

namespace WebAvanzada3Cuatrimestre.test
{
    public class CarroServicioTest
    {
        // xUnit crea una instancia nueva de la clase por cada prueba, asi cada prueba tiene sus propios mocks
        private readonly Mock<ICarroRepository> _repositorioMock = new();
        private readonly CarroServicio _servicio;

        public CarroServicioTest()
        {
            _servicio = new CarroServicio(_repositorioMock.Object, MapperFactory.Crear());
        }

        private static CarroDto CrearCarroDto(string marca = "Ferrari") => new()
        {
            Id = 1,
            Placa = "ABC-123",
            Marca = marca,
            Fkduenno = 5
        };

        // ---------- ObtenerCarrosAsync ----------

        [Fact]
        public async Task ObtenerCarros_ExistenRegistros_RetornaListaConDuennoMapeado()
        {
            // Arrange
            var carros = new List<Carro>
            {
                new()
                {
                    Id = 1, Placa = "ABC-123", Marca = "Ferrari", Fkduenno = 5,
                    FkduennoNavigation = new Duenno { Id = 5, Nombre = "Juan", Apellido1 = "Perez", Apellido2 = "Mora", Edad = 30 }
                }
            };
            _repositorioMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(carros);

            // Act
            var resultado = await _servicio.ObtenerCarrosAsync();

            // Assert
            var carro = Assert.Single(resultado);
            Assert.Equal("ABC-123", carro.Placa);
            Assert.Equal(5, carro.Fkduenno);
            Assert.NotNull(carro.Duenno); // FkduennoNavigation -> Duenno, definido en MapeoClases
            Assert.Equal("Juan", carro.Duenno.Nombre);
        }

        [Fact]
        public async Task ObtenerCarros_SinRegistros_RetornaListaVacia()
        {
            // Arrange
            _repositorioMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Carro>());

            // Act
            var resultado = await _servicio.ObtenerCarrosAsync();

            // Assert
            Assert.Empty(resultado);
        }

        // ---------- ObtenerCarroPorIdAsync ----------

        [Fact]
        public async Task ObtenerCarroPorId_Existe_RetornaDto()
        {
            // Arrange
            var carro = new Carro { Id = 4, Placa = "XYZ-999", Marca = "Ferrari", Fkduenno = 2 };
            _repositorioMock.Setup(r => r.GetByIdAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(carro);

            // Act
            var resultado = await _servicio.ObtenerCarroPorIdAsync(4);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(4, resultado.Id);
            Assert.Equal("XYZ-999", resultado.Placa);
            Assert.Equal(2, resultado.Fkduenno);
        }

        [Fact]
        public async Task ObtenerCarroPorId_NoExiste_RetornaNull()
        {
            // Arrange
            _repositorioMock.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((Carro)null!);

            // Act
            var resultado = await _servicio.ObtenerCarroPorIdAsync(99);

            // Assert
            Assert.Null(resultado);
        }

        // ---------- CrearCarroAsync ----------

        [Fact]
        public async Task CrearCarro_MarcaFerrari_LlamaRepositorioYRetornaCorrecto()
        {
            // Arrange
            _repositorioMock.Setup(r => r.CreateAsync(It.IsAny<Carro>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _servicio.CrearCarroAsync(CrearCarroDto());

            // Assert
            Assert.True(resultado.EsCorrecto);
            Assert.Equal(200, resultado.Codigo);
            Assert.Equal("ABC-123", resultado.Dato.Placa);
            _repositorioMock.Verify(r => r.CreateAsync(
                It.Is<Carro>(c => c.Placa == "ABC-123" && c.Marca == "Ferrari" && c.Fkduenno == 5),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CrearCarro_MarcaDistintaDeFerrari_RetornaError400SinLlamarRepositorio()
        {
            // Act
            var resultado = await _servicio.CrearCarroAsync(CrearCarroDto(marca: "Toyota"));

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(400, resultado.Codigo);
            Assert.Equal("Solo se permiten carros de la marca Ferrari.", resultado.Mensaje);
            _repositorioMock.Verify(r => r.CreateAsync(It.IsAny<Carro>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CrearCarro_RepositorioFalla_RetornaError500()
        {
            // Arrange
            _repositorioMock.Setup(r => r.CreateAsync(It.IsAny<Carro>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var resultado = await _servicio.CrearCarroAsync(CrearCarroDto());

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(500, resultado.Codigo);
            Assert.Equal("No se pudo crear el carro.", resultado.Mensaje);
        }

        // ---------- ActualizarCarroAsync ----------

        [Fact]
        public async Task ActualizarCarro_MarcaFerrari_RetornaCorrecto()
        {
            // Arrange
            _repositorioMock.Setup(r => r.UpdateAsync(It.IsAny<Carro>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _servicio.ActualizarCarroAsync(CrearCarroDto());

            // Assert
            Assert.True(resultado.EsCorrecto);
            Assert.Equal(1, resultado.Dato.Id);
            _repositorioMock.Verify(r => r.UpdateAsync(It.Is<Carro>(c => c.Id == 1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ActualizarCarro_MarcaDistintaDeFerrari_RetornaError400SinLlamarRepositorio()
        {
            // Act
            var resultado = await _servicio.ActualizarCarroAsync(CrearCarroDto(marca: "Nissan"));

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(400, resultado.Codigo);
            _repositorioMock.Verify(r => r.UpdateAsync(It.IsAny<Carro>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ActualizarCarro_RepositorioFalla_RetornaError500()
        {
            // Arrange
            _repositorioMock.Setup(r => r.UpdateAsync(It.IsAny<Carro>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var resultado = await _servicio.ActualizarCarroAsync(CrearCarroDto());

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(500, resultado.Codigo);
            Assert.Equal("No se pudo actualizar el carro.", resultado.Mensaje);
        }

        // ---------- EliminarCarroAsync ----------

        [Fact]
        public async Task EliminarCarro_Existe_RetornaCorrecto()
        {
            // Arrange
            _repositorioMock.Setup(r => r.DeleteAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _servicio.EliminarCarroAsync(3);

            // Assert
            Assert.True(resultado.EsCorrecto);
            Assert.Equal(200, resultado.Codigo);
        }

        [Fact]
        public async Task EliminarCarro_RepositorioFalla_RetornaError500()
        {
            // Arrange
            _repositorioMock.Setup(r => r.DeleteAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var resultado = await _servicio.EliminarCarroAsync(3);

            // Assert
            Assert.False(resultado.EsCorrecto);
            Assert.Equal(500, resultado.Codigo);
            Assert.Equal("No se pudo eliminar el carro.", resultado.Mensaje);
        }
    }
}
