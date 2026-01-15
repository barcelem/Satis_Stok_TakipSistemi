namespace EntityLayer
{
    public class SalesDetail : BaseEntity
    {
        public int SatisId { get; set; }
        public int UrunId { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
    }
}