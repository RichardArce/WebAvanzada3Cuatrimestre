using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;
using WebAvanzada3Cuatrimestre.Infrastructure.Repository;

namespace WebAvanzada3Cuatrimestre.test
{
    // Pruebas de integracion del repositorio generico contra una base SQLite en memoria (mismo proveedor que la app).
    // Cada prueba recibe una base nueva; la conexion se mantiene abierta porque al cerrarla SQLite borra la base en memoria
    public class RepositoryTest : IDisposable
    {
        private readonly SqliteConnection _conexion;
        private readonly DbContextOptions<ApplicationDbContext> _opciones;

        public RepositoryTest()
        {
            _conexion = new SqliteConnection("Data Source=:memory:");
            _conexion.Open();

            _opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_conexion)
                .Options;

            using var contexto = CrearContexto();
            contexto.Database.EnsureCreated();
        }

        public void Dispose() => _conexion.Dispose();

        // Se usa un contexto nuevo por operacion, igual que en la app (un contexto por peticion HTTP)
        private ApplicationDbContext CrearContexto() => new(_opciones);

        private static Duenno NuevoDuenno(string nombre = "Juan") => new()
        {
            Nombre = nombre,
            Apellido1 = "Perez",
            Apellido2 = "Mora",
            Edad = 30
        };

        private async Task<Duenno> InsertarDuennoAsync(string nombre = "Juan")
        {
            using var contexto = CrearContexto();
            var duenno = NuevoDuenno(nombre);
            await new DuennoRepository(contexto).CreateAsync(duenno);
            return duenno;
        }

        private async Task<Carro> InsertarCarroAsync(int fkDuenno, string placa = "ABC-123")
        {
            using var contexto = CrearContexto();
            var carro = new Carro { Placa = placa, Marca = "Ferrari", Fkduenno = fkDuenno };
            await new CarroRepository(contexto).CreateAsync(carro);
            return carro;
        }

        // ---------- Create / GetById / GetAll ----------

        [Fact]
        public async Task CreateAsync_EntidadValida_GuardaYAsignaId()
        {
            // Arrange
            using var contexto = CrearContexto();
            var repositorio = new DuennoRepository(contexto);
            var duenno = NuevoDuenno();

            // Act
            var resultado = await repositorio.CreateAsync(duenno);

            // Assert
            Assert.True(resultado);
            Assert.True(duenno.Id > 0);
        }

