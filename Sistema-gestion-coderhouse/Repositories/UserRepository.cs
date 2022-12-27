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
            }
            catch
            {
                throw;
            }
            finally
            {
                conection.Close();
            }
            return users;
        }
        public User? getUserByUserName(string username)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario=@username", conection))
                {
                    conection.Open();
                    cmd.Parameters.Add(new SqlParameter("username", SqlDbType.VarChar) { Value = username });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            User user = new User();
                            user.Id = int.Parse(reader["Id"].ToString());
                            user.Name = reader["Nombre"].ToString();
                            user.LastName = reader["Apellido"].ToString();
                            user.UserName = reader["NombreUsuario"].ToString();
                            user.Password = reader["Contraseña"].ToString();
                            user.Email = reader["Mail"].ToString();
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conection.Close();
            }
        }
        public User? getUserById(int id)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Id=@id", conection))
                {
                    conection.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.VarChar) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            User user = new User();
                            user.Id = int.Parse(reader["Id"].ToString());
                            user.Name = reader["Nombre"].ToString();
                            user.LastName = reader["Apellido"].ToString();
                            user.UserName = reader["NombreUsuario"].ToString();
                            user.Password = reader["Contraseña"].ToString();
                            user.Email = reader["Mail"].ToString();
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conection.Close();
            }
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
                throw;
            }
            finally
            {
                conection.Close();
            }
        }
        public void createUser(User user)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                User? existingUser = this.getUserByUserName(user.UserName);
                if (existingUser == null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Usuario(Nombre, Apellido, NombreUsuario, Contraseña, Mail)" +
                        "VALUES(@nombre, @apellido, @nombreUsuario, @contraseña, @mail)", conection))
                    {
                        conection.Open();
                        cmd.Parameters.Add(new SqlParameter("Nombre", SqlDbType.VarChar) { Value = user.Name });
                        cmd.Parameters.Add(new SqlParameter("Apellido", SqlDbType.VarChar) { Value = user.LastName });
                        cmd.Parameters.Add(new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = user.UserName});
                        cmd.Parameters.Add(new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = user.Password});
                        cmd.Parameters.Add(new SqlParameter("Mail", SqlDbType.VarChar) { Value = user.Email});
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new Exception($"Username {user.UserName} is in use already. Choose another username and try again.");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conection.Close();
            }
        }
        public bool login(string username, string password)
        {
            User? user = this.getUserByUserName(username);
            if(password == null)
            {
                return false;
            }
            return user.Password == password;
        }
        public User? updateUser(int id, User updatedUser)
        {
            if(conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                User? user = this.getUserById(id);
                if(user == null)
                {
                    return null;
                }
                List<string> updatedFields = new List<string>();
                if (user.Name != updatedUser.Name && !string.IsNullOrEmpty(updatedUser.Name))
                {
                    updatedFields.Add("Nombre = @nombre");
                    user.Name = updatedUser.Name;
                }
                if (user.LastName != updatedUser.LastName && !string.IsNullOrEmpty(updatedUser.LastName))
                {
                    updatedFields.Add("Apellido = @apellido");
                    user.LastName = updatedUser.LastName;
                }
                if (user.UserName != updatedUser.UserName && !string.IsNullOrEmpty(updatedUser.UserName))
                {
                    updatedFields.Add("NombreUsuario = @nombreUsuario");
                    user.UserName = updatedUser.UserName;
                }
                if (user.Password != updatedUser.Password && !string.IsNullOrEmpty(updatedUser.Password))
                {
                    updatedFields.Add("Contraseña= @contraseña");
                    user.Password = updatedUser.Password;
                }
                if (user.Email != updatedUser.Email && !string.IsNullOrEmpty(updatedUser.Email))
                {
                    updatedFields.Add("Mail = @mail");
                    user.Email = updatedUser.Email;
                }
                if (updatedFields.Count() == 0)
                {
                    throw new Exception("No field to update.");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Usuario SET {String.Join(", ", updatedFields)} WHERE Id=@id", conection))
                {
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = user.Name });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = user.LastName });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = user.UserName });
                    cmd.Parameters.Add(new SqlParameter("contraseña", SqlDbType.VarChar) { Value = user.Password});
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = user.Email });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int) { Value = id });
                    conection.Open();
                    cmd.ExecuteNonQuery();
                    return user;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conection.Close();
            }
        }
    }
}
