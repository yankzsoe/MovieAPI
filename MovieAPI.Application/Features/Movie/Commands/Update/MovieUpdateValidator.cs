using FluentValidation;
using FluentValidation.Results;
using MovieAPI.Application.DTOs.Movie;

namespace MovieAPI.Application.Features.Movie.Commands.Update {
    public class MovieUpdateValidator : AbstractValidator<(int id, CreateUpdateMovieDto dto)> {
        public MovieUpdateValidator() {
            RuleFor(m => m.Item1)
                .NotEmpty()
                .WithMessage("Id is requred");

            RuleFor(m => m.Item2.Title)
                .NotEmpty()
                .WithMessage("Tite is requred");

            RuleFor(m => m.Item2.Description)
                .NotEmpty()
                .WithMessage("Description is requred");
        }

        public async Task<ValidationResult> ValidateRequest(int id, CreateUpdateMovieDto dto) {
            var result = await this.ValidateAsync((id, dto));
            if (!result.IsValid) {
                throw new Common.Exceptions.ValidationException(result.Errors);
            }
            return result;
        }
    }
}
