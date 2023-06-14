using Microsoft.Data.SqlClient;
using Models.DataModels;
using Models.ViewModels;
using System.Data;

namespace BooksWebAPI.StoredProcedures
{
    public class UsersStoredProcs
    {
        private IConfiguration _config;
        private SqlConnection conn;
        private SqlCommand sqlCommand;
        private SPHelper helper;

        public UsersStoredProcs(IConfiguration config)
        {
            _config = config;
            helper = new SPHelper();
            conn = new SqlConnection(_config.GetConnectionString("booksDB"));
            conn.Open();
        }

        public long AddUser(RegisterUserVM registerModel)
        {
            sqlCommand = new SqlCommand("spAddUser", conn);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Add("@userType", SqlDbType.Int).Value = registerModel.UserType;
            sqlCommand.Parameters.Add("@firstName", SqlDbType.VarChar, 15).Value = registerModel.FirstName;
            sqlCommand.Parameters.Add("@lastName", SqlDbType.VarChar, 15).Value = registerModel.LastName;
            sqlCommand.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = registerModel.Email;
            sqlCommand.Parameters.Add("@password", SqlDbType.VarChar, 255).Value = registerModel.Password;
            sqlCommand.Parameters.Add("@mobileNo", SqlDbType.VarChar, 10).Value = registerModel.MobileNo;
            sqlCommand.Parameters.Add("@gender", SqlDbType.Int).Value = registerModel.Gender;
            sqlCommand.Parameters.Add("@dateofBirth", SqlDbType.Date).Value = registerModel.DateofBirth;
            sqlCommand.Parameters.Add("@profileImage", SqlDbType.VarChar, 500).Value = registerModel.ProfileImage;
            sqlCommand.Parameters.Add("@publisherName", SqlDbType.VarChar, 200).Value = registerModel.PublisherName;

            var a = sqlCommand.ExecuteScalar();

            conn.Close();

            return long.Parse(a.ToString());
        }

        public object GetUserList(int userType)
        {   
            object userList = new object();

            sqlCommand = new SqlCommand("spGetAllUsers", conn);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Add("@userType", SqlDbType.Int).Value = userType;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

            DataSet ds = new DataSet();

            dataAdapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            switch (userType)
            {
                case 1:
                    userList = helper.CreateListFromTable<Authors>(dt);
                    break;

                case 2:
                    userList = helper.CreateListFromTable<Customers>(dt);
                    break;

                case 3:
                    userList = helper.CreateListFromTable<Publishers>(dt);
                    break;
            }

            conn.Close();

            return userList;
        }

        public RegisterUserVM GetSingleUser(long userId)
        {
            RegisterUserVM user = new RegisterUserVM();

            sqlCommand = new SqlCommand($"Select * from vw_users where UserId = {userId}", conn);
            sqlCommand.CommandType = CommandType.Text;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);

            user = helper.CreateListFromTable<RegisterUserVM>(ds.Tables[0]).FirstOrDefault();

            user.ProfileImage = !string.IsNullOrEmpty(user.ProfileImage) ?
                                    (new Uri(Path.Combine(Globals.serverAddress, user.ProfileImage)).ToString())
                                    : null;

            conn.Close();
            return user;
        }

        public long UpdateUser(RegisterUserVM model)
        {
            sqlCommand = new SqlCommand("spUpdateUser", conn);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Add("@userId", SqlDbType.BigInt).Value = model.UserId;
            sqlCommand.Parameters.Add("@firstName", SqlDbType.VarChar, 15).Value = model.FirstName;
            sqlCommand.Parameters.Add("@lastName", SqlDbType.VarChar, 15).Value = model.LastName;
            sqlCommand.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = model.Email;
            sqlCommand.Parameters.Add("@mobileNo", SqlDbType.VarChar, 10).Value = model.MobileNo;
            sqlCommand.Parameters.Add("@gender", SqlDbType.Int).Value = model.Gender;
            sqlCommand.Parameters.Add("@dateofBirth", SqlDbType.Date).Value = model.DateofBirth;
            sqlCommand.Parameters.Add("@profileImage", SqlDbType.VarChar, 500).Value = model.ProfileImage;
            sqlCommand.Parameters.Add("@publisherName", SqlDbType.VarChar, 200).Value = model.PublisherName;

            var a = sqlCommand.ExecuteScalar();

            conn.Close();
            return long.Parse(a.ToString());
        }

        public void DeleteUser(long userId)
        {
            sqlCommand = new SqlCommand($"Delete From Users Where UserId = {userId}", conn);

            sqlCommand.CommandType = CommandType.Text;

            sqlCommand.ExecuteNonQuery();

            conn.Close();
        }
    }
}
