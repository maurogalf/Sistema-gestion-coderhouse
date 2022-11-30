using Sistema_gestion_coderhouse.Models;
using System.Data.SqlClient;

namespace Sistema_gestion_coderhouse.Repositories
{
    public class OrderRepository
    {
        private SqlConnection? conection;
        private String conectionString = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=maurogalf_;" +
            "User Id=maurogalf_;" +
            "Password=SQL0105!;";
        public OrderRepository()
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
        public List<Order> listOrders()
        {
            List<Order> orders = new List<Order>();
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta", conection))
                {
                    conection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Order order = new Order();
                                order.Id = int.Parse(reader["Id"].ToString());
                                order.Coments= reader["Comentarios"].ToString();
                                order.IdUser = reader["IdUsuario"].ToString();
                                orders.Add(order);
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
            return orders;
        }
    }
}
