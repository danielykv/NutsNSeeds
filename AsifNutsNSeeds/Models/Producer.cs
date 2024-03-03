using System.ComponentModel.DataAnnotations;
using AsifNutsNSeeds.Data.Base;

namespace AsifNutsNSeeds.Models
{
    public class Producer: IEntityBase
    {
        [Key]
        public int Id { get; set; }

		[Display(Name = "Profile Picture")]
		[Required(ErrorMessage = "Profile Picture is required")]
		public string ProfilePictureURL { get; set; }

		[Display(Name = "Full Name")]
		[Required(ErrorMessage = "Full Name is required")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 chars")]
		public string ProducerName { get; set; }

		[Display(Name = "Biography")]
		[Required(ErrorMessage = "Biography is required")] 
		public string ProducerBio { get; set; }

        public List<Product>? Products { get; set; }
    }
}
