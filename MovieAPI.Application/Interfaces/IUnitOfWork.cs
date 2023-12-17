using MovieAPI.Application.Interfaces.Repositories;

namespace MovieAPI.Application.Interfaces {
    public interface IUnitOfWork : IAsyncDisposable {
        IMovieRepository Movie { get; }

        Task<int> CompleteAsync();
    }
}
