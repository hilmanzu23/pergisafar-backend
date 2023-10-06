using Sieve.Attributes;

namespace pergisafar.Shared.Models
{
    public class BaseModel
    {
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsVerification { get; set; }
        public bool? IsActive { get; set; }
    }
}
