using BooksWebAPI.StoredProcedures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataModels;
using Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BooksWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private UsersStoredProcs _userSPs;
        private AuthStoredProcs _authSP;
        private IConfiguration _config;
        private UploadFileHelper _uploadFileHelper;
        public AuthController(UsersStoredProcs userSPs, AuthStoredProcs authSP, IConfiguration config, 
            UploadFileHelper uploadFileHelper)
        {
            _userSPs = userSPs;
            _authSP = authSP;
            _config = config;
            _uploadFileHelper = uploadFileHelper;
        }

        [HttpPost]
        [Route("AddUser")]
        public ResponseVM AddUser([FromForm]RegisterUserVM model)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                Guid guid = Guid.NewGuid();
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                if (model.uploadedImage != null)
                {
                    model.ProfileImage = _uploadFileHelper.uploadFile(model.uploadedImage, guid.ToString(), model.FirstName,
                                                                                              model.LastName);
                }
                long userId = _userSPs.AddUser(model);

                if (userId > 0)
                {
                    response.statusCode = 200;
                    response.message = "User added successfully";
                }
                else
                {
                    response.statusCode = 500;
                    response.message = "Email address already exist";
                }
            }
            catch(Exception)
            {
                response.statusCode = 500;
                response.message = "Some error occured";
            }
            return response;
        }

        
        [HttpPost]
        [Route("Login")]
        public ResponseVM LoginUser(LoginVM model)
        {
            ResponseVM response = new ResponseVM();

            try
            {
                response = _authSP.LoginUser(model.email, model.password);
                if(response.statusCode == 200)
                {
                    JwtSettings jwtSetting = _config.GetSection("Jwt").Get<JwtSettings>();

                    var token = JwtTokenHelper.GenerateToken(jwtSetting, (Users)response.data!);

                    dynamic responseObj = new
                    {
                        user = response.data,
                        token = token
                    };

                    response.data = responseObj;
                }
                return response;
            }
            catch (Exception)
            {
                response.statusCode = 500;
                response.message = "Some error occured";
            }

            return response;
        }

        [HttpGet]
        [Route("BadRequest")]
        public ResponseVM BadRequest()
        {
            ResponseVM response = new ResponseVM();

            response.statusCode = 500;
            response.message = "Some error occured";

            return response;
        }
    }
}
