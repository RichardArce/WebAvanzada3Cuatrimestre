using System;
using System.Collections.Generic;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Entidades;

public partial class Duenno
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Carro> Carros { get; set; } = new List<Carro>();
}
