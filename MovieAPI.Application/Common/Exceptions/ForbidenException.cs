namespace MovieAPI.Application.Common.Exceptions {
    public class ForbiddenException : Exception {
        public ForbiddenException() : base("Forbidden: Access is denied.") {
            Error = string.Empty;
        }

        public ForbiddenException(string message) : this() {
            Error = message;
        }

        public string Error { get; }
    }
}
