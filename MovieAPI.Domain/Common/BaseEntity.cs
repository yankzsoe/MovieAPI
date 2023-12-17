using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Domain.Common {
    public class BaseEntity {
        [Required]
        public int Id { get; set; }
    }
}
