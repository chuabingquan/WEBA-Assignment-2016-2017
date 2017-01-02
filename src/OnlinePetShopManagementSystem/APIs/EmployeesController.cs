using OnlinePetShopManagementSystem.Models;
using OnlinePetShopManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Cloudinary;
using Microsoft.AspNetCore.Authorization;

namespace OnlinePetShopManagementSystem.APIs
{
  [Route("api/[controller]")]
  //[Authorize]
    public class EmployeesController : Controller
  {
    //Declare an ApplicationDbContext type property, Database.
    //This property will represent/reference the actual database.
		public ApplicationDbContext Database { get; }
		private const string UPLOAD_PRESET = "upload_to_employees";
    public EmployeesController()
    { //Initialize, instatiate, activate the Database property
      Database = new ApplicationDbContext();
    }

				//-------------------------------------------------------------------------------------------------------------------------
				/*   PUT WEB API  */
				//-------------------------------------------------------------------------------------------------------------------------
				// PUT api/Students/5
				[HttpPut("{id}")]
				public IActionResult Put(int id, [FromBody]string value)
				{
						string customMessage = "";
						var employeeChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
						//To obtain the full name information, use studentChangeInput.fullName.Value
						//To obtain the email information, use studentChangeInput.email.Value
						var foundOneEmployee = Database.Employees
								.Where(employee => employee.EmployeeId == id).Single();
						foundOneEmployee.FullName = employeeChangeInput.fullName.Value;
						foundOneEmployee.JoinDate = DateTime.ParseExact(
										employeeChangeInput.joinDate.Value,
										"d/M/yyyy", CultureInfo.InvariantCulture);
						foundOneEmployee.UpdatedAt = DateTime.Now;
						try
						{
								Database.Update(foundOneEmployee);
								Database.SaveChanges();
						}
						catch (Exception ex)
						{
	
										customMessage = "Unable to save employee record " ;
										//Create an anonymous types object that has one property, message.
										//This anonymous object's message property contains a simple string message
										object httpFailRequestResultMessage = new { message = customMessage };
										//Return a 400 Bad Request HTTP Response to the client
										return BadRequest(httpFailRequestResultMessage);

						}//End of try .. catch block on saving data

						//Create an anonymous types object which has a 
						//message member variable (property)
						var successRequestResultMessage = new
						{
								message = "Saved employee record"
						};

						//Use the anonymous types object, successRequestResultMessage
						//together with the OkObjectResult class to create an OkObjectResult type
						//instance. Then, assign it to the httpOkResult variable.
						//Web application can use this httpOkResult to create a 200 OK HTTP Response
						//to the client.
						OkObjectResult httpOkResult =
										new OkObjectResult(successRequestResultMessage);

						return httpOkResult;

				}//End of Put Web API



				//-------------------------------------------------------------------------------------------------------------------------
				/*   GET WEB API  */
				//-------------------------------------------------------------------------------------------------------------------------
	// GET: api/Employees
	[HttpGet]
    public JsonResult Get()
    {
      List<object> employeeList = new List<object>();

      var employeeQueryResults = Database.Employees
        .Where(employee => employee.DeletedAt == null)
        .Include(employee => employee.EmployeePhoto);
     
      foreach (var oneEmployee in employeeQueryResults)
      {
        employeeList.Add(new
        {
						employeeId = oneEmployee.EmployeeId,
						fullName = oneEmployee.FullName,
						joinDate = oneEmployee.JoinDate,
						createdAt = oneEmployee.CreatedAt,
						updatedAt = oneEmployee.UpdatedAt,
  					    imageUrl = (oneEmployee.EmployeePhoto==null) ? "" : oneEmployee.EmployeePhoto.Url
        });
      }
      return new JsonResult(employeeList);
    }//end of Get()
		 // GET api/Employees/5
				[HttpGet("{id}")]
				public IActionResult Get(int id)
				{
						try
						{
								var foundEmployee = Database.Employees
										 .Where(employee => employee.EmployeeId == id)
										 .Include(employee => employee.EmployeePhoto).Single();
								var response = new
								{
										employeeId = foundEmployee.EmployeeId,
										fullName = foundEmployee.FullName,
										joinDate = foundEmployee.JoinDate,
										imageUrl = (foundEmployee.EmployeePhoto == null) ? "/images/no_image.jpg" : foundEmployee.EmployeePhoto.Url,
										photoPublicId = (foundEmployee.EmployeePhoto == null) ? "" : foundEmployee.EmployeePhoto.PublicId
								};//end of creation of the response object
								return new JsonResult(response);
						}
						catch (Exception exceptionObject)
						{
								//Create a fail message anonymous object
								//This anonymous object only has one Message property 
								//which contains a simple string message
								object httpFailRequestResultMessage =
																		new { Message = "Unable to obtain employee information." };
								//Return a bad http response message to the client
								return BadRequest(httpFailRequestResultMessage);
						}
				}//End of Get(id) Web API method






