using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.test
{
    // Pruebas de las reglas de negocio de la entidad Carro (capa de dominio)
    public class CarroTest
    {
        [Theory]
        [InlineData("Ferrari")]
        [InlineData("ferrari")]
        [InlineData("FERRARI")]
        public void ValidarReglaNegocioSoloFerrari_MarcaFerrari_RetornaTrue(string marca)
        {
            // Arrange
            var carro = new Carro { Marca = marca };

            // Act
            var resultado = carro.ValidarReglaNegocioSoloFerrari();

            // Assert
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("Toyota")]
        [InlineData("Ferrari ")]
        [InlineData("")]
        public void ValidarReglaNegocioSoloFerrari_MarcaDistinta_RetornaFalse(string marca)
        {
            // Arrange
            var carro = new Carro { Marca = marca };

            // Act
            var resultado = carro.ValidarReglaNegocioSoloFerrari();

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void ValidarReglaNegocioCarrosNuevos_CreadoHoy_RetornaTrue()
        {
            // Arrange
            var carro = new Carro { FechaCreacion = DateTime.Now };

            // Act
            var resultado = carro.ValidarReglaNegocioCarrosNuevos();

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void ValidarReglaNegocioCarrosNuevos_CreadoAyer_RetornaFalse()
        {
            // Arrange
            var carro = new Carro { FechaCreacion = DateTime.Now.AddDays(-1) };

            // Act
            var resultado = carro.ValidarReglaNegocioCarrosNuevos();

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void ValidarReglaNegocioCarrosNuevos_SinFecha_RetornaFalse()
        {
            // Arrange
            var carro = new Carro { FechaCreacion = null };

            // Act
            var resultado = carro.ValidarReglaNegocioCarrosNuevos();

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void ValidarReglasDeNegocio_FerrariCreadoHoy_RetornaTrue()
        {
            // Arrange
            var carro = new Carro { Marca = "Ferrari", FechaCreacion = DateTime.Now };

            // Act
            var resultado = carro.ValidarReglasDeNegocio();

            // Assert
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("Toyota", 0)]  // Falla la regla de marca
        [InlineData("Ferrari", -1)] // Falla la regla de carro nuevo
        [InlineData("Toyota", -1)]  // Fallan ambas
        public void ValidarReglasDeNegocio_AlgunaReglaFalla_RetornaFalse(string marca, int diasDesdeCreacion)
        {
            // Arrange
            var carro = new Carro { Marca = marca, FechaCreacion = DateTime.Now.AddDays(diasDesdeCreacion) };

            // Act
            var resultado = carro.ValidarReglasDeNegocio();

            // Assert
            Assert.False(resultado);
        }
    }
}
