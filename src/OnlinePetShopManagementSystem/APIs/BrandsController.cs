using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Cloudinary;
using OnlinePetShopManagementSystem.Data;
using OnlinePetShopManagementSystem.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlinePetShopManagementSystem.APIs
{
    [Route("api/[controller]")]
    public class BrandsController : Controller
    {
        //Declare a property called Database
        //Represents database in SQLServer database engine
        public ApplicationDbContext Database { get; }

        //Const string for upload preset
        private const string UPLOAD_PRESET = "upload_to_brands";

        //Categories Web API Controller Constructor
        public BrandsController()
        {
            //When Web Application creates object out of this controller
            //Database Property will be activated
            Database = new ApplicationDbContext();
        }


        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
                List<object> brandList = new List<Object>();
                string categoriesAssociated = "";


                var categoryBrandQueryResult = Database.CategoryBrands
                    /*.Where(input => input.DeleteAt == null)*/;
                var categoriesQueryResult = Database.Categories
                    .Where(input => input.DeletedAt == null);


                var brandsQueryResult = Database.Brands
                    .Where(input => input.DeletedAt == null)
                    .OrderBy(input => input.BrandId);

                var brandPhotosQueryResult = Database.BrandPhotos;


                foreach (var brandResult in brandsQueryResult)
                {
                    categoriesAssociated = "";

                    foreach (var categoryBrandResult in categoryBrandQueryResult)
                    {
                        if (categoryBrandResult.BrandId.Equals(brandResult.BrandId))
                        {
                            var associatedCategory = categoriesQueryResult
                                .Where(input => input.CategoryId.Equals(categoryBrandResult.CategoryId))
                                .Single();

                            categoriesAssociated += associatedCategory.CategoryName + ", ";

                        }

                    }

                    if (categoriesAssociated.Length > 0)
                    {
                        categoriesAssociated = categoriesAssociated.Remove(categoriesAssociated.Length - 2);
                    }

                    var brandPhotoResult = brandPhotosQueryResult.
                        Where(input => input.BrandId.Equals(brandResult.BrandId)).Single();

                    string brandPhotoResultUrl = brandPhotoResult.Url;

                    brandList.Add(new
                    {
                        brandId = brandResult.BrandId,
                        imageUrl = brandPhotoResultUrl,
                        brandName = brandResult.BrandName,
                        categoryName = categoriesAssociated,
                        noOfProducts = brandResult.NoOfProducts,
                        createdAt = brandResult.CreatedAt,
                        updatedAt = brandResult.UpdatedAt
                    });
                }

                return new JsonResult(brandList);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string customMessage = "";
            var nestedCategoryList = new List<object>();
            

            var foundOneBrand = Database.Brands
                .Where(item => item.BrandId == id).Single();

            var foundOneBrandPhoto = Database.BrandPhotos
                .Where(item => item.BrandId == id)
                .Single();

            var foundAssociatedCategories = Database.CategoryBrands
                .Where(input => input.BrandId == id)
                .OrderBy(input => input.CategoryId);

            foreach (var foundAssociatedCategory in foundAssociatedCategories)
            {
                var categoryResponse = new
                {
                    categoryId = foundAssociatedCategory.CategoryId
                };

                nestedCategoryList.Add(categoryResponse);
            }

            var response = new
            {
                brandId = foundOneBrand.BrandId,
                brandName = foundOneBrand.BrandName,
                brandPhotoUrl = foundOneBrandPhoto.Url,
                category = nestedCategoryList

            };//End of creation of response object

            return new JsonResult(response);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        [HttpPost("SearchBrand")]
        public IActionResult SearchBrand([FromBody]string value)
        {
            string customMessage = "";
            string brandSearchQuery = "";
            int filterSearchQuery;
            string categoriesAssociated = "";
            List<object> searchBrandList = new List<Object>();
            List<object> associatedCategoryList = new List<object>();
            bool check = false;


            brandSearchQuery = JsonConvert.DeserializeObject<dynamic>(value).searchQuery.Value.ToString();
            brandSearchQuery = brandSearchQuery.Trim();

            filterSearchQuery = Convert.ToInt32(JsonConvert.DeserializeObject<dynamic>(value).filter.Value);


            var categoryBrandQueryResult = Database.CategoryBrands
                .Where(input => input.CategoryId.Equals(filterSearchQuery));
            var categoriesQueryResult = Database.Categories;
            var brandPhotosQueryResult = Database.BrandPhotos;

            try
            {

                var searchBrandsQueryResult = Database.Brands
                    .Where(input => input.BrandName.Contains(brandSearchQuery)/* && input.DeletedAt == null*/)
                    //.Include()
                    .OrderBy(input => input.BrandId)
                    .ToList();

                var lastSearchBrandResult = searchBrandsQueryResult.LastOrDefault();


                if (searchBrandsQueryResult.Count > 0)
                {
                    foreach (var searchBrandResult in searchBrandsQueryResult)
                    {
                        categoriesAssociated = "";

                        foreach (var categoryBrandResult in categoryBrandQueryResult)
                        {
                            if (categoryBrandResult.BrandId.Equals(searchBrandResult.BrandId))
                            {
                                var associatedCategory = categoriesQueryResult
                                    .Where(input => input.CategoryId.Equals(categoryBrandResult.CategoryId))
                                    .Single();

                                associatedCategoryList.Add(associatedCategory.CategoryName);

                            }

                            if (categoryBrandResult.CategoryBrandId == categoryBrandQueryResult.Last().CategoryBrandId) {
                                if (associatedCategoryList.Count <= 0)
                                {
                                    check = false;
                                }

                                else
                                {
                                    check = true;
                                }
                            }

                        }

                        if (categoriesAssociated.Length > 0)
                        {
                            categoriesAssociated = categoriesAssociated.Remove(categoriesAssociated.Length - 2);
                        }

                        var brandPhotoResult = brandPhotosQueryResult.Where(input => input.BrandId.Equals(searchBrandResult.BrandId)).Single();

                        if (check)
                        {
                            searchBrandList.Add(new
                            {
                                brandId = searchBrandResult.BrandId,
                                imageUrl = brandPhotoResult.Url,
                                brandName = searchBrandResult.BrandName,
                                categoryName = categoriesAssociated,
                                noOfProducts = searchBrandResult.NoOfProducts,
                                createdAt = searchBrandResult.CreatedAt,
                                updatedAt = searchBrandResult.UpdatedAt
                            });
                        }

                    }

                    if (searchBrandList.Count <= 0)
                    {
                        customMessage = "Your search for " + brandSearchQuery +" by " + filterSearchQuery + " showed up no results.";
                        object failRequestResultMessage = new
                        {
                            Message = customMessage
                        };

                        return BadRequest(failRequestResultMessage);
                    }
                }

                else
                {
                    customMessage = "Your search for " + brandSearchQuery + " by " + filterSearchQuery + " showed up no results.";
                    object failRequestResultMessage = new
                    {
                        Message = customMessage
                    };

                    return BadRequest(failRequestResultMessage);
                }
            }

            catch (Exception ex)
            {
                customMessage = "Your search for " + brandSearchQuery + " by " + filterSearchQuery + " showed up no results.";
                object failRequestResultMessage = new
                {
                    Message = customMessage
                };

                return BadRequest(failRequestResultMessage);
            }

            return new JsonResult(searchBrandList);
        }

        [HttpPost("SaveBrandDataAndPhoto")]
        public async Task<IActionResult> SaveBrandDataAndPhoto(IList<IFormFile> fileInput, IFormCollection formData)
        {
            //IFormFile interface represents binary file image travelling across the network
            IFormFile oneFile = null;
            object successRequestResultMessage = null;
            string customMessage = "";


            if (fileInput.Count != 0)
            {
                oneFile = fileInput[0];
            }

            string brandName = formData["brandName"];

            //Collect data from the body of the HTTP request
            Brand newBrand = new Brand();

            if (brandName.Equals("") || brandName == null)
            {
                customMessage = "Please enter a brand name!";
                object httpFailRequestResultMessage = new { Message = customMessage };
                return BadRequest(httpFailRequestResultMessage);
            }

            else
            {
                newBrand.BrandName = brandName;
                newBrand.NoOfProducts = 0;
            }

            try
            {
                Database.Brands.Add(newBrand);
                Database.SaveChanges();
            }

            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Brand_BrandName_UniqueConstraint") == true)
                {
                    customMessage = "Your Brand, \"" + formData["brandName"]
                            + "\", has failed to add due to another Brand with the same name. Please try another Brand Name.";
                    object httpFailRequestResultMessage = new { Message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }
            }

            var justAddedBrand = Database.Brands
                .Where(input => input.BrandName.Equals(newBrand.BrandName))
                .Single();

            var selectedCategoriesJSON = formData["selectedAssociatedCat"];

                var selectedCategories = JsonConvert.DeserializeObject<dynamic>(selectedCategoriesJSON);

                try
                {
                    foreach (var category in selectedCategories)
                    {
                        CategoryBrand newCBAssociation = new CategoryBrand();
                        newCBAssociation.BrandId = justAddedBrand.BrandId;
                        newCBAssociation.CategoryId = Convert.ToInt32(category.categoryId.Value);
                        Database.CategoryBrands.Add(newCBAssociation);
                    }

                    Database.SaveChanges();
                    customMessage = "Your brand, \"" + formData["brandName"] +"\" is saved ";
                }
                catch (Exception ex)
                {
                    Database.Brands.Remove(justAddedBrand);
                    Database.SaveChanges();
                    customMessage = "Unable to save brand record, due to the selected categories. Please contact your system administrator for more details.";
                    object httpFailRequestResultMessage = new { message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }//End of try catch for saving Brand details and Brand-Category association


            //Add Brand Photo
            try
            {
                var fileName = ContentDispositionHeaderValue
                      .Parse(oneFile.ContentDisposition)
                      .FileName
                      .Trim('"');
                string contentType = oneFile.ContentType;
                BrandPhoto brandPhoto = new BrandPhoto();
                brandPhoto = await CloudinaryAPIs.
                UploadImageToCloudinary<BrandPhoto>(UPLOAD_PRESET, oneFile.OpenReadStream(),
                contentType, fileName);


                if (brandPhoto.PublicId != "")
                {
                    brandPhoto.BrandId = justAddedBrand.BrandId;
                    Database.BrandPhotos.Add(brandPhoto);
                    Database.SaveChanges();
                    customMessage += "with brand photo!";
                }
                else
                {
                    var associatedCBRecordsCollection = Database.CategoryBrands
                        .Where(input => input.BrandId == justAddedBrand.BrandId);

                    if (associatedCBRecordsCollection != null)
                    {
                        foreach (var associatedCBRecord in associatedCBRecordsCollection)
                        {
                            Database.CategoryBrands.Remove(associatedCBRecord);
                        }
                    }

                    Database.Brands.Remove(justAddedBrand);
                    Database.SaveChanges();

                    customMessage = "Unable to save brand! Brand upload failed!";
                    object httpFailRequestResultMessage = new { message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }

                successRequestResultMessage = new { message = customMessage };
                OkObjectResult httpOkResult =
                    new OkObjectResult(successRequestResultMessage);
                return httpOkResult;
            }

            catch (Exception ex)
            {
                var associatedCBRecordsCollection = Database.CategoryBrands
                    .Where(input => input.BrandId == justAddedBrand.BrandId);

                if (associatedCBRecordsCollection != null)
                {
                    foreach (var associatedCBRecord in associatedCBRecordsCollection)
                    {
                        Database.CategoryBrands.Remove(associatedCBRecord);
                    }
                }
                    
                Database.Brands.Remove(justAddedBrand);
                Database.SaveChanges();

                customMessage = "Unable to save brand! Please upload a brand image and try again.";
                object httpFailRequestResultMessage = new { message = customMessage };
                return BadRequest(httpFailRequestResultMessage);
            }
        }//End of SaveBrandDataAndPhoto()


        // PUT api/
        [HttpPut("UpdateBrandDataAndPhoto")]
        public async Task<IActionResult> Put(IList<IFormFile> fileInput, IFormCollection formData)
        {

            int id = Convert.ToInt32(formData["brandId"]);

            //Get Brand before Update
            var brandToBeUpdated = Database.Brands
                .Where(input => input.BrandId == id)
                .FirstOrDefault();

            //IFormFile interface represents binary file image travelling across the network
            IFormFile oneFile = null;
            object successRequestResultMessage = null;
            object httpFailRequestResultMessage = null;
            string customMessage = "";


            if (fileInput.Count != 0)
            {
                oneFile = fileInput[0];
            }

            string brandName = formData["brandName"];

            //Collect data from the body of the HTTP request
            //Brand newBrand = new Brand();

            if (brandName.Equals("") || brandName == null)
            {
                customMessage = "Please enter a brand name!";
                httpFailRequestResultMessage = new { Message = customMessage };
                return BadRequest(httpFailRequestResultMessage);
            }

            else
            {
                brandToBeUpdated.BrandName = brandName;
                //newBrand.NoOfProducts = 0;
            }

            //try
            //{
                //Database.Brands.Add(brandToBeUpdated);
                //Database.SaveChanges();
            //}

            //catch (Exception ex)
            //{
                //if (ex.InnerException.Message.Contains("Brand_BrandName_UniqueConstraint") == true)
                //{
                //    customMessage = "Your Brand, \"" + formData["brandName"]
                //            + "\", has failed to add due to another Brand with the same name. Please try another Brand Name.";
                //    object httpFailRequestResultMessage = new { Message = customMessage };
                //    return BadRequest(httpFailRequestResultMessage);
                //}
            //}

            //var justAddedBrand = Database.Brands
            //    .Where(input => input.BrandName.Equals(brandToBeUpdated.BrandName))
            //    .Single();

            var foundAssociatedCategoryBrands = Database.CategoryBrands
                .Where(input => input.BrandId == id);

            var selectedCategoriesJSON = formData["selectedAssociatedCat"];

            var selectedCategories = JsonConvert.DeserializeObject<dynamic>(selectedCategoriesJSON);

            try
            {
                foreach (var associatedCategoryBrand in foundAssociatedCategoryBrands)
                {
                    Database.CategoryBrands.Remove(associatedCategoryBrand);
                }

                foreach (var category in selectedCategories)
                {
                    CategoryBrand newCBAssociation = new CategoryBrand();
                    newCBAssociation.BrandId = brandToBeUpdated.BrandId;
                    newCBAssociation.CategoryId = Convert.ToInt32(category.categoryId.Value);
                    Database.CategoryBrands.Add(newCBAssociation);
                }

                Database.Brands.Add(brandToBeUpdated);
                //Database.SaveChanges();
                customMessage = "Your brand, \"" + formData["brandName"] + "\" is saved ";

                //Update Brand Photo
                //try
                //{
                var fileName = ContentDispositionHeaderValue
                      .Parse(oneFile.ContentDisposition)
                      .FileName
                      .Trim('"');
                string contentType = oneFile.ContentType;

                BrandPhoto newBrandPhoto;

                var brandPhotoQueryResult = Database.BrandPhotos
                    .Where(input => input.BrandId == id);

                foreach (var brandPhoto in brandPhotoQueryResult)
                {
                    await CloudinaryAPIs.DeleteImageInCloudinary(brandPhoto.PublicId);
                    Database.BrandPhotos.Remove(brandPhoto);
                }

                newBrandPhoto = await CloudinaryAPIs.
                    UploadImageToCloudinary<BrandPhoto>(UPLOAD_PRESET, oneFile.OpenReadStream(),
                    contentType, fileName);

                //brandPhoto = await CloudinaryAPIs.
                //UploadImageToCloudinary<BrandPhoto>(UPLOAD_PRESET, oneFile.OpenReadStream(),
                //contentType, fileName);


                if (newBrandPhoto.PublicId != "")
                    {
                        newBrandPhoto.BrandId = id;
                        Database.BrandPhotos.Add(newBrandPhoto);
                        Database.SaveChanges();
                        customMessage += "with brand photo!";
                    }
                    else
                    {
                        //var associatedCBRecordsCollection = Database.CategoryBrands
                        //    .Where(input => input.BrandId == justAddedBrand.BrandId);

                        //if (associatedCBRecordsCollection != null)
                        //{
                        //    foreach (var associatedCBRecord in associatedCBRecordsCollection)
                        //    {
                        //        Database.CategoryBrands.Remove(associatedCBRecord);
                        //    }
                        //}

                        //Database.Brands.Remove(justAddedBrand);
                        //Database.SaveChanges();

                        customMessage = "Unable to save brand! Brand upload failed!";
                        httpFailRequestResultMessage = new { message = customMessage };
                        return BadRequest(httpFailRequestResultMessage);
                    }
                //}

                //catch (Exception ex)
                //{
                //    var associatedCBRecordsCollection = Database.CategoryBrands
                //        .Where(input => input.BrandId == justAddedBrand.BrandId);

                //    if (associatedCBRecordsCollection != null)
                //    {
                //        foreach (var associatedCBRecord in associatedCBRecordsCollection)
                //        {
                //            Database.CategoryBrands.Remove(associatedCBRecord);
                //        }
                //    }

                    //Database.Brands.Remove(justAddedBrand);
                    //Database.SaveChanges();

                    //customMessage = "Unable to save brand! Please upload a brand image and try again.";
                    //httpFailRequestResultMessage = new { message = customMessage };
                    //return BadRequest(httpFailRequestResultMessage);
                //}




            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Brand_BrandName_UniqueConstraint") == true)
                {
                    customMessage = "Your Brand, \"" + formData["brandName"]
                            + "\", has failed to add due to another Brand with the same name. Please try another Brand Name.";
                    httpFailRequestResultMessage = new { Message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }

                //Database.Brands.Remove(justAddedBrand);
                //Database.SaveChanges();
                //else
                //{
                //    customMessage = "Unable to save brand record, due to the selected categories. Please contact your system administrator for more details.";
                //    httpFailRequestResultMessage = new { message = customMessage };
                //    return BadRequest(httpFailRequestResultMessage);
                //}
                else if (fileInput.Count == 0)
                {
                    customMessage = "Unable to save brand! Please upload a brand image and try again.";
                    httpFailRequestResultMessage = new { message = customMessage };
                    return BadRequest(httpFailRequestResultMessage);
                }
            }//End of try catch for saving Brand details and Brand-Category association

            successRequestResultMessage = new { message = customMessage };
            OkObjectResult httpOkResult =
                new OkObjectResult(successRequestResultMessage);
            return httpOkResult;

        }//End of UpdateBrandDataAndPhoto



        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string customMessage = "";
            string associatedProductsString = "";

            try
            {
                var foundOneBrand = Database.Brands
                    .Where(input => input.BrandId == id)
                    .Single();


                var foundAssociatedProductsList = Database.Products
                    .Where(input => input.BrandId == id)
                    .ToList();

                var foundAssociatedCBList = Database.CategoryBrands
                    .Where(input => input.BrandId.Equals(id));
                    


                if (foundAssociatedProductsList.Any() == true)
                {

                    foreach (var foundAssociatedProduct in foundAssociatedProductsList)
                    {
                        if (foundAssociatedProduct.BrandId != null)
                        {
                            //brandsAssociatedCat.Add(foundBrand);
                            associatedProductsString += foundAssociatedProduct.ProductName + ", ";
                        }
                    }

                    //Error deleting message
                    customMessage = "Your Brand, " + foundOneBrand.BrandName + " cannot be deleted as it is associated with products, "
                        + associatedProductsString + "Please delete these products first before deleting " + foundOneBrand.BrandName + " brand.";

                    object httpFailRequestResultMessage = new
                    {
                        message = customMessage
                    };

                    return BadRequest(httpFailRequestResultMessage);
                }

                else
                {
                    //Add delete time as soft delete record


                    /*ENABLE THIS WHEN YOU DATA SEED CB WITH DELETED AT*/
                    //foreach (var foundAssociatedCB in foundAssociatedCBList)
                    //{
                    //    foundAssociatedCB.DeletedAt = DateTime.Now;
                    //    Database.Update(foundAssociatedCB);
                    //}

                    foundOneBrand.DeletedAt = DateTime.Now;
                    Database.Update(foundOneBrand);
                    Database.SaveChanges();

                    var successRequestResultMessage = new
                    {
                        message = "Your Brand, " + foundOneBrand.BrandName + " record has been deleted!"
                    };

                    OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);
                    return httpOkResult;
                }
            }

            catch (Exception ex)
            {
                customMessage = "Unexpected error, unable to delete brand record.";
                object httpFailRequestResultMessage = new { message = customMessage + ex.Message };
                return BadRequest(httpFailRequestResultMessage);
            }
        }
    }
}
