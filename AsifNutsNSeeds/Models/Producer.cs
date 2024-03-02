using System.ComponentModel.DataAnnotations;

namespace AsifNutsNSeeds.Models
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }

        public string ProfilePictureURL { get; set; }
        public string ProducerName { get; set; }
        public string ProducerBio { get; set; }

        public List<Product> Products { get; set; }
    }
}
