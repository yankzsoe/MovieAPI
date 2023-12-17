namespace MovieAPI.Application.Common.Models.Responses {
    public class Pagination {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
