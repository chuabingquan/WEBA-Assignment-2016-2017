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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

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
            //Declarations
            string customMessage = "";
            string brandSearchQuery = "";
            int filterSearchQuery;
            string categoriesAssociated = "";
            string categoryFilterName = "";
            bool check = false;
            List<object> searchBrandList = new List<Object>();
            List<object> associatedCategoryList = new List<object>();

            //Start Try
            try
            {
                //Retrieve search query
                brandSearchQuery = JsonConvert.DeserializeObject<dynamic>(value).searchQuery.Value.ToString();
                brandSearchQuery = brandSearchQuery.Trim();

                //Retrieve category filter query
                filterSearchQuery = Convert.ToInt32(JsonConvert.DeserializeObject<dynamic>(value).filter.Value);

                //Get all brand results that matches search query
                var searchBrandsQueryResult = Database.Brands
                    .Where(input => input.BrandName.Contains(brandSearchQuery) && input.DeletedAt == null)
                    .OrderBy(input => input.BrandId)
                    .ToList();

                //Get all categorybrand results that matches the category filter query
                var categoryBrandQueryResult = Database.CategoryBrands
                    .Where(input => input.CategoryId.Equals(filterSearchQuery) /*&& input.DeletedAt == null*/);

                //Get all categories and brand photo results
                var categoriesQueryResult = Database.Categories
                    .OrderBy(input => input.CategoryId);
                var brandPhotosQueryResult = Database.BrandPhotos
                    .OrderBy(input => input.BrandPhotoId);

                //Get name of category filter query
                if (filterSearchQuery == 0)
                {
                    categoryFilterName = "All Brands";

                    //If 0 is inputted, you remove all filter limitations 
                    categoryBrandQueryResult = Database.CategoryBrands
                        .OrderBy(input => input.CategoryId);
                }

                else
                {
                    categoryFilterName = categoriesQueryResult
                        .Where(input => input.CategoryId == filterSearchQuery).Single().CategoryName;
                }//End of if - else if- else

                //var lastSearchBrandResult = searchBrandsQueryResult.LastOrDefault();

                //If there is more than one brand records based on search query alone
                if (searchBrandsQueryResult.Count > 0)
                {
                    //Start of foreach brand record in brand result based on search query
                    foreach (var searchBrandResult in searchBrandsQueryResult)
                    {
                        //Foreach new brand record based on search query, empty string and List
                        categoriesAssociated = "";
                        associatedCategoryList.Clear();

                        //Start of foreach categorybrand in categorybrand query results based on category filter query
                        foreach (var categoryBrandResult in categoryBrandQueryResult)
                        {
                            //If brand based on search query alone has a record in categoryBrand based on category filter query alone
                            if (categoryBrandResult.BrandId.Equals(searchBrandResult.BrandId))
                            {
                                //Get category associated with brand record based on search query via categoryBrand based on category filter query
                                var associatedCategory = categoriesQueryResult
                                    .Where(input => input.CategoryId.Equals(categoryBrandResult.CategoryId))
                                    .Single();

                                //Assign name of category that is associated with brand record based on search query via categoryBrand record based on category filter query
                                associatedCategoryList.Add(associatedCategory.CategoryName);

                            }//End of if


                            //If get all brands via 0, brand without cat will be excluded. So invoke this to solve the problem
                            //else if (filterSearchQuery == 0 && !(categoryBrandResult.BrandId.Equals(searchBrandResult.BrandId)))
                            //{
                            //    associatedCategoryList.Add("");
                            //}

                            //If this is the last categoryBrand result based on search filter query...
                            if (categoryBrandResult.CategoryBrandId == categoryBrandQueryResult.Last().CategoryBrandId)
                            {
                                //Assign checker to false if associatedCategoryList is empty
                                //This would mean there is no related matches between brand record based on search query and categorybrand record based on category filter search query
                                if (associatedCategoryList.Count > 0)
                                {
                                    check = true;
                                }

                                else
                                {
                                    check = false;
                                }
                            }//End of if

                        }//End of foreach categorybrand in categorybrand query results based on category filter query


                        //If check is true, that would mean that there is an association between current brand and categorybrand in nested foreach loop
                        if (check)
                        {
                            //Obtain brandPhoto result for the current brand in the foreach loop
                            var brandPhotoResult = brandPhotosQueryResult.Where(input => input.BrandId.Equals(searchBrandResult.BrandId)).Single();

                            //Loop out associated category names and add it to a string
                            foreach (var associatedCategoryName in associatedCategoryList)
                            {
                                categoriesAssociated += associatedCategoryName.ToString();

                                //categoriesAssociated += ", ";

                                ////If looped category name is not last in associatedCategoryList, add a comma to the string
                                if (!(associatedCategoryName == associatedCategoryList.Last()))
                                {
                                    categoriesAssociated += ", ";
                                }//End of if
                            }//End of foreach loop to get associated category names and add it to a string

                            //Create and add brand details response and add to searchBrandList
                            searchBrandList.Add(new
                            {
                                brandId = searchBrandResult.BrandId,
                                imageUrl = brandPhotoResult.Url,
                                brandName = searchBrandResult.BrandName,
                                categoryName = categoriesAssociated,
                                noOfProducts = searchBrandResult.NoOfProducts,
                                createdAt = searchBrandResult.CreatedAt,
                                updatedAt = searchBrandResult.UpdatedAt

                            });//Finished creating and adding brand details response and added to searchBrandList
                        }//End of check if IF is true 

                    }//End of foreach brand record in brand result based on search query

                    //If the whole nested foreach loop is completed and searchBrandsList is still empty, return a no results found message
                    if (searchBrandList.Any() == false)
                    {
                        customMessage = "Your search for " + brandSearchQuery + " by " + categoryFilterName + " showed up no results.";
                        object failRequestResultMessage = new
                        {
                            Message = customMessage
                        };

                        return BadRequest(failRequestResultMessage);
                    }
                }//End of if that proceeds when brand results based on search query alone shows up more than 0

                //Else, there is no brand results based on search query alone.
                //Return message about no results
                else
                {
                    customMessage = "Your search for " + brandSearchQuery + " by " + categoryFilterName + " showed up no results.";
                    object failRequestResultMessage = new
                    {
                        Message = customMessage
                    };

                    return BadRequest(failRequestResultMessage);
                }//End of else if brand results based on search query alone is 0
            }//End of try
            //Catch
            catch (Exception ex)
            {
                customMessage = "Your search for " + brandSearchQuery + " by " + categoryFilterName + " showed up no results.";
                object failRequestResultMessage = new
                {
                    Message = customMessage
                };

                return BadRequest(failRequestResultMessage);
            }//End of Try-Catch Block

            //Return search results
            return new JsonResult(searchBrandList);
        }//End of searchBrands Web API method



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


        /*// PUT api/
        [HttpPut("UpdateBrandDataAndPhoto")]
        public async Task<IActionResult> UpdateBrandDataAndPhoto(IList<IFormFile> fileInput, IFormCollection formData)
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

            */

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
        }//End of delete(id)


        // PUT /api/Brands/SaveBrandUpdateInformationIntoSession
        [HttpPut("SaveBrandUpdateInformationIntoSession/{id}")]
        public IActionResult SaveBrandUpdateInformationIntoSession(int id, [FromBody]string value)
        {

            string customMessage = "";
            List<CategoryBrand> changedCBList = new List<CategoryBrand>();

            //Reconstruct a useful object from the input string value. 
            var changedBrandInput = JsonConvert.DeserializeObject<dynamic>(value);

            var changedAssociatedCat = changedBrandInput["selectedAssociatedCat"];
            //var cat = catList.Children()["categoryId"];
            //JToken changedAssociatedCat = changedBrandInput.GetValue("selectedAssociatedCat");
            //var assoCat = changedAssociatedCat.Values;

            Brand brandToBeUpdated = new Brand();
            try
            {
                //Copy out all the brand data into the new Brand instance,
                //brandToBeUpdated.
                brandToBeUpdated.BrandId = id;
                brandToBeUpdated.BrandName = changedBrandInput.brandName.Value;
                brandToBeUpdated.UpdatedAt = DateTime.Now;



                foreach (var associatedCat in changedAssociatedCat)
                {
                    CategoryBrand changedCB = new CategoryBrand();
                    changedCB.BrandId = id;
                    changedCB.CategoryId = Convert.ToInt32(associatedCat.categoryId.Value);
                    changedCBList.Add(changedCB);
                }

                //Saved it into a Session variable, Employee (I can use other names...it is just a name)
                HttpContext.Session.SetString("Brand", JsonConvert.SerializeObject(brandToBeUpdated, Formatting.Indented));
                HttpContext.Session.SetString("AssociatedCB", JsonConvert.SerializeObject(changedCBList, Formatting.Indented));

                customMessage = "Saved brand into session";
            }
            catch (Exception ex)
            {
                customMessage = "Unable to update brand.";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block

            //If there is no runtime error in the try catch block, the code execution
            //should reach here. Sending success message back to the client.
            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = customMessage
            };
            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;
        }//End of SaveBrandUpdateInformationIntoSession() method


        //POST /API/Brands/UploadBrandPhotoAndUpdateBrandData
        [HttpPost("UploadBrandPhotoAndUpdateBrandData")]
        public async Task<IActionResult> UploadBrandPhotoAndUpdateBrandData(IList<IFormFile> fileInput)
        {
            //Retrieve the brand data which is stashed inside the Session, "Brand".
            //http://benjii.me/2015/07/using-sessions-and-httpcontext-in-aspnet5-and-mvc6/

            var brandString = HttpContext.Session.GetString("Brand");
            var changedCBString = HttpContext.Session.GetString("AssociatedCB");

            Brand brandToBeUpdated = JsonConvert.DeserializeObject<Brand>(brandString);
            List<CategoryBrand> changedCBList = JsonConvert.DeserializeObject<List<CategoryBrand>>(changedCBString);

            //Brand brandToBeUpdated = JsonConvert.DeserializeObject<dynamic>(brandString);
            //List<CategoryBrand> changedCBList = JsonConvert.DeserializeObject<dynamic>(changedCBString);


            //Get the current brand data from the database.
            //Also get the current brand Photo information.
            var foundOneBrand = Database.Brands
                .Where(input => input.BrandId == brandToBeUpdated.BrandId)
                .Include(input => input.BrandPhoto).Single();

            //Get current association in CB with brand to be updated
            var foundAssociatedCategoryBrands = Database.CategoryBrands
                .Where(input => input.BrandId == brandToBeUpdated.BrandId /*&& input.DeleteAt == null*/);
            
            //Assign Brand Name
            foundOneBrand.BrandName = brandToBeUpdated.BrandName;
            //Assign Updated Date
            foundOneBrand.UpdatedAt = brandToBeUpdated.UpdatedAt;

            //Remove old associated categories
            foreach (var associatedCategoryBrand in foundAssociatedCategoryBrands)
            {
                Database.CategoryBrands.Remove(associatedCategoryBrand);
            }

            //Add new associated categories
            foreach (var categoryBrand in changedCBList)
            {
                CategoryBrand newCBAssociation = new CategoryBrand();
                newCBAssociation.BrandId = brandToBeUpdated.BrandId;
                newCBAssociation.CategoryId = categoryBrand.CategoryId;
                Database.CategoryBrands.Add(newCBAssociation);
            }


            var oneFile = fileInput[0];
            var fileName = ContentDispositionHeaderValue
                        .Parse(oneFile.ContentDisposition)
                        .FileName
                        .Trim('"');
            string contentType = oneFile.ContentType;
            //Upload the binary file first
            var currentBrandPhoto = await Cloudinary.CloudinaryAPIs.UploadImageToCloudinary<BrandPhoto>(UPLOAD_PRESET, oneFile.OpenReadStream(),
                        contentType, fileName);

            //Delete the existing binary file
            //Obtain the Cloudinary public id value from the foundOneEmployee's EmployeePhoto navigation property
            string originalCloudinaryPublicId = foundOneBrand.BrandPhoto.PublicId;
            //Use the Cloudinary public id value as an input argument for the DeleteImageInCloudinary to delete the binary
            //file resource.
            Boolean result = await Cloudinary.CloudinaryAPIs.DeleteImageInCloudinary(originalCloudinaryPublicId);

            if (currentBrandPhoto.PublicId != "")
            {
                foundOneBrand.BrandPhoto.ImageSize = currentBrandPhoto.ImageSize;
                foundOneBrand.BrandPhoto.Version = currentBrandPhoto.Version;
                foundOneBrand.BrandPhoto.Height = currentBrandPhoto.Height;
                foundOneBrand.BrandPhoto.Width = currentBrandPhoto.Width;
                foundOneBrand.BrandPhoto.PublicId = currentBrandPhoto.PublicId;
                foundOneBrand.BrandPhoto.Url = currentBrandPhoto.Url;
                foundOneBrand.BrandPhoto.SecureUrl = currentBrandPhoto.SecureUrl;
            }
            Database.Brands.Update(foundOneBrand);
            Database.SaveChanges();
            var successRequestResultMessage = new
            {
                Message = "Brand is updated successfully!.",
                NewImageUrl = foundOneBrand.BrandPhoto.Url
            };
            OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);
            return httpOkResult;
        }//End of UploadBrandPhotoAndUpdateBrandData()




        // PUT /api/Brands/SaveBrandUpdateInformationIntoDatabase
        [HttpPut("SaveBrandUpdateInformationIntoDatabase/{id}")]
        public IActionResult SaveBrandUpdateInformationIntoDatabase(int id, [FromBody]string value)
        {
            string customMessage = "";
            //Reconstruct a useful object from the input string value. 
            var changedBrandInput = JsonConvert.DeserializeObject<dynamic>(value);
            var changedCBList = changedBrandInput["selectedAssociatedCat"];

            //Brand brandToBeUpdated = new Brand();
            try
            {
                //Get existing Brand that is to be updated
                var foundOneBrand = Database.Brands
                        .Where(input => input.BrandId == id)
                        .Single();

                //Get current association in CB with brand to be updated
                var foundAssociatedCategoryBrands = Database.CategoryBrands
                    .Where(input => input.BrandId == foundOneBrand.BrandId /*&& input.DeleteAt == null*/);

                //Assign new values to the brand to be updated
                foundOneBrand.BrandName = changedBrandInput.brandName.Value;
                foundOneBrand.UpdatedAt = DateTime.Now;

                //Remove old associated categories
                foreach (var associatedCategoryBrand in foundAssociatedCategoryBrands)
                {
                    Database.CategoryBrands.Remove(associatedCategoryBrand);
                }

                //Add new associated categories
                foreach (var categoryBrand in changedCBList)
                {
                    CategoryBrand newCBAssociation = new CategoryBrand();
                    newCBAssociation.BrandId = foundOneBrand.BrandId;
                    newCBAssociation.CategoryId = Convert.ToInt32(categoryBrand.categoryId.Value);
                    Database.CategoryBrands.Add(newCBAssociation);
                }


                Database.Brands.Update(foundOneBrand);
                Database.SaveChanges();//Without this command, the changes are not committed.
                customMessage = "Saved employee into database.";
            }
            catch (Exception ex)
            {
                customMessage = "Unable to update brand.";
                //Create a fail message anonymous object that has one property, Message.
                //This anonymous object's Message property contains a simple string message
                object httpFailRequestResultMessage = new { Message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }//End of Try..Catch block

            //If there is no runtime error in the try catch block, the code execution
            //should reach here. Sending success message back to the client.
            //******************************************************
            //Construct a custom message for the client
            //Create a success message anonymous object which has a 
            //Message member variable (property)
            var successRequestResultMessage = new
            {
                Message = customMessage
            };
            //Create a HttpOkObjectResult class instance, httpOkResult.
            //When creating the object, provide the previous message object into it.
            OkObjectResult httpOkResult = new OkObjectResult(successRequestResultMessage);
            //Send the HttpOkObjectResult class object back to the client.
            return httpOkResult;

        }//End of SaveBrandUpdateInformationIntoDatabase() method



    }
}
