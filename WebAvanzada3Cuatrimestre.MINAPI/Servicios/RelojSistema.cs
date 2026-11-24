namespace WebAvanzada3Cuatrimestre.MINAPI.Servicios;

public sealed class RelojSistema : IReloj
{
    public DateTime Hoy => DateTime.Now.Date;
}
