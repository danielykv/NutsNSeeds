using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AsifNutsNSeeds.Models
{
	public class ApplicationUser:IdentityUser
	{
		[Display(Name="Full name")]
        public string Fullname { get; set; }


        [Display(Name = "Address")]

        public string Address { get; set; }
        [Display(Name = "City")]

        public string City { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

    }
}
