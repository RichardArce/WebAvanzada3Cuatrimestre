using System;
using System.Collections.Generic;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Entidades;

public partial class Carro
{
    public int Id { get; set; }

    public string Placa { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public int Fkduenno { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Duenno FkduennoNavigation { get; set; } = null!;
}
