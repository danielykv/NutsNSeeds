using AsifNutsNSeeds.Data.Enums;
using AsifNutsNSeeds.Data.Static;
using AsifNutsNSeeds.Models;
using Microsoft.AspNetCore.Identity;

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

                            new Country()
                            {
                                CountryName = "Brazil",
                                ProfilePictureURL = "https://th.bing.com/th/id/R.1c50cfe9aa6fa2ea3f17270e276b66b0?rik=LjXzIFc%2bx1PLUA&pid=ImgRaw&r=0",
                                CountryBio = "Native to the Amazon rainforest, Brazil nuts grow on tall trees in countries like Brazil, Peru, and Bolivia. Harvesting: Gatherers collect fallen fruits during the rainy season, cracking open the hard shells to reveal the edible seeds. Nutritional Value: Brazil nuts are rich in selenium and healthy fats. Culinary Uses: Enjoyed as a snack or used in recipes, they add a unique flavor and crunch. Fun Fact: The world’s largest Brazil nut tree covers an area of 7,500 square meters in Natal, Brazil! 🌴🌰\r\n"
                            },

                             new Country()
                            {
                                CountryName = "Bolivia",
                                ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/48/Flag_of_Bolivia.svg/1280px-Flag_of_Bolivia.svg.png",
                                CountryBio = "Brazil Nuts (Actually Bolivia Nuts): These seeds, often called Brazil nuts\r\n"
                            },

                            new Country()
                            {
                                CountryName = "Mexico",
                                ProfilePictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Flag_of_Mexico.svg/1920px-Flag_of_Mexico.svg.png",
                                CountryBio = "While Mexico is known for its vibrant cuisine, its indigenous cultures have long appreciated nuts and seeds.\r\n"
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
								Logo = "https://th.bing.com/th/id/OIP.O_ij5xfdd47P4qkcXUPfywHaEb?rs=1&pid=ImgDetMain",
								BranchDescription = "Our Netivot branch is a haven for health enthusiasts, offering a wide variety of nuts and seeds. Located in the heart of the city, it’s a convenient stop for your daily dose of nutritious snacks."
							},

							new Branch()
							{
								BranchName = "Ofakim",
								Logo = "https://www.claude.co.il/Pictures/20220426144118.551.jpg",
								BranchDescription = "Situated in the bustling city of Ofakim, this branch boasts an extensive range of high-quality nuts and seeds. Its friendly staff and cozy atmosphere make it a favorite among locals.\r\n"
							},

							new Branch()
							{
								BranchName = "Ashkelon",
								Logo = "https://www.korenvs.com/wp-content/uploads/2022/01/151-thumb.jpg",
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
								ProductName = "Cashew",
								ProductDescription = "Cashew nuts, derived from the cashew apple, are versatile and nutritious. They’re rich in healthy fats and protein.",
								ProductPrice = 20,
								ImageURL = "https://hamamanuts.co.il/cdn/shop/products/fdd39d92-ca17-4341-8eca-e8286d69e472.jpg?v=1681655158&width=713",
								CountryID = 4,
								ProducerID = 1,
                                Sold = 2,
                                Stock = 3,
                                productCategory = ProductCategory.NutsAndSeeds


							},

							new Product()
							{
								ProductName = "Peanuts",
								ProductDescription = "Peanuts are a nutritious legume. They are high in protein, healthy fats, and various nutrients.",
								ProductPrice = 20,
								ImageURL = "https://hamamanuts.co.il/cdn/shop/products/baf198ef-1489-4d4e-8338-8968c86b92f4.jpg?v=1681655423&width=493",
								CountryID = 5,
								ProducerID = 1,
								Sold = 2,
								Stock = 3,
								productCategory = ProductCategory.NutsAndSeeds
                            },

							new Product()
							{
								ProductName = "Walnut",
								ProductDescription = "Walnuts are nutritious tree nuts that are rich in healthy fats, antioxidants, and essential vitamins and minerals.",
								ProductPrice = 20,
								ImageURL = "https://hamamanuts.co.il/cdn/shop/products/2023-04-17_-13.09.08.png?v=1681726285&width=713",
								CountryID = 4,
								ProducerID = 2,
                                Sold = 2,
                                Stock = 3,
                                productCategory = ProductCategory.NutsAndSeeds
                            },

                            new Product()
                            {
                                ProductName = "Pecan",
                                ProductDescription = "Pecans are delicious tree nuts known for their rich, buttery flavor and versatility in both sweet and savory dishes. ",
                                ProductPrice = 20,
                                ImageURL = "https://hamamanuts.co.il/cdn/shop/products/2023-04-17_-13.13.50.png?v=1681726462&width=713",
                                CountryID = 4,
                                ProducerID = 2,
                                Sold = 2,
                                Stock = 3,
                                productCategory = ProductCategory.NutsAndSeeds
                            },

         
                            new Product()
                            {
                                ProductName = "Almond",
                                ProductDescription = "Almonds are nutritious seeds from the fruit of an almond tree. They are rich in healthy fats, protein, and essential vitamins and minerals.",
                                ProductPrice = 20,
                                ImageURL = "https://hamamanuts.co.il/cdn/shop/products/2023-04-17_-13.15.22.png?v=1681726547&width=713",
                                CountryID = 4,
                                ProducerID = 2,
                                Sold = 2,
                                Stock = 3,
                                productCategory = ProductCategory.NutsAndSeeds
                            },

                             new Product()
                            {
                                ProductName = "Sunflower seed",
                                ProductDescription = "Sunflower seeds are rich in healthy fats, protein, fiber, and essential nutrients such as vitamin E and selenium.",
                                ProductPrice = 20,
                                ImageURL = "https://hamamanuts.co.il/cdn/shop/products/ae7ebedf-54da-4b9e-847c-10f8571d8581.webp?v=1681659679&width=713",
                                CountryID = 4,
                                ProducerID = 2,
                                Sold = 2,
                                Stock = 3,
                                productCategory = ProductCategory.NutsAndSeeds
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

		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				// Roles Section
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.User))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

				// Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var adminUserEmail = "admin@AsifNutsNSeeds.com";
				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new ApplicationUser()
					{
						Fullname = "Admin user",
						UserName = "admin-user",
						Email = adminUserEmail,
						Address = "Leah 47/2",
						City = "Beer Sheva",
						PostalCode = "8452738",

                        EmailConfirmed = true

					};
					await userManager.CreateAsync(newAdminUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}

				var appUserEmail = "user@AsifNutsNSeeds.com";
				var appUser = await userManager.FindByEmailAsync(appUserEmail);
				if (appUser == null)
				{
					var newAppUser = new ApplicationUser()
					{
						Fullname = "Application user",
						UserName = "app-user",
						Email = appUserEmail,
                        Address = "dfgdf",
                        City = "dfgd",
                        PostalCode = "dfg",
                        EmailConfirmed = true

					};
					await userManager.CreateAsync(newAppUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
				}
			}


		}

	}
}
