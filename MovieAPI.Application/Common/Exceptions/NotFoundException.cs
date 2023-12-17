namespace MovieAPI.Application.Common.Exceptions {
    public class NotFoundException : Exception {
        public NotFoundException() : base("Data is not found.") {
            Error = string.Empty;
        }

        public NotFoundException(string message) : this() {
            Error = message;
        }

        public string Error { get; }
    }
}
