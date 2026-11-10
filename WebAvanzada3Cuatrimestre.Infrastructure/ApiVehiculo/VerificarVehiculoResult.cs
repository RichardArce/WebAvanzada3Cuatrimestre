using System;
using System.Collections.Generic;
using System.Text;

namespace WebAvanzada3Cuatrimestre.Infrastructure.ApiVehiculo
{
    public class VerificarVehiculoResult
    {
        public int statusCode { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }
}
