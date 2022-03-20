namespace CarRenting.Data
{
    public class DataConstants
    {
        public class Car
        {
            public const int CarBrandMinLength = 2;
            public const int CarBrandMaxLength = 20;
            public const int CarModelMinLength = 2;
            public const int CarModelMaxLength = 30;
            public const int CarDescriptionMinLength = 10;
            public const int CarDescriptionMaxLength = 400;
            public const int CarYearMinValue = 2000;
            public const int CarYearMaxValue = 2050;
        }
        
        public class Dealer
        {
            public const int DealerNameMinLength = 2;
            public const int DealerNameMaxLength = 30;
            public const int DealerPhoneMinLength = 6;
            public const int DealerPhoneMaxLength = 30;
        }

        public class Category
        {
            public const int CategoryNameMaxLength = 30;
        }
        
    }
}
