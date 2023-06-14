using Microsoft.Data.SqlClient;
using Models.DataModels;
using Models.ViewModels;
using System.Data;

namespace BooksWebAPI.StoredProcedures
{
    public class AuthStoredProcs
    {
        private IConfiguration _config;
        private SqlConnection conn;
        private SqlCommand sqlCommand;
        private SPHelper helper;

        public AuthStoredProcs(IConfiguration config)
        {
            _config = config;
            helper = new SPHelper();
            conn = new SqlConnection(_config.GetConnectionString("booksDB"));
            conn.Open();
        }

        public ResponseVM LoginUser(string email, string password)
        {
            ResponseVM response = new ResponseVM();

            sqlCommand = new SqlCommand($"Select * from Users where Email = '{email}' ", conn);

            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();

            adapter.Fill(ds);

            Users user = helper.CreateListFromTable<Users>(ds.Tables[0]).FirstOrDefault(new Users());
            user.ProfileImage = !string.IsNullOrEmpty(user.ProfileImage) ? 
                                    (new Uri(Path.Combine(Globals.serverAddress, user.ProfileImage)).ToString())
                                    : null;

            if (string.IsNullOrEmpty(user.Email))
            {
                response.statusCode = 404;
                response.message = "Invalid user email";
                return response;
            }

            if(BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                response.statusCode = 200;
                response.message = "Login Success";
                response.data = user;
                return response;
            }

            response.statusCode = 500;
            response.message = "Invalid password";
            return response;

        }
    }
}
