using FluentValidation;

namespace MovieAPI.Application.Features.Movie.Commands.Update {
    public class MovieUpdateValidator : AbstractValidator<MovieUpdateCommand> {
        public MovieUpdateValidator() {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage("Id is requred");

            RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage("Tite is requred");

            RuleFor(m => m.Description)
                .NotEmpty()
                .WithMessage("Description is requred");
        }
    }
}
