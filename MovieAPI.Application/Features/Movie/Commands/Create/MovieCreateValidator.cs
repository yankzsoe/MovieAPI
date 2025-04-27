using FluentValidation;
using FluentValidation.Results;
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

        public async Task<ValidationResult> ValidateRequest(CreateUpdateMovieDto dto) {
            var result = await this.ValidateAsync(dto);
            if (!result.IsValid) {
                throw new Common.Exceptions.ValidationException(result.Errors);
            }
            return result;
        }
    }
}
