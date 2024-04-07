using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Data.Enums;

namespace AsifNutsNSeeds.Models
{
    public class ProductNotification
    {
        public int Id { get; set; } // Primary key

        public int ProductId { get; set; } // Foreign key to Product entity
        public string UserId { get; set; } // Foreign key to ApplicationUser entity


      
    }

}
