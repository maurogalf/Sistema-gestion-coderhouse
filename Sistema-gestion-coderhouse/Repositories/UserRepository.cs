using Sistema_gestion_coderhouse.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_gestion_coderhouse.Repositories
{
    public class UserRepository
    {
        private SqlConnection? conection;
        private String conectionString = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=maurogalf_;" +
            "User Id=maurogalf_;" +
            "Password=SQL0105!;";
        public UserRepository()
        {
            try
            {
                conection = new SqlConnection(conectionString);
            }
            catch
            {
                throw;
            }
        }
        public List<User> listUsers()
        {
            List<User> users = new List<User>();
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario", conection))
                {
                    conection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                /*int id, string name, string lastName, string userName, string password, string email*/
                                User user = new User();
                                user.Id = int.Parse(reader["Id"].ToString());
                                user.Name = reader["Nombre"].ToString();
                                user.LastName = reader["Apellido"].ToString();
                                user.UserName = reader["NombreUsuario"].ToString();
                                user.Password = reader["Contraseña"].ToString();
                                user.Email = reader["Mail"].ToString();
                                users.Add(user);
                            }
                        }
                    }
                }
                conection.Close();
            }
            catch
            {
                throw;
            }
            return users;
        }
        public bool deleteUser(int id)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                int afectedRows = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Usuario WHERE Id=@id", conection))
                {
                    conection.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    afectedRows = cmd.ExecuteNonQuery();
                    return afectedRows > 0;
                }
            }
            catch
            {

            }
            return false;
        }
    }
}
