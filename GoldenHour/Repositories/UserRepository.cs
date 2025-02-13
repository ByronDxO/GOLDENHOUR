using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GoldenHour.Model;
namespace GoldenHour.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [GT_User] where usu_username=@username and [usu_password]=@password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [GT_User] where usu_username=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            usu_IdUser = Convert.ToInt32(reader[0]),
                            usu_Username = reader[1].ToString(),
                            usu_Password = string.Empty,
                            usu_Firstname = reader[3].ToString(),
                            usu_LastName = reader[4].ToString(),
                            mail = reader[5].ToString(),
                        };
                    }
                }
            }
            return user;
        }
        public IEnumerable<GT_User> GetAllEmployees()
        {
            var employees = new List<GT_User>();
            using (SqlConnection conn = GetConnection())
            {
                string sql = @"
                    SELECT usu_idUser, usu_username, usu_firstname, usu_lastname -- etc., según tu tabla
                    FROM GT_User";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new GT_User
                            {
                                usu_idUser = Convert.ToInt32(reader["usu_idUser"]),
                                usu_username = reader["usu_username"].ToString(),
                                usu_firstname = reader["usu_firstname"].ToString(),
                                usu_lastName = reader["usu_lastname"].ToString()
                                // Asigna el resto de campos según corresponda
                            });
                        }
                    }
                    conn.Close();
                }
            }
            return employees;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
