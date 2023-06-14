using Microsoft.Data.SqlClient.Server;
using Models.DataModels;

namespace BooksWebAPI
{
    public class UploadFileHelper
    {
        private IWebHostEnvironment _webHostEnv;
        private IHttpContextAccessor _contextAccessor;
        public UploadFileHelper(IWebHostEnvironment webHostEnv, IHttpContextAccessor contextAccessor)
        {
            _webHostEnv = webHostEnv;
            _contextAccessor = contextAccessor;

        }
        public string uploadFile(IFormFile file, string userId, string firstName, string lastName)
        {
            /*if (string.IsNullOrWhiteSpace(_webHostEnv.WebRootPath))
            {
                _webHostEnv.WebRootPath = Path.Combine(Directory.GetCurrentDirectory());
            }*/

            string rootFolder = Directory.GetCurrentDirectory();

            string profileImages = Path.Combine(rootFolder, Globals.profileImageFolder);

            string extenstion = Path.GetExtension(file.FileName);
            string filename = userId + "-" + firstName + "-" + lastName + extenstion; //generating file name (2-fname-lname)
            string finalPath = Path.Combine(profileImages, filename);

            string fileCommonName = userId + "-" + firstName + "-" + lastName; //common filename (without extension)

            //for deleting existing user profile images
            DirectoryInfo directoryInfo = new DirectoryInfo(profileImages);
            if (directoryInfo.Exists)
            {
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    if (fileInfo.Name.Contains(fileCommonName))
                    {
                        fileInfo.Delete();
                    }
                }
            }

            //saving uploaded image on server and path in database
            using (FileStream fs = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return Path.Combine(Globals.profileImageFolder, filename);
        }
    }
}
