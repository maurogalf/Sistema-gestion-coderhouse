using Sistema_gestion_coderhouse.Models;
using System.Data.SqlClient;

namespace Sistema_gestion_coderhouse.Repositories
{
    public class SoldProductRepository
    {
        private SqlConnection? conection;
        private String conectionString = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=maurogalf_;" +
            "User Id=maurogalf_;" +
            "Password=SQL0105!;";
        public SoldProductRepository()
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
        public List<SoldProduct> listSoldProducts()
        {
            List<SoldProduct> soldProducts = new List<SoldProduct>();
            if (conection == null)
            {
                throw new Exception("Conection failed.");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido", conection))
                {
                    conection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SoldProduct soldProduct = new SoldProduct();
                                soldProduct.Id = int.Parse(reader["Id"].ToString());
                                soldProduct.Stock= int.Parse(reader["Stock"].ToString());
                                soldProduct.IdProduct = int.Parse(reader["IdProducto"].ToString());
                                soldProduct.IdOrder= int.Parse(reader["IdVenta"].ToString());
                                soldProducts.Add(soldProduct);
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
            return soldProducts;
        }
    }
}
