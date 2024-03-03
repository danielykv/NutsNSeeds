using AsifNutsNSeeds.Controllers;
using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data.ViewModels
{
    public class NewProductDropdownsVM
    {
        public NewProductDropdownsVM()
        {
            Producers = new List<Producer>();
            Countries = new List<Country>();
            Branches = new List<Branch>();
        }

        public List<Producer> Producers { get; set; }
        public List<Country> Countries { get; set; }
        public List<Branch> Branches { get; set; }
    }
}
