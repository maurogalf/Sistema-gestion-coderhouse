namespace Sistema_gestion_coderhouse.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public string? Coments { get; set; }
        public int IdUser { get; set; }
        public List<SoldProduct>? SoldProducts { get; set; }
        public Order()
        {
        }
        public Order(int id, string coments, int idUser)
        {
            this.Id = id;
            this.Coments = coments;
            this.IdUser = idUser;
            this.SoldProducts = new List<SoldProduct>();
        }
    }
}
