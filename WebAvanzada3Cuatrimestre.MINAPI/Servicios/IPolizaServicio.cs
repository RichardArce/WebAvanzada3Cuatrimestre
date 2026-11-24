using WebAvanzada3Cuatrimestre.MINAPI.Modelos;

namespace WebAvanzada3Cuatrimestre.MINAPI.Servicios;

public interface IPolizaServicio
{
    bool ExistePlaca(string placa);

    bool ExisteNumeroPoliza(string numeroPoliza);

    bool Agregar(Poliza poliza);

    IReadOnlyList<Poliza> ObtenerPorPlaca(string placa);

    Poliza? ObtenerPorNumero(string numeroPoliza);
}
