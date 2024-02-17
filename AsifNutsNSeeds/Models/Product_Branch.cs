namespace AsifNutsNSeeds.Models
{
    public class Product_Branch
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int BranchID { get; set; }
        public Branch Branch { get; set; }
    }
}
