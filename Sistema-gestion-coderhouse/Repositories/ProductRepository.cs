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
                                Product product = new Product();
                                product.Id = int.Parse(reader["Id"].ToString());
                                product.Description = reader["Descripciones"].ToString();
                                product.Cost = decimal.Parse(reader["Costo"].ToString());
                                product.SalePrice = decimal.Parse(reader["PrecioVenta"].ToString());
                                product.Stock = int.Parse(reader["Stock"].ToString());
                                product.IdUser = reader["IdUsuario"].ToString();
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
    }
}
