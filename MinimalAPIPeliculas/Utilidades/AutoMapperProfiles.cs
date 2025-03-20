using AutoMapper;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        protected AutoMapperProfiles()
        {
            CreateMap<CrearGeneroDTO, Genero>();
            CreateMap<GeneroDTO, Genero>().ReverseMap();

            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<CrearActorDTO, Actor>()
                    .ForMember(x => x.Foto, opciones => opciones.Ignore());

           
        }
    }
}
