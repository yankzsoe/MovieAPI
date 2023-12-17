namespace MovieAPI.Application.Common.Models.Responses {
    public class PagedResponse<T> : Response<T> {
        public Pagination Pagination { get; set; }

        public PagedResponse(T data
            , int totalCount, int pageSize, int pageNumber
            , string message) {
            Succeeded = true;
            Message = message;
            Data = data;

            Pagination = new Pagination {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            Pagination.PreviousPage = Pagination.CurrentPage > 1;
            Pagination.NextPage = Pagination.CurrentPage < Pagination.TotalPages;
        }
    }
}
