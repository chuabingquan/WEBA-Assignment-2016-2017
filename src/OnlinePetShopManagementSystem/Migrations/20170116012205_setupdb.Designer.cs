using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OnlinePetShopManagementSystem.Data;

namespace OnlinePetShopManagementSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170116012205_setupdb")]
    partial class setupdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("BrandId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnName("BrandName")
                        .HasColumnType("VARCHAR(30)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<int>("NoOfProducts")
                        .HasColumnName("NoOfProducts")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.HasKey("BrandId")
                        .HasName("PrimaryKey_BrandId");

                    b.HasIndex("BrandName")
                        .IsUnique()
                        .HasName("Brand_BrandName_UniqueConstraint");

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.BrandPhoto", b =>
                {
                    b.Property<int>("BrandPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("BrandPhotoId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("Format")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Format")
                        .HasColumnType("VARCHAR(14)")
                        .HasDefaultValue("");

                    b.Property<int>("Height")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Height")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("ImageSize")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ImageSize")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("PublicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PublicId")
                        .HasColumnType("VARCHAR(100)")
                        .HasDefaultValue("");

                    b.Property<string>("SecureUrl")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SecureUrl")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<string>("Url")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Url")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Version")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Width")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Width")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("BrandPhotoId")
                        .HasName("PrimaryKey_BrandPhotoId");

                    b.HasIndex("BrandId")
                        .IsUnique();

                    b.ToTable("BrandPhoto");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CategoryId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnName("CategoryName")
                        .HasColumnType("VARCHAR(30)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<DateTime?>("EndDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsSpecial")
                        .HasColumnName("IsSpecial")
                        .HasColumnType("BIT");

                    b.Property<DateTime?>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("StartDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int>("Visibility")
                        .HasColumnName("Visibility")
                        .HasColumnType("int");

                    b.HasKey("CategoryId")
                        .HasName("PrimaryKey_CategoryId");

                    b.HasIndex("CategoryName")
                        .IsUnique()
                        .HasName("Category_CategoryName_UniqueConstraint");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.CategoryBrand", b =>
                {
                    b.Property<int>("CategoryBrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CategoryBrandId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId");

                    b.Property<int>("CategoryId");

                    b.HasKey("CategoryBrandId")
                        .HasName("PrimaryKey_CategoryBrandId");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryBrand");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.CategoryProduct", b =>
                {
                    b.Property<int>("CategoryProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CategoryProductId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<int>("ProductId");

                    b.HasKey("CategoryProductId")
                        .HasName("PrimaryKey_CategoryProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EmployeeId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnName("FullName")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<DateTime>("JoinDate");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("UpdatedById")
                        .IsRequired();

                    b.HasKey("EmployeeId")
                        .HasName("PrimaryKey_EmployeeId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.EmployeePhoto", b =>
                {
                    b.Property<int>("EmployeePhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EmployeePhotoId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int>("EmployeeId");

                    b.Property<string>("Format")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Format")
                        .HasColumnType("VARCHAR(14)")
                        .HasDefaultValue("");

                    b.Property<int>("Height")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Height")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("ImageSize")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ImageSize")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("PublicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PublicId")
                        .HasColumnType("VARCHAR(100)")
                        .HasDefaultValue("");

                    b.Property<string>("SecureUrl")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SecureUrl")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<string>("Url")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Url")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Version")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Width")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Width")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("EmployeePhotoId")
                        .HasName("PrimaryKey_EmployeePhotoId");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("EmployeePhoto");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProductId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnName("ProductName")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.HasKey("ProductId")
                        .HasName("PrimaryKey_ProductId");

                    b.HasIndex("BrandId");

                    b.HasIndex("ProductName")
                        .IsUnique()
                        .HasName("Product_ProductName_UniqueConstraint");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.ProductPhoto", b =>
                {
                    b.Property<int>("ProductPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProductPhotoId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("Format")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Format")
                        .HasColumnType("VARCHAR(14)")
                        .HasDefaultValue("");

                    b.Property<int>("Height")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Height")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("ImageSize")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ImageSize")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("ProductId");

                    b.Property<string>("PublicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PublicId")
                        .HasColumnType("VARCHAR(100)")
                        .HasDefaultValue("");

                    b.Property<string>("SecureUrl")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SecureUrl")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<string>("Url")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Url")
                        .HasColumnType("VARCHAR(300)")
                        .HasDefaultValue("");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Version")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Width")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Width")
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("ProductPhotoId")
                        .HasName("PrimaryKey_ProductPhotoId");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("ProductPhoto");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlinePetShopManagementSystem.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.BrandPhoto", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.Brand", "Brand")
                        .WithOne("BrandPhoto")
                        .HasForeignKey("OnlinePetShopManagementSystem.Models.BrandPhoto", "BrandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.CategoryBrand", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.Brand", "Brand")
                        .WithMany("CategoryBrands")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlinePetShopManagementSystem.Models.Category", "Category")
                        .WithMany("CategoryBrands")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.CategoryProduct", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.Category", "Category")
                        .WithMany("CategoryProducts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlinePetShopManagementSystem.Models.Product", "Product")
                        .WithMany("CategoryProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.Employee", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("OnlinePetShopManagementSystem.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("OnlinePetShopManagementSystem.Models.ApplicationUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.EmployeePhoto", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.Employee", "Employee")
                        .WithOne("EmployeePhoto")
                        .HasForeignKey("OnlinePetShopManagementSystem.Models.EmployeePhoto", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.Product", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlinePetShopManagementSystem.Models.ProductPhoto", b =>
                {
                    b.HasOne("OnlinePetShopManagementSystem.Models.Product", "Product")
                        .WithOne("ProductPhoto")
                        .HasForeignKey("OnlinePetShopManagementSystem.Models.ProductPhoto", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
