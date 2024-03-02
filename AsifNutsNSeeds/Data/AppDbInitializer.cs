using AsifNutsNSeeds.Data.Enums;
using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data
{
	public class AppDbInitializer
	{

		public static void Seed(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context =  serviceScope.ServiceProvider.GetService<AppDbContext>();

				context.Database.EnsureCreated();

				//Countries
				if (!context.Countries.Any())
				{
					context.Countries.AddRange(new List<Country>()
						{
							new Country()
							{
								CountryName = "Guatemala",
								ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/Flag_of_Guatemala.svg/1200px-Flag_of_Guatemala.svg.png",
								CountryBio = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks."
							},

							new Country()
							{
								CountryName = "Colombia",
								ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/21/Flag_of_Colombia.svg/800px-Flag_of_Colombia.svg.png",
								CountryBio = "Situated in the bustling city of Ofakim, this branch boasts an extensive range of high-quality nuts and seeds. Its friendly staff and cozy atmosphere make it a favorite among locals.\r\n"
							},

							new Country()
							{
								CountryName = "Argentina",
								ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1a/Flag_of_Argentina.svg/2560px-Flag_of_Argentina.svg.png",
								CountryBio = "Located near the beautiful coastline of Ashkelon, this branch offers a diverse selection of nuts and seeds. It’s the perfect place to grab a healthy snack after a day at the beach.\r\n"
							},


						});

					context.SaveChanges();

				}

				//Branches
				if (!context.Branches.Any())
				{
					context.Branches.AddRange(new List<Branch>()
						{
							new Branch()
							{
								BranchName = "Netivot",
								Logo = "",
								BranchDescription = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks."
							},

							new Branch()
							{
								BranchName = "Ofakim",
								Logo = "",
								BranchDescription = "Situated in the bustling city of Ofakim, this branch boasts an extensive range of high-quality nuts and seeds. Its friendly staff and cozy atmosphere make it a favorite among locals.\r\n"
							},

							new Branch()
							{
								BranchName = "Ashkelon",
								Logo = "",
								BranchDescription = "Located near the beautiful coastline of Ashkelon, this branch offers a diverse selection of nuts and seeds. It’s the perfect place to grab a healthy snack after a day at the beach.\r\n"
							},


						});

					context.SaveChanges();
				}

				//Producers
				if (!context.Producers.Any())
				{
					context.Producers.AddRange(new List<Producer>()
						{
							new Producer()
							{
								ProducerName = "Dani's Nuts and Seeds",
								ProfilePictureURL = "https://jerusalem.mynet.co.il/picserver/mynet/wcm_upload/wcm_mynet_pic/2019/01/09/432212/432212.jpg",
								ProducerBio = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks."
							},

							new Producer()
							{
								ProducerName = "Hamama Seeds",
								ProfilePictureURL = "https://hamamanuts.co.il/cdn/shop/files/copyof_Logo_Hamama_Nuts_Online.png?v=1680510380&width=600",
								ProducerBio = "Situated in the bustling city of Ofakim, this branch boasts an extensive range of high-quality nuts and seeds. Its friendly staff and cozy atmosphere make it a favorite among locals.\r\n"
							},

							new Producer()
							{
								ProducerName = "Nuts and Seeds Champion",
								ProfilePictureURL = "https://aluf-hapizuhim.co.il/wp-content/uploads/2021/06/%D7%9C%D7%95%D7%92%D7%95.png",
								ProducerBio = "Located near the beautiful coastline of Ashkelon, this branch offers a diverse selection of nuts and seeds. It’s the perfect place to grab a healthy snack after a day at the beach.\r\n"
							},


						});

					context.SaveChanges();
				}
				
				//Products
				if (!context.Products.Any())
				{
					context.Products.AddRange(new List<Product>()
						{
							new Product()
							{
								ProductName = "Guatemala",
								ProductDescription = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks.",
								ProductPrice = 20,
								ImageURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/Flag_of_Guatemala.svg/1200px-Flag_of_Guatemala.svg.png",
								CountryID = 1,
								ProducerID = 1,
								productCategory = ProductCategory.NutsAndSeeds


							},

							new Product()
							{
								ProductName = "Matok",
								ProductDescription = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks.",
								ProductPrice = 20,
								ImageURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/Flag_of_Guatemala.svg/1200px-Flag_of_Guatemala.svg.png",
								CountryID = 3,
								ProducerID = 1,
								productCategory = ProductCategory.Sweets
							},

							new Product()
							{
								ProductName = "Camon",
								ProductDescription = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks.",
								ProductPrice = 20,
								ImageURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/Flag_of_Guatemala.svg/1200px-Flag_of_Guatemala.svg.png",
								CountryID = 2,
								ProducerID = 2,
								productCategory = ProductCategory.Spices
							},


						});

					context.SaveChanges();
				}

				//Product & Branches
				if (!context.Product_Branches.Any())
				{
					context.Product_Branches.AddRange(new List<Product_Branch>()
				{
					new Product_Branch()
					{
						ProductID = 1,
						Id = 1,
					},
					new Product_Branch()
					{
						ProductID = 2,
						Id = 2,
					},
					new Product_Branch()
					{
						ProductID = 3,
						Id = 3,
					},

				});
					context.SaveChanges();

				}



			}



		}

	}
}
