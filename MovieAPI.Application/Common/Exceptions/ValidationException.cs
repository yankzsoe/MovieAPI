using FluentValidation.Results;

namespace MovieAPI.Application.Common.Exceptions {
    public class ValidationException : Exception {
        public ValidationException() : base("Validation failures have occurred.") {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this() {
            foreach (var failure in failures) {
                Errors.Add(failure.ErrorMessage);
            }
        }

        public ValidationException(string message) : this() {
            Errors.Add(message);
        }

        public List<string> Errors { get; }
    }
}
