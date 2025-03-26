using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validaciones
{
    public class CrearActorDTOValidador : AbstractValidator<CrearActorDTO>
    {
        public CrearActorDTOValidador()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoRequeridoMensaje) //validad que el campo no sea nulo 
           .MaximumLength(50).WithMessage(Utilidades.MaximumLengthMensaje)
           .Must(Utilidades.PrimeraLetraMayusculas).WithMessage(Utilidades.PrimeraLetraMayusculaMensaje);

            var fechaMinima = new DateTime(1900, 1, 1);
            RuleFor(x => x.FechaNacimiento)
                .GreaterThanOrEqualTo(fechaMinima)
                .WithMessage(Utilidades.GreaterThanEqualToMensaje(fechaMinima));
        }

       
    }
}