        [Fact]
        public async Task GetByIdAsync_Existe_RetornaEntidadConFechaCreacionDeLaBd()
        {
            // Arrange
            var insertado = await InsertarDuennoAsync();
            using var contexto = CrearContexto();

            // Act
            var resultado = await new DuennoRepository(contexto).GetByIdAsync(insertado.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Juan", resultado.Nombre);
            Assert.NotNull(resultado.FechaCreacion); // La llena el DEFAULT CURRENT_TIMESTAMP
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public async Task GetByIdAsync_IdInvalidoOInexistente_RetornaNull(int id)
        {
            // Arrange
            using var contexto = CrearContexto();

            // Act
            var resultado = await new DuennoRepository(contexto).GetByIdAsync(id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task GetAllAsync_VariosRegistros_RetornaTodos()
        {
            // Arrange
            await InsertarDuennoAsync("Juan");
            await InsertarDuennoAsync("Ana");
            using var contexto = CrearContexto();

            // Act
            var resultado = await new DuennoRepository(contexto).GetAllAsync();

            // Assert
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public async Task GetAllAsync_LecturaSinSeguimiento_NoQuedaRastreadaEnElContexto()
        {
            // Arrange
            await InsertarDuennoAsync();
            using var contexto = CrearContexto();

            // Act
            await new DuennoRepository(contexto).GetAllAsync();

            // Assert
            Assert.Empty(contexto.ChangeTracker.Entries()); // Verifica el AsNoTracking de Consulta()
        }

        // ---------- Update ----------

        [Fact]
        public async Task UpdateAsync_Existe_ActualizaCamposYConservaFechaCreacion()
        {
            // Arrange
            var insertado = await InsertarDuennoAsync();
            DateTime? fechaOriginal;
            using (var contextoLectura = CrearContexto())
            {
                fechaOriginal = (await new DuennoRepository(contextoLectura).GetByIdAsync(insertado.Id))!.FechaCreacion;
            }

            // Simula lo que llega desde el DTO: sin FechaCreacion
            var cambios = new Duenno { Id = insertado.Id, Nombre = "Pedro", Apellido1 = "Rojas", Apellido2 = "Solis", Edad = 45, FechaCreacion = null };

            // Act
            bool resultado;
            using (var contexto = CrearContexto())
            {
                resultado = await new DuennoRepository(contexto).UpdateAsync(cambios);
            }

            // Assert
            Assert.True(resultado);
            using var contextoVerificacion = CrearContexto();
            var actualizado = await new DuennoRepository(contextoVerificacion).GetByIdAsync(insertado.Id);
            Assert.Equal("Pedro", actualizado!.Nombre);
            Assert.Equal(45, actualizado.Edad);
            Assert.NotNull(actualizado.FechaCreacion);
            Assert.Equal(fechaOriginal, actualizado.FechaCreacion);
        }

        [Fact]
        public async Task UpdateAsync_NoExiste_RetornaFalse()
        {
            // Arrange
            using var contexto = CrearContexto();
            var cambios = NuevoDuenno();
            cambios.Id = 999;

            // Act
            var resultado = await new DuennoRepository(contexto).UpdateAsync(cambios);

            // Assert
            Assert.False(resultado);
        }

        // ---------- Delete ----------

        [Fact]
        public async Task DeleteAsync_Existe_EliminaElRegistro()
        {
            // Arrange
            var insertado = await InsertarDuennoAsync();

            // Act
            bool resultado;
            using (var contexto = CrearContexto())
            {
                resultado = await new DuennoRepository(contexto).DeleteAsync(insertado.Id);
            }

            // Assert
            Assert.True(resultado);
            using var contextoVerificacion = CrearContexto();
            Assert.Null(await new DuennoRepository(contextoVerificacion).GetByIdAsync(insertado.Id));
        }

        [Fact]
        public async Task DeleteAsync_NoExiste_RetornaFalse()
        {
            // Arrange
            using var contexto = CrearContexto();

            // Act
            var resultado = await new DuennoRepository(contexto).DeleteAsync(999);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task DeleteAsync_DuennoConCarros_EliminaSusCarrosEnCascada()
        {
            // Arrange
            var duenno = await InsertarDuennoAsync();
            await InsertarCarroAsync(duenno.Id, "AAA-111");
            await InsertarCarroAsync(duenno.Id, "BBB-222");

            // Act
            using (var contexto = CrearContexto())
            {
                await new DuennoRepository(contexto).DeleteAsync(duenno.Id);
            }

            // Assert
            using var contextoVerificacion = CrearContexto();
            Assert.Empty(await new CarroRepository(contextoVerificacion).GetAllAsync());
        }

        // ---------- CarroRepository (override de Consulta y regresiones) ----------

        [Fact]
        public async Task CarroRepository_GetAllAsync_IncluyeAlDuenno()
        {
            // Arrange
            var duenno = await InsertarDuennoAsync("Ana");
            await InsertarCarroAsync(duenno.Id);
            using var contexto = CrearContexto();

            // Act
            var resultado = await new CarroRepository(contexto).GetAllAsync();

            // Assert
            var carro = Assert.Single(resultado);
            Assert.NotNull(carro.FkduennoNavigation);
            Assert.Equal("Ana", carro.FkduennoNavigation.Nombre);
        }

        [Fact]
        public async Task CarroRepository_GetByIdAsync_IncluyeAlDuenno()
        {
            // Arrange
            var duenno = await InsertarDuennoAsync("Ana");
            var carro = await InsertarCarroAsync(duenno.Id);
            using var contexto = CrearContexto();

            // Act
            var resultado = await new CarroRepository(contexto).GetByIdAsync(carro.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Ana", resultado.FkduennoNavigation.Nombre);
        }

        [Fact]
        public async Task CarroRepository_CreateAsync_GuardaElCarro() // Regresion: antes lanzaba NotImplementedException
        {
            // Arrange
            var duenno = await InsertarDuennoAsync();

            // Act
            var carro = await InsertarCarroAsync(duenno.Id);

            // Assert
            Assert.True(carro.Id > 0);
        }

        [Fact]
        public async Task CarroRepository_DeleteAsync_EliminaElCarro() // Regresion: antes FindAsync(id, token) lanzaba excepcion
        {
            // Arrange
            var duenno = await InsertarDuennoAsync();
            var carro = await InsertarCarroAsync(duenno.Id);

            // Act
            bool resultado;
            using (var contexto = CrearContexto())
            {
                resultado = await new CarroRepository(contexto).DeleteAsync(carro.Id);
            }

            // Assert
            Assert.True(resultado);
            using var contextoVerificacion = CrearContexto();
            Assert.Null(await new CarroRepository(contextoVerificacion).GetByIdAsync(carro.Id));
        }

        [Fact]
        public async Task CarroRepository_UpdateAsync_ActualizaPlacaMarcaYDuenno()
        {
            // Arrange
            var duenno1 = await InsertarDuennoAsync("Juan");
            var duenno2 = await InsertarDuennoAsync("Ana");
            var carro = await InsertarCarroAsync(duenno1.Id);
            var cambios = new Carro { Id = carro.Id, Placa = "NEW-999", Marca = "ferrari", Fkduenno = duenno2.Id };

            // Act
            bool resultado;
            using (var contexto = CrearContexto())
            {
                resultado = await new CarroRepository(contexto).UpdateAsync(cambios);
            }

            // Assert
            Assert.True(resultado);
            using var contextoVerificacion = CrearContexto();
            var actualizado = await new CarroRepository(contextoVerificacion).GetByIdAsync(carro.Id);
            Assert.Equal("NEW-999", actualizado!.Placa);
            Assert.Equal("Ana", actualizado.FkduennoNavigation.Nombre);
            Assert.NotNull(actualizado.FechaCreacion);
        }
    }
}
