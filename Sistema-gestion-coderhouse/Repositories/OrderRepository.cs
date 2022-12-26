using Sistema_gestion_coderhouse.Models;
using System.Data;
using System.Data.SqlClient;


namespace Sistema_gestion_coderhouse.Repositories
{
    public class OrderRepository
    {
        private SqlConnection? connection;
        private String conectionString = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=maurogalf_;" +
            "User Id=maurogalf_;" +
            "Password=SQL0105!;";
        public OrderRepository()
        {
            try
            {
                connection = new SqlConnection(conectionString);
            }
            catch
            {
                throw;
            }
        }
        public List<Order> listOrders()
        {
            List<Order> orders = new List<Order>();
            if (connection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Order order = new Order();
                                order.Id = int.Parse(reader["Id"].ToString());
                                order.Coments= reader["Comentarios"].ToString();
                                order.IdUser = int.Parse(reader["IdUsuario"].ToString());
                                orders.Add(order);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch
            {
                throw;
            }
            return orders;
        }
        public void createOrder(Order order)
        {
            if (connection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Venta(Comentarios, IdUsuario) VALUES(@comentarios, @idUsuario); SELECT @@Identity", connection))
                {
                    connection.Open();
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = order.Coments });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.VarChar) { Value = order.IdUser });
                    order.Id = int.Parse(cmd.ExecuteScalar().ToString()); 
                    if (order.SoldProducts != null && order.SoldProducts.Count > 0)
                    {
                        foreach(SoldProduct soldProduct in order.SoldProducts)
                        {
                            soldProduct.IdOrder = order.Id;
                            registSoldProduct(soldProduct);
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
                connection.Close();
            }
        }
        private SoldProduct registSoldProduct(SoldProduct soldProduct)
        {
            Product? product = ProductRepository.getSimpleProductById(soldProduct.IdProduct, connection);
            if(product != null)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ProductoVendido(Stock, IdProducto, IdVenta) VALUES(@stock, @idProducto, @idVenta); SELECT @@Identity;", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.BigInt) { Value = soldProduct.Stock });
                    cmd.Parameters.Add(new SqlParameter("idProducto", SqlDbType.Int) { Value = soldProduct.IdProduct });
                    cmd.Parameters.Add(new SqlParameter("idVenta", SqlDbType.BigInt) { Value = soldProduct.IdOrder });
                    soldProduct.Id = int.Parse(cmd.ExecuteScalar().ToString());
                }
                stockUpdate(product, soldProduct.Stock);
            }
            else
                {
                    throw new Exception("Producto no encontrado");
                }
                return soldProduct;
        }

        private void stockUpdate(Product product, int q)
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE Producto SET stock = @stock WHERE id = @id", connection))
            {
                cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = product.Stock - q });
                cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = product.Id });
                cmd.ExecuteNonQuery();
            }
        }
        public bool deleteOrder(int id)
        {
            if (connection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                int afectedRows = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM venta WHERE Id=@id", connection))
                {
                    connection.Open();
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
