using System.ComponentModel.DataAnnotations;
using AsifNutsNSeeds.Data.Base;

namespace AsifNutsNSeeds.Models
{
    public class Country : IEntityBase
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Country Picture")]
		[Required(ErrorMessage = "Country Picture is required")]
		public string ProfilePictureURL { get; set; }
		[Display(Name = "Country Name")]
		[Required(ErrorMessage = "Country Name is required")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Country Name must be between 3 and 50 chars")]
		public  string CountryName  { get; set; }

		[Display(Name = "Biography")]
		[Required(ErrorMessage = "Biography is required")]
		public  string CountryBio { get; set; }

        public List<Product>? Products { get; set; }
    }
}
