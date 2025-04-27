using MovieAPI.Application.Common.Models;

namespace MovieAPI.Application.Interfaces {
    public interface IEmailService {
        Task SendEmailAsync(Email email);
    }
}
