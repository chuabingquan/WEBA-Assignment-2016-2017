using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlinePetShopManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlinePetShopManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //Category, Brand, CategoryBrand, Product, CategoryProduct, BrandPhotos, ProductPhotos DbSet properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CategoryBrand> CategoryBrands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<BrandPhoto> BrandPhotos { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }

        //Others
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePhoto> EmployeePhotos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
						optionsBuilder.UseSqlServer(@"Server=DESKTOP-P2TEUPQ;Database=OnlinePetStoreSystem_WEBA;Trusted_Connection=True;MultipleActiveResultSets=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Reference: http://benjii.me/2016/05/dotnet-ef-migrations-for-asp-net-core/
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
            //----------- Defining Employee Entity - Start --------------
            //Make the EmployeeId a  Primary Key and Identity column
            modelBuilder.Entity<Employee>()
              .HasKey(input => input.EmployeeId)
              .HasName("PrimaryKey_EmployeeId");

            //Tell the modelBuilder more about the EmployeeId column
            modelBuilder.Entity<Employee>()
             .Property(input => input.EmployeeId)
             .HasColumnName("EmployeeId")
             .HasColumnType("int")
             .UseSqlServerIdentityColumn()
             .ValueGeneratedOnAdd()
             .IsRequired();

            //Tell the modelBuilder more about the FullName column
            modelBuilder.Entity<Employee>()
             .Property(input => input.FullName)
             .HasColumnName("FullName")
             .HasColumnType("VARCHAR(50)")
             .IsRequired();

            //Tell the modelBuilder more about the CreatedAt column
            modelBuilder.Entity<Employee>()
             .Property(input => input.CreatedAt)
             .HasDefaultValueSql("GetDate()");

            //Tell the modelBuilder more about the UpdatedAt column
            modelBuilder.Entity<Employee>()
             .Property(input => input.UpdatedAt)
             .HasDefaultValueSql("GetDate()");

            //Tell the modelBuilder more about the DeletedAt column
            modelBuilder.Entity<Employee>()
             .Property(input => input.DeletedAt)
             .IsRequired(false);

            modelBuilder.Entity<Employee>()
            .HasOne(courseClass => courseClass.CreatedBy)
            .WithMany()
            .HasForeignKey(courseClass => courseClass.CreatedById)
            .HasPrincipalKey(applicationUserClass => applicationUserClass.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<Employee>()
            .HasOne(courseClass => courseClass.UpdatedBy)
            .WithMany()
            .HasForeignKey(courseClass => courseClass.UpdatedById)
            .HasPrincipalKey(applicationUserClass => applicationUserClass.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<Employee>()
           .HasOne(courseClass => courseClass.DeletedBy)
           .WithMany()
           .HasForeignKey(courseClass => courseClass.DeletedById)
           .HasPrincipalKey(applicationUserClass => applicationUserClass.Id)
           .OnDelete(DeleteBehavior.Restrict);
            //----------- Defining Employee Entity - End --------------

            //----------- Defining EmployeePhoto Entity - Start --------------
            modelBuilder.Entity<EmployeePhoto>()
            .HasKey(input => input.EmployeePhotoId)
            .HasName("PrimaryKey_EmployeePhotoId");

            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.EmployeePhotoId)
            .HasColumnName("EmployeePhotoId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            // Created At
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.CreatedAt)
            .HasDefaultValueSql("GetDate()");


            // Public Cloudinary ID
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.PublicId)
            .HasColumnName("PublicId")
            .HasColumnType("VARCHAR(100)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Version
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.Version)
            .HasColumnName("Version")
            .HasColumnType("int")
            .HasDefaultValue(0);


            // Width
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.Width)
            .HasColumnName("Width")
            .HasColumnType("int")
            .HasDefaultValue(0);
            // Height
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.Height)
            .HasColumnName("Height")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Format
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.Format)
            .HasColumnName("Format")
            .HasColumnType("VARCHAR(14)")
            .HasDefaultValue("")
            .IsRequired(true);

            // ImageSize
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.ImageSize)
            .HasColumnName("ImageSize")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Image URL
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.Url)
            .HasColumnName("Url")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(true);

            // Secure Image URL
            modelBuilder.Entity<EmployeePhoto>()
            .Property(input => input.SecureUrl)
            .HasColumnName("SecureUrl")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(false);
						//----------- Defining EmployeePhoto Entity - End --------------

						//----------------------------------------------------------------------------
						//Define One to One relationship
						//----------------------------------------------------------------------------
						//Reference: http://stackoverflow.com/questions/35506158/one-to-one-relationships-in-entity-framework-7-code-first
		modelBuilder.Entity<Employee>()
            .HasOne(input => input.EmployeePhoto)
            .WithOne(input => input.Employee)
            .HasForeignKey<EmployeePhoto>(input => input.EmployeeId);



            //CA1
            //----------- Defining CategoryBrand Entity - Start --------------

            //Make CategoryBrandId a Primary Key
            modelBuilder.Entity<CategoryBrand>()
                .HasKey(input => input.CategoryBrandId)
                .HasName("PrimaryKey_CategoryBrandId");

            //Define general properties of CategoryBrandId
            modelBuilder.Entity<CategoryBrand>()
                .Property(input => input.CategoryBrandId)
                .HasColumnName("CategoryBrandId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            //Many-One relationship, CategoryBrand and Brand
            modelBuilder.Entity<CategoryBrand>()
                .HasOne(input => input.Brand)
                .WithMany(input => input.CategoryBrands)
                .HasForeignKey(input => input.BrandId);

            //Many-One relationship, CategoryBrand and Category
            modelBuilder.Entity<CategoryBrand>()
                .HasOne(input => input.Category)
                .WithMany(input => input.CategoryBrands)
                .HasForeignKey(input => input.CategoryId);

            //----------- Defining CategoryBrand Entity - End --------------

            //----------- Defining CategoryProduct Entity - Start --------------

            //Make CategoryProductId a Primary Key
            modelBuilder.Entity<CategoryProduct>()
                .HasKey(input => input.CategoryProductId)
                .HasName("PrimaryKey_CategoryProductId");

            //Define general properties of CategoryProductId
            modelBuilder.Entity<CategoryProduct>()
                .Property(input => input.CategoryProductId)
                .HasColumnName("CategoryProductId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            //Many-One relationship, CategoryProduct and Category
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(input => input.Category)
                .WithMany(input => input.CategoryProducts)
                .HasForeignKey(input => input.CategoryId);

            //Many-One relationship, CategoryProduct and Product
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(input => input.Product)
                .WithMany(input => input.CategoryProducts)
                .HasForeignKey(input => input.ProductId);

            //----------- Defining CategoryProduct Entity - End --------------


            //----------- Defining Brand Entity - Start --------------

            //Make BrandId a Primary Key
            modelBuilder.Entity<Brand>()
                .HasKey(input => input.BrandId)
                .HasName("PrimaryKey_BrandId");

            //Define general properties of BrandId
            modelBuilder.Entity<Brand>()
                .Property(input => input.BrandId)
                .HasColumnName("BrandId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            //Define general properties of BrandName
            modelBuilder.Entity<Brand>()
                .Property(input => input.BrandName)
                .HasColumnName("BrandName")
                .HasColumnType("VARCHAR(30)")
                .IsRequired(true);

            //Define general properties of NoOfProducts
            modelBuilder.Entity<Brand>()
                .Property(input => input.NoOfProducts)
                .HasColumnName("NoOfProducts")
                .HasColumnType("int")
                .IsRequired(true);

            //Define general properties of CreatedAt
            modelBuilder.Entity<Brand>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GetDate()");

            //Define general properties of CreatedBy
            //modelBuilder.Entity<Brand>()
            //    .Property(input => input.CreatedBy)

            //Define general properties of UpdatedAt
            modelBuilder.Entity<Brand>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");

            //Define general properties of UpdatedBy
            //modelBuilder.Entity<Brand>()
            //    .Property(input => input.UpdatedBy)


            //Define general properties of DeletedAt
            modelBuilder.Entity<Brand>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            //Define general properties of DeletedBy
            //modelBuilder.Entity<Brand>()
            //    .Property(input => input.DeletedBy)


            //Enforce unique constraint on BrandName
            modelBuilder.Entity<Brand>()
                .HasIndex(input => input.BrandName).IsUnique()
                .HasName("Brand_BrandName_UniqueConstraint");



            //----------- Defining Brand Entity - End --------------


            //----------- Defining Category Entity - Start --------------

            //Make CategoryId a Primary Key
            modelBuilder.Entity<Category>()
                .HasKey(input => input.CategoryId)
                .HasName("PrimaryKey_CategoryId");

            //Define general properties of CategoryId
            modelBuilder.Entity<Category>()
                .Property(input => input.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            //Define general properties of CategoryName
            modelBuilder.Entity<Category>()
                .Property(input => input.CategoryName)
                .HasColumnName("CategoryName")
                .HasColumnType("VARCHAR(30)")
                .IsRequired(true);

            //Define general properties of Visibility
            modelBuilder.Entity<Category>()
                .Property(input => input.Visibility)
                .HasColumnName("Visibility")
                .HasColumnType("int")
                .IsRequired(true);

            //Define general properties of StartDate
            modelBuilder.Entity<Category>()
                .Property(input => input.StartDate)
                .HasColumnName("StartDate")
                .HasColumnType("datetime")
                .ForSqlServerHasDefaultValueSql(null)
                .IsRequired(false);

            //Define general properties of EndDate
            modelBuilder.Entity<Category>()
                .Property(input => input.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("datetime")
                .ForSqlServerHasDefaultValueSql(null)
                .IsRequired(false);

            //Define general properties of IsSpecial
            modelBuilder.Entity<Category>()
                .Property(input => input.IsSpecial)
                .HasColumnName("IsSpecial")
                .HasColumnType("BIT")
                .IsRequired(true);

            //Define general properties of CreatedAt
            modelBuilder.Entity<Category>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GetDate()");

            //Define general properties of CreatedBy
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.CreatedBy)

            //Define general properties of UpdatedAt
            modelBuilder.Entity<Category>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");

            //Define general properties of UpdatedBy
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.UpdatedBy)


            //Define general properties of DeletedAt
            modelBuilder.Entity<Category>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            //Define general properties of DeletedBy
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.DeletedBy)

            //Enforce unique constraint on CategoryName
            modelBuilder.Entity<Category>()
                .HasIndex(input => input.CategoryName).IsUnique()
                .HasName("Category_CategoryName_UniqueConstraint");

            //----------- Defining Category Entity - End --------------



            //----------- Defining Product Entity - Start --------------

            //Make ProductId a Primary Key
            modelBuilder.Entity<Product>()
                .HasKey(input => input.ProductId)
                .HasName("PrimaryKey_ProductId");

            //Define general properties of ProductId
            modelBuilder.Entity<Product>()
                .Property(input => input.ProductId)
                .HasColumnName("ProductId")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            //Define general properties of ProductName
            modelBuilder.Entity<Product>()
                .Property(input => input.ProductName)
                .HasColumnName("ProductName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(true);

            //Define general properties of CreatedAt
            modelBuilder.Entity<Product>()
                .Property(input => input.CreatedAt)
                .HasDefaultValueSql("GetDate()");

            //Define general properties of CreatedBy
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.CreatedBy)

            //Define general properties of UpdatedAt
            modelBuilder.Entity<Product>()
                .Property(input => input.UpdatedAt)
                .HasDefaultValueSql("GetDate()");

            //Define general properties of UpdatedBy
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.UpdatedBy)


            //Define general properties of DeletedAt
            modelBuilder.Entity<Product>()
                .Property(input => input.DeletedAt)
                .IsRequired(false);

            //Define general properties of DeletedBy
            //modelBuilder.Entity<Category>()
            //    .Property(input => input.DeletedBy)

            //Enforce unique constraint on ProductName
            modelBuilder.Entity<Product>()
                .HasIndex(input => input.ProductName).IsUnique()
                .HasName("Product_ProductName_UniqueConstraint");


            //One - One Relationship, Product and Brand
            modelBuilder.Entity<Product>()
                .HasOne(input => input.Brand)
                .WithMany(input => input.Products)
                .HasForeignKey(input => input.BrandId);

            //----------- Defining Product Entity - End --------------



            //----------- Defining BrandPhoto Entity - Start --------------
            modelBuilder.Entity<BrandPhoto>()
            .HasKey(input => input.BrandPhotoId)
            .HasName("PrimaryKey_BrandPhotoId");

            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.BrandPhotoId)
            .HasColumnName("BrandPhotoId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            // Created At
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.CreatedAt)
            .HasDefaultValueSql("GetDate()");


            // Public Cloudinary ID
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.PublicId)
            .HasColumnName("PublicId")
            .HasColumnType("VARCHAR(100)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Version
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.Version)
            .HasColumnName("Version")
            .HasColumnType("int")
            .HasDefaultValue(0);


            // Width
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.Width)
            .HasColumnName("Width")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Height
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.Height)
            .HasColumnName("Height")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Format
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.Format)
            .HasColumnName("Format")
            .HasColumnType("VARCHAR(14)")
            .HasDefaultValue("")
            .IsRequired(true);

            // ImageSize
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.ImageSize)
            .HasColumnName("ImageSize")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Image URL
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.Url)
            .HasColumnName("Url")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(true);

            // Secure Image URL
            modelBuilder.Entity<BrandPhoto>()
            .Property(input => input.SecureUrl)
            .HasColumnName("SecureUrl")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(false);
            //----------- Defining BrandPhoto Entity - End --------------


            //One-One Relationship, BrandPhoto & Brand
            modelBuilder.Entity<Brand>()
            .HasOne(input => input.BrandPhoto)
            .WithOne(input => input.Brand)
            .HasForeignKey<BrandPhoto>(input => input.BrandId);


            //----------- Defining ProductPhoto Entity - Start --------------
            modelBuilder.Entity<ProductPhoto>()
            .HasKey(input => input.ProductPhotoId)
            .HasName("PrimaryKey_ProductPhotoId");

            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.ProductPhotoId)
            .HasColumnName("ProductPhotoId")
            .HasColumnType("int")
            .UseSqlServerIdentityColumn()
            .ValueGeneratedOnAdd()
            .IsRequired(true);

            // Created At
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.CreatedAt)
            .HasDefaultValueSql("GetDate()");


            // Public Cloudinary ID
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.PublicId)
            .HasColumnName("PublicId")
            .HasColumnType("VARCHAR(100)")
            .HasDefaultValue("")
            .IsRequired(false);

            // Version
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.Version)
            .HasColumnName("Version")
            .HasColumnType("int")
            .HasDefaultValue(0);


            // Width
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.Width)
            .HasColumnName("Width")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Height
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.Height)
            .HasColumnName("Height")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Format
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.Format)
            .HasColumnName("Format")
            .HasColumnType("VARCHAR(14)")
            .HasDefaultValue("")
            .IsRequired(true);

            // ImageSize
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.ImageSize)
            .HasColumnName("ImageSize")
            .HasColumnType("int")
            .HasDefaultValue(0);

            // Image URL
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.Url)
            .HasColumnName("Url")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(true);

            // Secure Image URL
            modelBuilder.Entity<ProductPhoto>()
            .Property(input => input.SecureUrl)
            .HasColumnName("SecureUrl")
            .HasColumnType("VARCHAR(300)")
            .HasDefaultValue("")
            .IsRequired(false);
            //----------- Defining ProductPhoto Entity - End --------------


            //One-One Relationship, ProductPhoto & Product
            modelBuilder.Entity<Product>()
            .HasOne(input => input.ProductPhoto)
            .WithOne(input => input.Product)
            .HasForeignKey<ProductPhoto>(input => input.ProductId);


            base.OnModelCreating(modelBuilder);
        }//End of OnModelCreating method
    }//End of ApplicationDbContext class definition
}//End of Namespace
