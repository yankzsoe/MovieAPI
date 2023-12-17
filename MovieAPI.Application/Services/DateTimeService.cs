using MovieAPI.Application.Interfaces;

namespace MovieAPI.Application.Services {
    public class DateTimeService : IDateTime {
        public DateTime Now => DateTime.Now;
    }
}