				[HttpPost("SaveEmployeePhoto")]
        public async Task<IActionResult> SaveEmployeePhoto(IList<IFormFile> fileInput, IFormCollection formData)
        {
            object successRequestResultMessage;
            string customMessage = "";
            //http://weblogs.asp.net/imranbaloch/file-upload-in-aspnet5-mvc6 

            var oneFile = fileInput[0];

            EmployeePhoto newEmployeePhoto;
            //Collect data from the body of the HTTP request

            var fileName = ContentDispositionHeaderValue
                  .Parse(oneFile.ContentDisposition)
                  .FileName
                  .Trim('"');
            string contentType = oneFile.ContentType;
						//Find one EmployeePhoto record which has an EmployeeId value that matches
						//the given Employee Id value. If there is, delete the image file at cloudinary and
						//then delete the EmployeePhoto record.
            var employeePhotoQueryResults = Database.EmployeePhotos
                .Where(input => input.EmployeeId == Int32.Parse(formData["employeeId"]));
			
            foreach (EmployeePhoto employeePhoto in employeePhotoQueryResults)
            {
								//Delete the image file at Cloudinary
                await CloudinaryAPIs.DeleteImageInCloudinary(employeePhoto.PublicId);
								//Delete the EmployeePhoto record
                Database.EmployeePhotos.Remove(employeePhoto);
            }

            newEmployeePhoto = await  CloudinaryAPIs.
						UploadImageToCloudinary<EmployeePhoto>(UPLOAD_PRESET,oneFile.OpenReadStream(), 
						contentType, fileName);
            //http://benjii.me/2015/07/using-sessions-and-httpcontext-in-aspnet5-and-mvc6/

            if (newEmployeePhoto.PublicId != "")
            {
								newEmployeePhoto.EmployeeId = Int32.Parse(formData["employeeId"]);
                Database.EmployeePhotos.Add(newEmployeePhoto);
                Database.SaveChanges();
                successRequestResultMessage = new
                {
                    message = "Saved employee photo",
										newImageUrl = newEmployeePhoto.Url
								};
            }
            else
            {
                successRequestResultMessage = new
                {
                    message = "Unable to save employee photo"
								};
            }


            OkObjectResult httpOkResult =
                new OkObjectResult(successRequestResultMessage);
            return httpOkResult;

				}//End of SaveEmployeePhoto()


