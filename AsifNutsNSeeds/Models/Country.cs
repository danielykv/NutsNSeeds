using System.ComponentModel.DataAnnotations;

namespace AsifNutsNSeeds.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }

        public string ProfilePictureURL { get; set; }
        public  string CountryName  { get; set; }
        public  string CountryBio { get; set; }

        public List<Product> Products { get; set; }
    }
}
