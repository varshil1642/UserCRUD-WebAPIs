using BooksWebAPI.StoredProcedures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BooksWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private UsersStoredProcs _userSPs;
        private UploadFileHelper _uploadFileHelper;
        public UsersController(UsersStoredProcs userSPs, UploadFileHelper uploadFileHelper)
        {
            _userSPs = userSPs;
            _uploadFileHelper = uploadFileHelper;
        }

        [HttpGet]
        [Route("GetSingleUser/{id}")]
        public ResponseVM GetUser(long id)
        {
            ResponseVM response = new ResponseVM();

            try
            {
                response.data = _userSPs.GetSingleUser(id);
                response.statusCode = 200;
            }
            catch (Exception)
            {
                response.statusCode = 500;
                response.message = "Some error occured";
            }

            return response;
        }

        [HttpGet("GetUsers/{id}")]
        public ResponseVM GetAllUsers(int id)
        {
            ResponseVM response = new ResponseVM(); 
            try
            {
                response.data = _userSPs.GetUserList(id);
                response.statusCode = 200;
            }
            catch (Exception)
            {
                response.statusCode = 500;
                response.message = "Some error occured";
            }
            //response.data = null;
            return response;
        }

        [HttpPut("UpdateUser")]
        public ResponseVM UpdateUser([FromForm]RegisterUserVM model)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                if(model.uploadedImage != null)
                {
                    model.ProfileImage = _uploadFileHelper.uploadFile(model.uploadedImage, 
                                                                      model.UserId.ToString(), 
                                                                      model.FirstName, 
                                                                      model.LastName);
                }

                if (string.IsNullOrEmpty(model.PublisherName))
                {
                    model.PublisherName = string.Empty;
                }

                long userId = _userSPs.UpdateUser(model);
                
                if(userId > 0)
                {
                    response.statusCode = 200;
                    response.message = "User updated successfully";
                }
                else
                {
                    response.statusCode = 500;
                    response.message = "This email address is already taken";
                }
            }
            catch (Exception)
            {
                response.statusCode = 500;
                response.message = "Some error occured";
            }

            return response;
        }

        [HttpDelete("DeleteUser/{id}")]
        public ResponseVM Delete(long id)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                _userSPs.DeleteUser(id);
                response.statusCode = 200;
                response.message = "User deleted successfully";
            }
            catch (Exception)
            {
                response.statusCode = 500;
                response.message = "Some error occured";
            }
            return response;
        }
    }
}