				[HttpPost("SaveEmployeeDataAndPhoto")]
        public async Task<IActionResult> SaveEmployeeDataAndPhoto(IList<IFormFile> fileInput,IFormCollection formData)
        {
            IFormFile oneFile = null;
            object successRequestResultMessage=null;
            string customMessage = "";
            //http://weblogs.asp.net/imranbaloch/file-upload-in-aspnet5-mvc6 
            if (fileInput.Count != 0)
            {
                oneFile = fileInput[0];
            }
            EmployeePhoto newPhoto;
            //Collect data from the body of the HTTP request
            
            Employee newEmployee = new Employee();
						newEmployee.FullName = formData["fullName"];
            //DateTime datatype is not that straightforward.
            //Need to make a DateTime datatype conversion first because
            //the JoinDate of the Employee instance, newEmployee is DateTime datatype.
            newEmployee.JoinDate = DateTime.ParseExact(formData["joinDate"],
            "d/M/yyyy", CultureInfo.InvariantCulture);
           
            try
            {
                Database.Employees.Add(newEmployee);
                Database.SaveChanges();
                customMessage = "Saved employee data ";
            }
            catch (Exception ex)
            {
                    customMessage += "Unable to save employee record. " ;
                    //Create an anonymous type instance that has one property, message.
                    //This anonymous object's message property contains a simple string message
                    object httpFailRequestResultMessage = new { message = customMessage };
                    //Return a HTTP response of Bad Request status
                    //and embed the anonymous type instance's content within the message-body section.
                    return BadRequest(httpFailRequestResultMessage);
            }//End of try .. catch block on saving data

            if (oneFile != null)
            {
                var fileName = ContentDispositionHeaderValue
                      .Parse(oneFile.ContentDisposition)
                      .FileName
                      .Trim('"');
                string contentType = oneFile.ContentType;
								EmployeePhoto employeePhoto = new EmployeePhoto();
								//http://stackoverflow.com/questions/16392751/unable-to-cast-base-class-data-contract-to-derived-class
								employeePhoto = await CloudinaryAPIs.
								UploadImageToCloudinary<EmployeePhoto>(UPLOAD_PRESET, oneFile.OpenReadStream(), 
								contentType, fileName);
								
                
								//http://benjii.me/2015/07/using-sessions-and-httpcontext-in-aspnet5-and-mvc6/

                if (employeePhoto.PublicId != "")
                {
										employeePhoto.EmployeeId = newEmployee.EmployeeId;
                    Database.EmployeePhotos.Add(employeePhoto);
                    Database.SaveChanges();
                    customMessage += "Saved employee photo";
                }
                else
                {
   
                        customMessage = "Unable to save employee photo";
   
                }

            }//end of checking if there is a file for upload
            successRequestResultMessage = new { message = customMessage };
            OkObjectResult httpOkResult =
                new OkObjectResult(successRequestResultMessage);
            return httpOkResult;

        }//End of SaveEmployeeDataAndPhoto()
				 //-------------------------------------------------------------------------------------------------------------------------
				 /*   DELETE WEB API  */
				 //-------------------------------------------------------------------------------------------------------------------------
				 // DELETE api/Employees/5
				[HttpDelete("{id}")]
				public IActionResult Delete(int id)
				{
						string customMessage = "";
						try
						{
								var foundOneEmployee = Database.Employees
								.Single(employee => employee.EmployeeId == id);
								foundOneEmployee.DeletedAt = DateTime.Now;

								//Update the database model
								Database.Update(foundOneEmployee);
								//Tell the db model to commit/persist the changes to the database, 
								//I use the following command.
								Database.SaveChanges();
						}
						catch (Exception ex)
						{
								customMessage = "Unable to delete employee record.";
								object httpFailRequestResultMessage = new { message = customMessage };
								//Return a bad http request message to the client
								return BadRequest(httpFailRequestResultMessage);
						}//End of try .. catch block on manage data

						//Build a custom message for the client
						//Create a success message anonymous type object which has a 
						//message member variable (property)
						var successRequestResultMessage = new
						{
								message = "Deleted employee record"
						};

						//Create a OkObjectResult class instance, httpOkResult.
						//When creating the object, provide the previous message object into it.
						OkObjectResult httpOkResult =
																		new OkObjectResult(successRequestResultMessage);
						//Send the OkObjectResult class object back to the client.
						return httpOkResult;

				}//end of Delete() Web API method
		}//End of Web API controller class
}//End of namespace
