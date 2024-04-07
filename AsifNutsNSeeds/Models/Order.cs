using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsifNutsNSeeds.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }
        public DateTime OrderDate { get; set; } // Add this property for OrderDate


        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
