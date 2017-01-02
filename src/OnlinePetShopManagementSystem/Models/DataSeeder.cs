
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Builder;
using OnlinePetShopManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlinePetShopManagementSystem.Models
{
    public static class DataSeeder
    {
        public static async void SeedData(this IApplicationBuilder app)
        {


            var db = app.ApplicationServices.GetService<ApplicationDbContext>();
            db.Database.Migrate();
            //RoleStore needs using Microsoft.AspNet.Identity.EntityFramework;
            var identityRoleStore = new RoleStore<IdentityRole>(db);
            var identityRoleManager = new RoleManager<IdentityRole>(identityRoleStore, null, null, null, null, null);

            var adminRole = new IdentityRole { Name = "ADMIN" };
            await identityRoleManager.CreateAsync(adminRole);



            var officerRole = new IdentityRole { Name = "OFFICER" };
            await identityRoleManager.CreateAsync(officerRole);




            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);


            var danielUser = new ApplicationUser { UserName = "88881", Email = "DANIEL@EMU.COM", FullName = "DANIEL" };
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            danielUser.PasswordHash = ph.HashPassword(danielUser, "P@ssw0rd"); //More complex password
            await userManager.CreateAsync(danielUser);

            await userManager.AddToRoleAsync(danielUser, "ADMIN");



            var susanUser = new ApplicationUser { UserName = "88882", Email = "SUSAN@EMU.COM", FullName = "SUSAN" };
            susanUser.PasswordHash = ph.HashPassword(susanUser, "P@ssw0rd"); //More complex password
            await userManager.CreateAsync(susanUser);

            await userManager.AddToRoleAsync(susanUser, "ADMIN");



            var randyUser = new ApplicationUser { UserName = "88883", Email = "RANDY@EMU.COM", FullName = "RANDY" };
            randyUser.PasswordHash = ph.HashPassword(randyUser, "P@ssw0rd"); //More complex password
            await userManager.CreateAsync(randyUser);

            await userManager.AddToRoleAsync(randyUser, "OFFICER");


            var thomasUser = new ApplicationUser { UserName = "88884", Email = "THOMAS@EMU.COM", FullName = "THOMAS" };
            thomasUser.PasswordHash = ph.HashPassword(thomasUser, "P@ssw0rd"); //More complex password

            await userManager.CreateAsync(thomasUser);
            await userManager.AddToRoleAsync(thomasUser, "OFFICER");

            var benUser = new ApplicationUser { UserName = "88885", Email = "BEN@EMU.COM", FullName = "BEN" };
            benUser.PasswordHash = ph.HashPassword(benUser, "P@ssw0rd"); //More complex password

            await userManager.CreateAsync(benUser);

            await userManager.AddToRoleAsync(benUser, "OFFICER");

            var gabrielUser = new ApplicationUser { UserName = "88886", Email = "GABRIEL@EMU.COM", FullName = "GABRIEL" };
            gabrielUser.PasswordHash = ph.HashPassword(gabrielUser, "P@ssw0rd"); //More complex password

            //Use the UserManager class instance, userManager
            //which manages the many-to-many AspNetUserRoles table
            //to create a user, Gabriel.
            await userManager.CreateAsync(gabrielUser); //Add the user
            //Use the UserManager class instance, userManager
            //which also manages the many-to-many AspNetUserRoles table
            //to create a relationship describing that, Gabriel user is an OFFICER role user
            await userManager.AddToRoleAsync(gabrielUser, "OFFICER");

            var fredUser = new ApplicationUser { UserName = "88887", Email = "FRED@EMU.COM", FullName = "FRED" };
            fredUser.PasswordHash = ph.HashPassword(fredUser, "P@ssw0rd"); //More complex password

            //Use the UserManager class instance, userManager
            //which manages the many-to-many AspNetUserRoles table
            //to create a user, Fred.
            await userManager.CreateAsync(fredUser); //Add the user
                                                     //Use the UserManager class instance, userManager
                                                     //which also manages the many-to-many AspNetUserRoles table
                                                     //to create a relationship describing that, Fred user is an OFFICER role user
            await userManager.AddToRoleAsync(fredUser, "OFFICER");



            //Add Category records into Category Table
            Category catFood, dogFood, treats, clearance;

            catFood = new Category()
            {
                CategoryName = "Cat Food",
                Visibility = 0,
                IsSpecial = false
            };

            dogFood = new Category()
            {
                CategoryName = "Dog Food",
                Visibility = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsSpecial = false
            };

            treats = new Category()
            {
                CategoryName = "Treats",
                Visibility = 2,
                IsSpecial = false
            };

            clearance = new Category()
            {
                CategoryName = "Clearance Sale",
                Visibility = 2,
                IsSpecial = true
            };

            db.Categories.Add(catFood);
            db.Categories.Add(dogFood);
            db.Categories.Add(treats);
            db.Categories.Add(clearance);



            //Add Brand records into Brand Table
            Brand addiction, avoderm, bosch, dailyDelight, justFish, nutripe;

            addiction = new Brand()
            {
                BrandName = "Addiction",
                NoOfProducts = 3
            };

            avoderm = new Brand()
            {
                BrandName = "Avoderm",
                NoOfProducts = 2
            };

            bosch = new Brand()
            {
                BrandName = "Bosch",
                NoOfProducts = 1
            };

            dailyDelight = new Brand()
            {
                BrandName = "Daily Delight",
                NoOfProducts = 1
            };

            justFish = new Brand()
            {
                BrandName = "Just Fish",
                NoOfProducts = 1
            };

            nutripe = new Brand()
            {
                BrandName = "Nutripe",
                NoOfProducts = 1
            };

            db.Brands.Add(addiction);
            db.Brands.Add(avoderm);
            db.Brands.Add(bosch);
            db.Brands.Add(dailyDelight);
            db.Brands.Add(justFish);
            db.Brands.Add(nutripe);

            CategoryBrand dogAddiction, dogAvoderm, dogNutripe, dogBosch, catAddiction, catAvoderm, catNutripe, catDailyDelight, treatsAddiction, treatsJustFish;

            dogAddiction = new CategoryBrand()
            {
                CategoryId = dogFood.CategoryId,
                BrandId = addiction.BrandId
            };

            dogAvoderm = new CategoryBrand()
            {
                CategoryId = dogFood.CategoryId,
                BrandId = avoderm.BrandId
            };

            dogNutripe = new CategoryBrand()
            {
                CategoryId = dogFood.CategoryId,
                BrandId = nutripe.BrandId
            };

            dogBosch = new CategoryBrand()
            {
                CategoryId = dogFood.CategoryId,
                BrandId = bosch.BrandId
            };

            catAddiction = new CategoryBrand()
            {
                CategoryId = catFood.CategoryId,
                BrandId = addiction.BrandId
            };

            catAvoderm = new CategoryBrand()
            {
                CategoryId = catFood.CategoryId,
                BrandId = avoderm.BrandId
            };

            catNutripe = new CategoryBrand()
            {
                CategoryId = catFood.CategoryId,
                BrandId = nutripe.BrandId
            };

            catDailyDelight = new CategoryBrand()
            {
                CategoryId = catFood.CategoryId,
                BrandId = dailyDelight.BrandId
            };

            treatsAddiction = new CategoryBrand()
            {
                CategoryId = treats.CategoryId,
                BrandId = addiction.BrandId
            };

            treatsJustFish = new CategoryBrand()
            {
                CategoryId = treats.CategoryId,
                BrandId = justFish.BrandId
            };

            db.CategoryBrands.Add(dogAddiction);
            db.CategoryBrands.Add(dogAvoderm);
            db.CategoryBrands.Add(dogNutripe);
            db.CategoryBrands.Add(dogBosch);
            db.CategoryBrands.Add(catAddiction);
            db.CategoryBrands.Add(catAvoderm);
            db.CategoryBrands.Add(catNutripe);
            db.CategoryBrands.Add(catDailyDelight);
            db.CategoryBrands.Add(treatsAddiction);
            db.CategoryBrands.Add(treatsJustFish);

            Product addiction1Product, addiction2Product, addiction3Product, avoderm1Product, avoderm2Product, bosch1Product, dailyDelight1Product, justFish1Product, nutripe1Product;

            addiction1Product = new Product()
            {
                ProductName = "Addiction Black Forest Rabbit & Blueberries",
                BrandId = addiction.BrandId
            };

            addiction2Product = new Product()
            {
                ProductName = "Addiction Cat Blackforest Rabbit & Blueberries Entree",
                BrandId = addiction.BrandId
            };

            addiction3Product = new Product()
            {
                ProductName = "Addiction Meaty Bites (Beef)",
                BrandId = addiction.BrandId
            };

            avoderm1Product = new Product()
            {
                ProductName = "AvoDerm Natural Chicken & Brown Rice",
                BrandId = avoderm.BrandId
            };

            avoderm2Product = new Product()
            {
                ProductName = "AvoDerm Natural Adult Cat Chicken & Herring Corn Free Formula",
                BrandId = avoderm.BrandId
            };

            bosch1Product = new Product() {
                ProductName = "Bosch High Premium Adult Fish & Potato",
                BrandId = bosch.BrandId
            };

            dailyDelight1Product = new Product()
            {
                ProductName = "Daily Delight Pure Skipjack Tuna White & Chicken With Baby Clam",
                BrandId = dailyDelight.BrandId
            };

            justFish1Product = new Product()
            {
                ProductName = "Just Fish Cod Biscuit",
                BrandId = justFish.BrandId
            };

            nutripe1Product = new Product()
            {
                ProductName = "Nutripe Cat Lamb With Green Tripe + GLM",
                BrandId = nutripe.BrandId
            };

            db.Products.Add(addiction1Product);
            db.Products.Add(addiction2Product);
            db.Products.Add(addiction3Product);
            db.Products.Add(avoderm1Product);
            db.Products.Add(avoderm2Product);
            db.Products.Add(bosch1Product);
            db.Products.Add(dailyDelight1Product);
            db.Products.Add(justFish1Product);
            db.Products.Add(nutripe1Product);

            CategoryProduct dogAddiction1Product, catAddiction2Product, treatsAddiction3Product, dogAvoderm1Product, catAvoderm2Product, dogBosch1Product, catDailyDelight1Product, treatsJustFish1Product, catNutripe1Product, clearanceNutripe1Product;

            dogAddiction1Product = new CategoryProduct()
            {
                CategoryId = dogFood.CategoryId,
                ProductId = addiction1Product.ProductId
            };

            catAddiction2Product = new CategoryProduct()
            {
                CategoryId = catFood.CategoryId,
                ProductId = addiction2Product.ProductId
            };

            treatsAddiction3Product = new CategoryProduct()
            {
                CategoryId = treats.CategoryId,
                ProductId = addiction3Product.ProductId
            };

            dogAvoderm1Product = new CategoryProduct()
            {
                CategoryId = dogFood.CategoryId,
                ProductId = avoderm1Product.ProductId
            };

            catAvoderm2Product = new CategoryProduct()
            {
                CategoryId = catFood.CategoryId,
                ProductId = avoderm2Product.ProductId
            };

            dogBosch1Product = new CategoryProduct()
            {
                CategoryId = dogFood.CategoryId,
                ProductId = bosch1Product.ProductId
            };

            catDailyDelight1Product = new CategoryProduct()
            {
                CategoryId = catFood.CategoryId,
                ProductId = dailyDelight1Product.ProductId
            };

            treatsJustFish1Product = new CategoryProduct()
            {
                CategoryId = treats.CategoryId,
                ProductId = justFish1Product.ProductId
            };

            catNutripe1Product = new CategoryProduct()
            {
                CategoryId = catFood.CategoryId,
                ProductId = nutripe1Product.ProductId
            };

            clearanceNutripe1Product = new CategoryProduct()
            {
                CategoryId = clearance.CategoryId,
                ProductId = nutripe1Product.ProductId
            };

            db.CategoryProducts.Add(dogAddiction1Product);
            db.CategoryProducts.Add(catAddiction2Product);
            db.CategoryProducts.Add(treatsAddiction3Product);
            db.CategoryProducts.Add(dogAvoderm1Product);
            db.CategoryProducts.Add(catAvoderm2Product);
            db.CategoryProducts.Add(dogBosch1Product);
            db.CategoryProducts.Add(catDailyDelight1Product);
            db.CategoryProducts.Add(treatsJustFish1Product);
            db.CategoryProducts.Add(catNutripe1Product);
            db.CategoryProducts.Add(clearanceNutripe1Product);



            db.SaveChanges();


            return;
				}//End of SeedData() method
		}
}
