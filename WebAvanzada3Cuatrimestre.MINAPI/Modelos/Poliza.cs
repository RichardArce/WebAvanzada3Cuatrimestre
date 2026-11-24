namespace WebAvanzada3Cuatrimestre.MINAPI.Modelos
{
    public class Poliza
    {
        public string placa { get; set; }

        public string codigoPoliza { get; set; } //Tipo de poliza

        public string numeroPoliza { get; set; } //Identicador unico de la poliza

        public DateTime FechaContratacion { get; set; } //Fecha de contratacion de la poliza

        public DateTime FechaVencimiento { get; set; } //Fecha de vencimiento de la poliza
    }



    //Crearle poliza de seguro a un carro, si es propietario del mismo -> Recibo la placa del carro, la poliza que quiere contratar y la crea.

    //Obtener la lista de polizas que tiene un carro-> Recibo numero de placa y me devuelve la lista de polizas que tiene ese carro

    //Verificar vencimiento de la poliza de un carro -> Recibo numero de poliza y me devuelve si esta vencida o no
}
