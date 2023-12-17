using System.ComponentModel.DataAnnotations;
using MovieAPI.Domain.Common;

namespace MovieAPI.Domain.Entities {
    public class Movie : AuditEntity {

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public float Rating { get; set; }

        public string Image { get; set; }
    }
}
