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
            finally
            {
                conection.Close();
            }
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
                        return products;
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
        public static Product? getStockProductById(int id, SqlConnection connection)
        { 
        if (connection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Stock FROM producto WHERE id = @id", connection))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Product product = new Product()
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Stock = int.Parse(reader["Stock"].ToString())
                            };
                            return product;
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
        }
        public static Product? getSimpleProductById(int id, SqlConnection connection)
        {
            if (connection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Descripciones, PrecioVenta FROM producto WHERE id = @id", connection))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Product product = new Product(reader["Descripciones"].ToString(), decimal.Parse(reader["PrecioVenta"].ToString()));
                            //{
                            //    Description = reader["Descripciones"].ToString(),
                            //    SalePrice = decimal.Parse(reader["PrecioVenta"].ToString())
                            //};
                            return product;
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
                throw;
            }
            finally
            {
                conection.Close();
            }
        }
        public Product? updateProduct(int id, Product updatedProduct)
        {
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                Product? product = this.getProductById(id);
                if (product == null)
                {
                    return null;
                }
                List<string> updatedFields = new List<string>();
                if (product.Description != updatedProduct.Description && !string.IsNullOrEmpty(updatedProduct.Description))
                {
                    updatedFields.Add("Descripciones = @descripciones");
                    product.Description = updatedProduct.Description;
                }
                if (product.Cost != updatedProduct.Cost && updatedProduct.Cost > 0)
                {
                    updatedFields.Add("Costo = @costo");
                    product.Cost = updatedProduct.Cost;
                }
                if (product.SalePrice != updatedProduct.SalePrice && updatedProduct.SalePrice > 0)
                {
                    updatedFields.Add("PrecioVenta = @precioventa");
                    product.SalePrice = updatedProduct.SalePrice;
                }
                if (product.Stock != updatedProduct.Stock && updatedProduct.Stock > 0)
                {
                    updatedFields.Add("Stock= @stock");
                    product.Stock = updatedProduct.Stock;
                }
                if (product.IdUser != updatedProduct.IdUser && !string.IsNullOrEmpty(updatedProduct.IdUser))
                {
                    updatedFields.Add("IdUsuario = @idusuario");
                    product.IdUser = updatedProduct.IdUser;
                }
                if (updatedFields.Count() == 0)
                {
                    throw new Exception("No field to update.");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", updatedFields)} WHERE Id=@id", conection))
                {
                    cmd.Parameters.Add(new SqlParameter("descripciones", SqlDbType.VarChar) { Value = product.Description });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = product.Cost});
                    cmd.Parameters.Add(new SqlParameter("precioventa", SqlDbType.Float) { Value = product.SalePrice});
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = product.Stock });
                    cmd.Parameters.Add(new SqlParameter("idusuario", SqlDbType.BigInt) { Value = product.IdUser});
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int) { Value = id });
                    conection.Open();
                    cmd.ExecuteNonQuery();
                    return product;
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
