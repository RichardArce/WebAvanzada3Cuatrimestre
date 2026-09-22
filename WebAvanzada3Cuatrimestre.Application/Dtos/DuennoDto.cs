using System;
using System.Collections.Generic;
using System.Text;

namespace WebAvanzada3Cuatrimestre.Application.Dtos
{
    public class DuennoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int Edad { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }
    }
}
