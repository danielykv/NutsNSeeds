namespace AsifNutsNSeeds.Models
{
    public class Product_Branch
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int Id { get; set; }
        public Branch Branch { get; set; }
    }
}
