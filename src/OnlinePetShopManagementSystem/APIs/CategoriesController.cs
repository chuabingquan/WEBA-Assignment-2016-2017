using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlinePetShopManagementSystem.Data;
using OnlinePetShopManagementSystem.Models;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Cloudinary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlinePetShopManagementSystem.APIs
{

    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : Controller
    {
        //Declare a property called Database
        //Represents database in SQLServer database engine
        public ApplicationDbContext Database { get; }

        //Categories Web API Controller Constructor
        public CategoriesController()
        {
            //When Web Application creates object out of this controller
            //Database Property will be activated
            Database = new ApplicationDbContext();
        }


        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            List<object> categoryList = new List<Object>();
            //List<object> associatedBrandsList = new List<object>();

            var categoriesQueryResult = Database.Categories
                .Where(input => input.DeletedAt == null)
                //.Include()
                .OrderBy(input => input.CategoryId);

            //var categoryBrandQueryResult = Database.CategoryBrands
            //    .OrderBy(input => input.BrandId)
            //    .Distinct();
                //.Where(input => input.DeletedAt == null)

            foreach (var categoryResult in categoriesQueryResult)
            {
                //associatedBrandsList.Clear();
                //foreach (var categoryBrandResult in categoryBrandQueryResult)
                //{
                //    if (categoryResult.CategoryId.Equals(categoryBrandResult.CategoryId))
                //    {
                //        associatedBrandsList.Add(new
                //        {
                //            brandId = categoryBrandResult.BrandId
                //        });
                //    }
                //}

                categoryList.Add(new
                {
                    categoryId = categoryResult.CategoryId,
                    categoryName = categoryResult.CategoryName,
                    createdAt = categoryResult.CreatedAt,
                    //createdBy = categoryResult.CreatedBy,
                    updatedAt = categoryResult.UpdatedAt,
                    //updatedBy = categoryResult.updatedBy,
                    isSpecial = categoryResult.IsSpecial,
                    visibility = categoryResult.Visibility,
                    //brand = associatedBrandsList.ToArray()
                });

            }

            return new JsonResult(categoryList);
        }

        // GET: api/values
        [HttpGet("GetSpecialCategories")]
        public JsonResult GetSpecialCategories()
        {
            List<object> specialCategoryList = new List<Object>();
            var specialCategoriesQueryResult = Database.Categories
                .Where(input => input.IsSpecial == true && input.DeletedAt == null)
                //.Include()
                .OrderBy(input => input.CategoryId);

            foreach (var specialCategoryResult in specialCategoriesQueryResult)
            {
                specialCategoryList.Add(new
                {
                    categoryId = specialCategoryResult.CategoryId,
                    categoryName = specialCategoryResult.CategoryName,
                    createdAt = specialCategoryResult.CreatedAt,
                    //createdBy = specialCategoryResult.CreatedBy,
                    updatedAt = specialCategoryResult.UpdatedAt,
                    //updatedBy = specialCategoryResult.updatedBy,
                    isSpecial = specialCategoryResult.IsSpecial,
                    visibility = specialCategoryResult.Visibility,
                });

            }

            return new JsonResult(specialCategoryList);
        }

        // GET: api/values
        [HttpGet("GetCategoriesForControls")]
        public JsonResult GetCategoriesForControls()
        {
            var categoryList = new List<object>();

            var foundCategories = Database.Categories
                .Where(input => input.DeletedAt == null && (input.IsSpecial == false));

            foreach (var category in foundCategories)
            {
                categoryList.Add((new
                    {
                        categoryId = category.CategoryId,
                        categoryName = category.CategoryName
                    }));
            }

            return new JsonResult(categoryList);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string customMessage = "";

            var foundOneCategory = Database.Categories
                .Where(item => item.CategoryId == id).Single();

            var response = new
            {
                categoryId = foundOneCategory.CategoryId,
                categoryName = foundOneCategory.CategoryName,
                visibility = foundOneCategory.Visibility,
                startDate = foundOneCategory.StartDate,
                endDate = foundOneCategory.EndDate,
                isSpecial = foundOneCategory.IsSpecial
            };//End of creation of response object

            return new JsonResult(response);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            string customMessage = "";
            Category newCategory = new Category();

            //Converts string data in JSON into usable object
            var newCategoryInput = JsonConvert.DeserializeObject<dynamic>(value);

            newCategory.CategoryName = newCategoryInput.categoryName.Value;
            newCategory.Visibility = Convert.ToInt32(newCategoryInput.visibility.Value);

            DateTime newStartDateInput = new DateTime();
            if (newCategory.Visibility == 1 && (DateTime.TryParseExact(newCategoryInput.startDate.Value,
                "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newStartDateInput)))
            {
                newCategory.StartDate = newStartDateInput;
            }

            else
            {
                newCategory.StartDate = null;
            }

            DateTime newEndDateInput = new DateTime();
            if (newCategory.Visibility == 1 && (DateTime.TryParseExact(newCategoryInput.endDate.Value,
                "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newEndDateInput)))
            {
                newCategory.EndDate = newEndDateInput;
            }

            else
            {
                newCategory.EndDate = null;
            }

            newCategory.IsSpecial = bool.Parse(newCategoryInput.isSpecial.Value);

            try
            {
                Database.Add(newCategory);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message
                    .Contains("Category_CategoryName_UniqueConstraint") == true)
                {
                    customMessage = "Your Category, \"" + newCategoryInput.categoryName.Value
                        + "\", has failed to add due to another Category with the same name. Please try another Category Name.";
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }
            }

            var successRequestResultMessage = new
            {
                Message = newCategoryInput.categoryName.Value + " has been added to your Categories successfully!"
            };

            OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);


            return httpOkResult;
        }


        // POST api/values
        [HttpPost("SearchCategoryByName")]
        public IActionResult SearchCategoryByName([FromBody]string value)
        {
            string customMessage = "";
            string categorySearchQuery = "";
            List<object> searchCategoryList = new List<Object>();

            try
            {

            categorySearchQuery = JsonConvert.DeserializeObject<dynamic>(value).searchQuery.Value.ToString();
            categorySearchQuery = categorySearchQuery.Trim();

                //categorySearchQuery = value.ToString();

                var searchCategoriesQueryResult = Database.Categories
                    .Where(input => input.CategoryName.Contains(categorySearchQuery) && input.DeletedAt == null)
                    //.Include()
                    .OrderBy(input => input.CategoryId)
                    .ToList();


                if (searchCategoriesQueryResult.Count > 0)
                {
                    foreach (var searchCategoryResult in searchCategoriesQueryResult)
                    {
                        searchCategoryList.Add(new
                        {
                            categoryId = searchCategoryResult.CategoryId,
                            categoryName = searchCategoryResult.CategoryName,
                            createdAt = searchCategoryResult.CreatedAt,
                            //createdBy = specialCategoryResult.CreatedBy,
                            updatedAt = searchCategoryResult.UpdatedAt,
                            //updatedBy = specialCategoryResult.updatedBy,
                            isSpecial = searchCategoryResult.IsSpecial,
                            visibility = searchCategoryResult.Visibility,
                        });

                    }
                }

                else
                {
                    customMessage = "Your search for " + value + " showed up no results.";
                    object failRequestResultMessage = new
                    {
                        Message = customMessage
                    };

                    return BadRequest(failRequestResultMessage);
                }
            }

            catch (Exception ex)
            {
                customMessage = "Your search for " + value + " showed up no results.";
                object failRequestResultMessage = new
                {
                    Message = customMessage
                };

                return BadRequest(failRequestResultMessage);
            }

            return new JsonResult(searchCategoryList);
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {            
            
            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore,
            //    DefaultValueHandling = DefaultValueHandling.Ignore,
            //};

            String customMessage = "";
            var changedCategoryInput = JsonConvert.DeserializeObject<dynamic>(value);

            var foundOneCategory = Database.Categories
                .Where(item => item.CategoryId == id).FirstOrDefault();

            String orginalCatName = foundOneCategory.CategoryName;

            foundOneCategory.CategoryName = changedCategoryInput.categoryName.Value;
            foundOneCategory.Visibility = Convert.ToInt32(changedCategoryInput.visibility.Value);

            DateTime changedStartDateInput = new DateTime();
            if (foundOneCategory.Visibility == 1 && (DateTime.TryParseExact(changedCategoryInput.startDate.Value,
                "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out changedStartDateInput)))
            {
                foundOneCategory.StartDate = changedStartDateInput;
            }

            else
            {
                foundOneCategory.StartDate = null;
            }

            DateTime changedEndDateInput = new DateTime();
            if (foundOneCategory.Visibility == 1 && (DateTime.TryParseExact(changedCategoryInput.endDate.Value,
                "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out changedEndDateInput)))
            {
                foundOneCategory.EndDate = changedEndDateInput;
            }

            else
            {
                foundOneCategory.EndDate = null;
            }

            foundOneCategory.IsSpecial = bool.Parse(changedCategoryInput.isSpecial.Value);

            foundOneCategory.UpdatedAt = DateTime.Now;

            try
            {
                Database.Update(foundOneCategory);
                Database.SaveChanges();
            }

            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Category_CategoryName_UniqueConstraint"))
                {
                    customMessage = "Update failed as there is already a Category by the same name, \""
                        + changedCategoryInput.categoryName.Value + "\" in the database records. Please use another Category Name.";
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }
            }

            var successRequestResultMessage = new
            {
                Message = "Category has been successfully updated!"
            };

            OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);

            return httpOkResult;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string customMessage = "";
            string associatedBrandsString = "";
            string associatedProductsString = "";

            try
            {
                //Check if Category Record to be deleted has any Brands associated with it
                //var foundOneCategory = Database.Categories
                //    .Where(input => input.CategoryId == id)
                //    .Single();

                //List<Brand> brandsAssociatedCat = null;
                

                var foundOneCategory = Database.Categories
                    .Where(input => input.CategoryId == id)
                    .Single();


                var foundCategoryBrandsCollection = Database.CategoryBrands
                    .Where(input => input.CategoryId == id)
                    .ToList();

                var foundCategoryProductsCollection = Database.CategoryProducts
                    .Where(input => input.CategoryId == id)
                    .ToList();


                if (foundCategoryBrandsCollection.Any() == true)
                {

                    foreach (var foundCategoryBrand in foundCategoryBrandsCollection)
                    {
                        var foundBrand = Database.Brands
                            .Where(input => input.BrandId == foundCategoryBrand.BrandId /*&& input.DeletedAt == null*/)
                            .Single();

                        if (foundBrand.BrandId != null)
                        {
                            //brandsAssociatedCat.Add(foundBrand);
                            associatedBrandsString += foundBrand.BrandName + ", ";
                        }
                    }

                    associatedBrandsString = associatedBrandsString.Substring(0, associatedBrandsString.Length - 2);
                    associatedBrandsString += ".";

                    //Error deleting message
                    customMessage = "Your category, " + foundOneCategory.CategoryName + " cannot be deleted as it is associated with brands: "
                        + associatedBrandsString + "Please delete these brands first before deleting " + foundOneCategory.CategoryName + " category.";

                    object httpFailRequestResultMessage = new
                    {
                        message = customMessage
                    };

                    return BadRequest(httpFailRequestResultMessage);
                }

                else if (foundCategoryProductsCollection.Any() == true)
                {
                    foreach (var foundCategoryProduct in foundCategoryProductsCollection)
                    {
                        var foundProduct = Database.Products
                            .Where(input => input.ProductId == foundCategoryProduct.ProductId /*&& input.DeletedAt == null*/)
                            .Single();

                        if (foundProduct.ProductId != null)
                        {
                            //brandsAssociatedCat.Add(foundBrand);
                            associatedProductsString += foundProduct.ProductName + ", ";
                        }
                    }

                    associatedProductsString = associatedProductsString.Substring(0, associatedProductsString.Length - 2);
                    associatedProductsString += ".";


                    //Error deleting message
                    customMessage = "Your category, " + foundOneCategory.CategoryName + ", cannot be deleted as it is associated with products: "
                        + associatedProductsString + "Please delete these products first before deleting " + foundOneCategory.CategoryName + " category.";

                    object httpFailRequestResultMessage = new
                    {
                        message = customMessage
                    };

                    return BadRequest(httpFailRequestResultMessage);
                }

                else
                {
                    //Add delete time as soft delete record

                    foundOneCategory.DeletedAt = DateTime.Now;
                    Database.Update(foundOneCategory);
                    Database.SaveChanges();

                    var successRequestResultMessage = new
                    {
                        message = "Your category, " + foundOneCategory.CategoryName + " record has been deleted!"
                    };

                    OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);
                    return httpOkResult;
                }     
            }

            catch (Exception ex)
            {
                customMessage = "Unexpected error, unable to delete category record.";      
                object httpFailRequestResultMessage = new { message = customMessage + ex.Message};
                return BadRequest(httpFailRequestResultMessage);
            }

        }
    }
    }

