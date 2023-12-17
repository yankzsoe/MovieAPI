using MovieAPI.Application.Common.Models.Enums;

namespace MovieAPI.Application.Common.Models.Responses {
    public class PagedQuery {
        /// <summary>
        /// Default: Asc
        /// </summary>
        public SortBy SortBy { get; set; } = SortBy.Asc;

        /// <summary>
        /// Default: 1
        /// </summary>
        public int PageNumber {
            get => pageNumber < 1 ? 1 : pageNumber;
            set => pageNumber = value;
        }
        private int pageNumber { get; set; } = 1;

        /// <summary>
        /// Default: 10 
        /// 
        /// Min size: 1
        /// 
        /// Max size: 20
        /// </summary>
        public int PageSize {
            get => pageSize < 1 ? 1 : pageSize;
            set => pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
        private int pageSize { get; set; } = 10;
        private const int _maxPageSize = 20;
    }
}
