using System.ComponentModel.DataAnnotations;

namespace AsifNutsNSeeds.Models
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        public string Logo { get; set; }
        public string BranchName { get; set; }
        public string BranchDescription { get; set; }

        public List<Product_Branch> Product_Branches{ get; set; }
    }
}
