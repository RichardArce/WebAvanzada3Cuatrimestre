using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Domain;


namespace WebAvanzada3Cuatrimestre.Application.Mapping
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            CreateMap<Duenno, Dtos.DuennoDto>().ReverseMap();
            CreateMap<Carro, Dtos.CarroDto>().ReverseMap();



        }
    }
}
