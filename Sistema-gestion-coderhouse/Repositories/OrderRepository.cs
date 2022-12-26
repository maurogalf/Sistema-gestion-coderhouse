using Microsoft.AspNetCore.Server.IIS.Core;
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
        public List<Order> listOrders(int? id)
        {
            if (connection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                string query = "SELECT * FROM Venta";
                if(id != null)
                {
                    query += $" WHERE Id=@id";
                }
                using (SqlCommand cmd = new SqlCommand(query , connection))
                {
                    connection.Open();
                    if(id !=null)
                    {
                        cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int) { Value = id});
                    }
                    List<Order> orders = new List<Order>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Order order = new Order();
                                order.Id = int.Parse(reader["Id"].ToString());
                                order.Coments = reader["Comentarios"].ToString();
                                order.IdUser = int.Parse(reader["IdUsuario"].ToString());
                                orders.Add(order);
                            }
                        }
                    }
                    foreach(Order order in orders)
                    {
                        order.SoldProducts = getOrdersSoldProducts((int) order.Id);
                    }
                    return orders;
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
        private List<SoldProduct> getOrdersSoldProducts (int orderId)
        {
            try
            {

            List<SoldProduct> soldProducts = new List<SoldProduct>();
            string query = "SELECT A.Id, A.IdProducto, A.Stock, B.Descripciones, B.PrecioVenta " +
                "FROM ProductoVendido AS A " +
                "INNER JOIN Producto AS B " +
                "ON A.IdProducto = B.Id " +
                "WHERE A.IdVenta = @id";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int) { Value = orderId });
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Order> orders = new List<Order>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            SoldProduct soldProduct = new SoldProduct()
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                IdProduct = int.Parse(reader["IdProducto"].ToString()),
                                Stock = int.Parse(reader["Stock"].ToString()),
                                IdOrder = orderId,
                                Product= new Product()
                                {
                                    Description = reader["Descripciones"].ToString(),
                                    SalePrice = decimal.Parse(reader["PrecioVenta"].ToString())
                                }

                            };
                            soldProducts.Add(soldProduct);
                        }
                    }
                    return soldProducts;
                }
            }
            }
            catch
            {
                throw;
            }
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
