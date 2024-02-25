using System.ComponentModel.DataAnnotations;

namespace AsifNutsNSeeds.Models
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }   

        [Display(Name = "Branch Picture URL")]
        public string Logo { get; set; }
		[Display(Name = "Branch Name ")]

		public string BranchName { get; set; }

		[Display(Name = "Branch Description")]
		public string BranchDescription { get; set; }

        public List<Product_Branch> Product_Branches{ get; set; }
    }
}
