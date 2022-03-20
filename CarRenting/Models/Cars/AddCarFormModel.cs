using System.ComponentModel.DataAnnotations;
using static CarRenting.Data.DataConstants.Car;

namespace CarRenting.Models.Cars
{
    public class AddCarFormModel
    {
        [Required]
        [StringLength(CarBrandMaxLength, MinimumLength = CarBrandMinLength, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Brand { get; init; }

        [Required]
        [StringLength(CarModelMaxLength, MinimumLength = CarModelMinLength, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Model { get; init; }

        [Required]
        [StringLength(CarDescriptionMaxLength, MinimumLength = CarDescriptionMinLength, ErrorMessage = "{0} must be between {2} and {1}")]
        public string Description { get; init; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        [Range(CarYearMinValue, CarYearMaxValue)]
        public int Year { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CarCategoryViewModel>? Categories { get; set; }
    }
}
