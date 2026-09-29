using System;
using System.Collections.Generic;

namespace WebAvanzada3Cuatrimestre.Domain;

public partial class Carro
{
    public const string MarcaPermita = "Ferrari";


    public int Id { get; set; }

    public string Placa { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public int Fkduenno { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Duenno FkduennoNavigation { get; set; } = null!;


    public bool ValidarReglaNegocioSoloFerrari()
    {
        return string.Equals(Marca, MarcaPermita, StringComparison.OrdinalIgnoreCase);
    }

    public bool ValidarReglaNegocioCarrosNuevos()
    {
        if (FechaCreacion.HasValue && FechaCreacion.Value.Date == DateTime.Now.Date)
        {
            return true;
        }
        return false;
    }
    //Soluciona el caso de muchas reglas de negocio, si alguna falla, no se cumple la validacion
    public bool ValidarReglasDeNegocio()
    {
        return ValidarReglaNegocioSoloFerrari() 
            && ValidarReglaNegocioCarrosNuevos();
    }
}
