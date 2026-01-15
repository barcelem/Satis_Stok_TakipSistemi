using System;
namespace EntityLayer
{
    public class Sales : BaseEntity
    {
        public int UrunId { get; set; }
        public int MusteriId { get; set; }
        public decimal Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamTutar { get; set; }
        public DateTime Tarih { get; set; }
        public decimal SatisFiyati { get; set; }
    }
}