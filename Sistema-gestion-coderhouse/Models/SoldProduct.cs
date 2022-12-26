namespace Sistema_gestion_coderhouse.Models
{
    public class SoldProduct
    {
        public int? Id { get; set; }
        public int IdProduct { get; set; }
        public int Stock { get; set; }
        public int? IdOrder { get; set; }
        public Product? Product { get; set; }

        public SoldProduct()
        {

        }
        public SoldProduct(int id, int idProduct, int stock, int idOrder, Product product)
        {
            this.Id = id;
            this.IdProduct = idProduct;
            this.Stock = stock;
            this.IdOrder = idOrder;
            this.Product = product;
        }
    }
}
