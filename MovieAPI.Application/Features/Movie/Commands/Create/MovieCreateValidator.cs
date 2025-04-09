using FluentValidation;
using MovieAPI.Application.DTOs.Movie;

namespace MovieAPI.Application.Features.Movie.Commands.Create {
    public class MovieCreateValidator : AbstractValidator<CreateUpdateMovieDto> {
        public MovieCreateValidator() {
            RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage("Tite is requred");

            RuleFor(m => m.Description)
                .NotEmpty()
                .WithMessage("Description is requred");
        }
    }
}
