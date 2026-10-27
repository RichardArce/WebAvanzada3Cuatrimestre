using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.test
{
    // Pruebas de las reglas de negocio de la entidad Duenno (capa de dominio)
    public class DuennoTest
    {
        [Theory]
        [InlineData(19)]
        [InlineData(25)]
        [InlineData(80)]
        public void ValidarReglaNegocioMayorEdad_EdadMayorA18_RetornaTrue(int edad)
        {
            // Arrange
            var duenno = new Duenno { Edad = edad };

            // Act
            var resultado = duenno.ValidarReglaNegocioMayorEdad();

            // Assert
            Assert.True(resultado);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(17)]
        [InlineData(18)] // Limite: la regla actual es Edad > 18, por lo que 18 no es valido
        public void ValidarReglaNegocioMayorEdad_EdadMenorOIgualA18_RetornaFalse(int edad)
        {
            // Arrange
            var duenno = new Duenno { Edad = edad };

            // Act
            var resultado = duenno.ValidarReglaNegocioMayorEdad();

            // Assert
            Assert.False(resultado);
        }
    }
}
