namespace MovieAPI.Application.Common.Models {
    public class MovieViewModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
