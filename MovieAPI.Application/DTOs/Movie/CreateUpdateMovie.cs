namespace MovieAPI.Application.DTOs.Movie {
    public class CreateUpdateMovie {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
    }
}
