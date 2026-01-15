using System; // DateTime kullanmak için bu kütüphane şart

namespace EntityLayer
{
    public class Product : BaseEntity // Kalıtım tamam ✅
    {
        public string UrunAdi { get; set; }
        public string UrunKodu { get; set; }
        public decimal StokMiktari { get; set; }
        public decimal AlisFiyati { get; set; }
        public decimal SatisFiyati { get; set; }
        public int KritikStokSeviyesi { get; set; }
        public string Birim { get; set; }


    }
}