using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Sistema_gestion_coderhouse.Models;

namespace Sistema_gestion_coderhouse.Repositories
{
    public class ProductRepository
    {
        private SqlConnection? conection;
        private String conectionString = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=maurogalf_;" +
            "User Id=maurogalf_;" +
            "Password=SQL0105!;";
        public ProductRepository()
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
        public void createProduct(Product product)
        {
            if(conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES(@descripcion, @costo, @precioVenta, @stock, @idUsuario)", conection))
                {
                    conection.Open();
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = product.Description });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = product.Cost });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = product.SalePrice});
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = product.Stock });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.VarChar) { Value = product.IdUser});
                    cmd.ExecuteNonQuery();

                }
            }
            catch
            {
                throw;
            }
            conection.Close();
        }
        public List<Product> listProducts()
        {
            List<Product> products = new List<Product>();
            if(conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM Producto", conection))
                {
                    conection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Product product = GetProductFromReader(reader);
                                products.Add(product);
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
            return products;
        }

        public Product? getProductById(int id)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Producto WHERE id=@id", conection))
                {
                    conection.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Product product = GetProductFromReader(reader);
                            return product;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                conection.Close();
            }
            catch
            {
                throw;
            }
        }

        public bool deleteProduct(int id)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                int afectedRows = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Producto WHERE Id=@id", conection))
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

        private Product GetProductFromReader(SqlDataReader reader)
        {
            Product product = new Product();
            product.Id = int.Parse(reader["Id"].ToString());
            product.Description = reader["Descripciones"].ToString();
            product.Cost = decimal.Parse(reader["Costo"].ToString());
            product.SalePrice = decimal.Parse(reader["PrecioVenta"].ToString());
            product.Stock = int.Parse(reader["Stock"].ToString());
            product.IdUser = reader["IdUsuario"].ToString();
            return product;
        }
    }
}
