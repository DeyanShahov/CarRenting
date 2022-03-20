using System.ComponentModel.DataAnnotations;
using static CarRenting.Data.DataConstants.Dealer;

namespace CarRenting.Data.Models
{
    public class Dealer
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(DealerNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DealerPhoneMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Car> Cars { get; init; } = new List<Car>();

       

    }
}
