namespace WebAvanzada3Cuatrimestre.APICONSULTA.Services
{
    public class VerificarVehiculoService : IVerificarVehiculoService
    {
         
        private static IReadOnlyDictionary<string, string> PropietarioPorPlaca = new Dictionary<string, string>
        {
            ["111"] = "Carlos Rodríguez Méndez",
            ["222"] = "María Gómez Vargas",
            ["333"] = "José Antonio Ramírez López",
            ["444"] = "Ana Lucía Fernández Mora",
            ["555"] = "Luis Alberto Sánchez Castro",
            ["666"] = "Sofía Martínez Rojas",
            ["777"] = "Diego Vargas Hernández",
            ["888"] = "Laura Patricia Jiménez Ruiz",
            ["999"] = "Andrés Ramírez Solano",
            ["100"] = "Valeria Torres Mendoza",
            ["200"] = "Miguel Ángel Navarro Pérez",
            ["300"] = "Daniela Castro Villalobos",
            ["400"] = "Diego Torres Cordero"
        };



        public bool esPropietario(string cliente, string placa)
        {
            
            return PropietarioPorPlaca.TryGetValue(placa, out var propietario) && propietario == cliente;

           
        }
    }
}
