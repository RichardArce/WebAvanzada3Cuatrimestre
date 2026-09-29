using System;
using System.Collections.Generic;
using System.Text;

namespace WebAvanzada3Cuatrimestre.Application.Dtos
{
    public class CarroDto
    {
        public int Id { get; set; }

        public string Placa { get; set; } = null!;

        public string Marca { get; set; } = null!;
    }
}
