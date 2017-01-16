
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
                CategoryName = "CAT FOOD",
                Visibility = 0,
                IsSpecial = false
            };

            dogFood = new Category()
            {
                CategoryName = "DOG FOOD",
                Visibility = 1,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(30),
                IsSpecial = false
            };

            treats = new Category()
            {
                CategoryName = "TREATS",
                Visibility = 2,
                IsSpecial = false
            };

            clearance = new Category()
            {
                CategoryName = "CLEARANCE SALE",
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
                BrandName = "ADDICTION",
                NoOfProducts = 3
            };

            avoderm = new Brand()
            {
                BrandName = "AVODERM",
                NoOfProducts = 2
            };

            bosch = new Brand()
            {
                BrandName = "BOSCH",
                NoOfProducts = 1
            };

            dailyDelight = new Brand()
            {
                BrandName = "DAILY DELIGHT",
                NoOfProducts = 1
            };

            justFish = new Brand()
            {
                BrandName = "JUST FISH",
                NoOfProducts = 1
            };

            nutripe = new Brand()
            {
                BrandName = "NUTRIPE",
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


            BrandPhoto addictionImg, avodermImg, boschImg, dailyDelightImg, justFishImg, nutripeImg;

            addictionImg = new BrandPhoto()
            {
                BrandId = addiction.BrandId,
                CreatedAt = DateTime.Now,
                Format = "jpg",
                Height = 179,
                ImageSize = 27080,
                PublicId = "Brands/hol86dfxhxehguhrorb2",
                SecureUrl = "https://res.cloudinary.com/chuabingquan/image/upload/v1483543661/Brands/hol86dfxhxehguhrorb2.jpg",
                Url = "http://res.cloudinary.com/chuabingquan/image/upload/v1483543661/Brands/hol86dfxhxehguhrorb2.jpg",
                Version = 1483543661,
                Width = 202
            };

            avodermImg = new BrandPhoto()
            {
                BrandId = avoderm.BrandId,
                CreatedAt = DateTime.Now,
                Format = "jpg",
                Height = 179,
                ImageSize = 24788,
                PublicId = "Brands/duwy4mfepo1a1slfpvfh",
                SecureUrl = "https://res.cloudinary.com/chuabingquan/image/upload/v1483543737/Brands/duwy4mfepo1a1slfpvfh.jpg",
                Url = "http://res.cloudinary.com/chuabingquan/image/upload/v1483543737/Brands/duwy4mfepo1a1slfpvfh.jpg",
                Version = 1483543737,
                Width = 202
            };

            boschImg = new BrandPhoto()
            {
                BrandId = bosch.BrandId,
                CreatedAt = DateTime.Now,
                Format = "jpg",
                Height = 179,
                ImageSize = 50562,
                PublicId = "Brands/tccedcpniw41cwyymvda",
                SecureUrl = "https://res.cloudinary.com/chuabingquan/image/upload/v1483543789/Brands/tccedcpniw41cwyymvda.jpg",
                Url = "http://res.cloudinary.com/chuabingquan/image/upload/v1483543789/Brands/tccedcpniw41cwyymvda.jpg",
                Version = 1483543789,
                Width = 202
            };

            dailyDelightImg = new BrandPhoto()
            {
                BrandId = dailyDelight.BrandId,
                CreatedAt = DateTime.Now,
                Format = "jpg",
                Height = 179,
                ImageSize = 34676,
                PublicId = "Brands/j6hbhiyoxd1uwjrxbe3x",
                SecureUrl = "https://res.cloudinary.com/chuabingquan/image/upload/v1483543817/Brands/j6hbhiyoxd1uwjrxbe3x.jpg",
                Url = "http://res.cloudinary.com/chuabingquan/image/upload/v1483543817/Brands/j6hbhiyoxd1uwjrxbe3x.jpg",
                Version = 1483543817,
                Width = 202
            };

            justFishImg = new BrandPhoto()
            {
                BrandId = justFish.BrandId,
                CreatedAt = DateTime.Now,
                Format = "jpg",
                Height = 179,
                ImageSize = 24595,
                PublicId = "Brands/uhgie5vzfq4mo7jfsqih",
                SecureUrl = "https://res.cloudinary.com/chuabingquan/image/upload/v1483543874/Brands/uhgie5vzfq4mo7jfsqih.jpg",
                Url = "http://res.cloudinary.com/chuabingquan/image/upload/v1483543874/Brands/uhgie5vzfq4mo7jfsqih.jpg",
                Version = 1483543874,
                Width = 202
            };

            nutripeImg = new BrandPhoto()
            {
                BrandId = nutripe.BrandId,
                CreatedAt = DateTime.Now,
                Format = "jpg",
                Height = 179,
                ImageSize = 30309,
                PublicId = "Brands/oxriz3emmoyr4crrhryg",
                SecureUrl = "https://res.cloudinary.com/chuabingquan/image/upload/v1483543912/Brands/oxriz3emmoyr4crrhryg.jpg",
                Url = "http://res.cloudinary.com/chuabingquan/image/upload/v1483543912/Brands/oxriz3emmoyr4crrhryg.jpg",
                Version = 1483543912,
                Width = 202
            };


            db.BrandPhotos.Add(addictionImg);
            db.BrandPhotos.Add(avodermImg);
            db.BrandPhotos.Add(boschImg);
            db.BrandPhotos.Add(dailyDelightImg);
            db.BrandPhotos.Add(justFishImg);
            db.BrandPhotos.Add(nutripeImg);

            db.SaveChanges();


            return;
				}//End of SeedData() method
		}
}
