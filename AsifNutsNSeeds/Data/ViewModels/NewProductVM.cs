using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Base;
using System.ComponentModel.DataAnnotations;
using AsifNutsNSeeds.Data.Enums;


namespace AsifNutsNSeeds.Data
{
    public class NewProductVM
    {
        public int Id { get; set; }

        [Display(Name = "Product name")]
        [Required(ErrorMessage = "Name is required")]
        public string ProductName { get; set; }

        [Display(Name = "Product description")]
        [Required(ErrorMessage = "Description is required")]
        public string ProductDescription { get; set; }

        [Display(Name = "Price in $")]
        [Required(ErrorMessage = "Price is required")]
        public double ProductPrice { get; set; }

        [Display(Name = "Product image URL")]
        [Required(ErrorMessage = "Product image URL is required")]
        public string ImageURL { get; set; }

        [Display(Name = "Select a category")]
        [Required(ErrorMessage = "Product category is required")]
        public ProductCategory ProductCategory { get; set; }

        //Relationships
        [Display(Name = "Select Branch(es)")]
        [Required(ErrorMessage = "Product branch(es) is required")]
        public List<int> BranchIds { get; set; }

        [Display(Name = "Select a Country")]
        [Required(ErrorMessage = "Product country is required")]
        public int CountryID { get; set; }

        [Display(Name = "Select a producer")]
        [Required(ErrorMessage = "Product producer is required")]
        public int ProducerID { get; set; }

    }
}
