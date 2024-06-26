﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Data.Enums;

namespace AsifNutsNSeeds.Models
{
    public class Product:IEntityBase
    {
        [Key]
        public int Id  { get; set; }

        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }

        public string ImageURL { get; set; }

        public ProductCategory productCategory { get; set; }
        
        //Relationshops

        // Branch - many to many
        public List<Product_Branch> Product_Branches { get; set; }

        //Country - one to many

        public int CountryID { get; set; }
        [ForeignKey("CountryID")]
        public Country Country { get; set; }

        //Producer - one to many

        public int ProducerID { get; set; }
        [ForeignKey("ProducerID")]

        public Producer Producer { get; set; }
        public int Stock { get;set; }
        public int Sold { get; set; }
    }
}
