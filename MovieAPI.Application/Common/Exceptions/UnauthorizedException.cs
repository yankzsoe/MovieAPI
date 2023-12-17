namespace MovieAPI.Application.Common.Exceptions {
    public class UnauthorizedException : Exception {
        public UnauthorizedException() : base("Unauthorized: Access is denied.") {
            Error = string.Empty;
        }

        public UnauthorizedException(string message) : this() {
            Error = message;
        }

        public string Error { get; }
    }
}
