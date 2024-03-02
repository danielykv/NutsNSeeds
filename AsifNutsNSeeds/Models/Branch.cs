using System.ComponentModel.DataAnnotations;
using AsifNutsNSeeds.Data.Base;

namespace AsifNutsNSeeds.Models
{
    public class Branch: IEntityBase
	{
        [Key]
        public int Id { get; set; }   

        [Display(Name = "Branch Picture URL")]
        [Required(ErrorMessage ="Logo picture is required")]
        public string Logo { get; set; }
		[Display(Name = "Branch Name ")]
        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(50,MinimumLength = 3 ,ErrorMessage = "Full name must be 3-50 characters")]

        public string BranchName { get; set; }

		[Display(Name = "Branch Description")]
        [Required(ErrorMessage = "Branch Description is required")]

        public string BranchDescription { get; set; }

        public List<Product_Branch>? Product_Branches{ get; set; }
    }
}
