﻿using FluentValidation;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Repositorios;

namespace MinimalAPIPeliculas.Validaciones
{
    public class CrearGeneroDTOValidador : AbstractValidator<CrearGeneroDTO>
    {
        public CrearGeneroDTOValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {

            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;
            
            if(valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje) //validad que el campo no sea nulo 
            .MaximumLength(50).WithMessage(Utilidades.MaximumLengthMensaje)
            .Must(Utilidades.PrimeraLetraMayusculas).WithMessage(Utilidades.PrimeraLetraMayusculaMensaje)
            .MustAsync(async (nombre, _) =>
            {
                var existe = await repositorioGeneros.Existe(id: id, nombre);
                return !existe;
            }).WithMessage(g => $"ya existe un genero con el nombre {g.Nombre}");
        }

       
    }
}
