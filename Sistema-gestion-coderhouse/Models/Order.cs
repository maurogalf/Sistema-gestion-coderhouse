namespace Sistema_gestion_coderhouse.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Coments { get; set; }
        public string IdUser { get; set; }
        public Order()
        {

        }
        public Order(int id, string coments, string idUser)
        {
            this.Id = id;
            this.Coments = coments;
            this.IdUser = idUser;
        }
    }
}
