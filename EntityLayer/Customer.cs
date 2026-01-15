using System;

namespace EntityLayer
{
    // DÜZELTME: : BaseEntity ekledik (Miras alma)
    public class Customer : BaseEntity
    {
        // DÜZELTME: Id satırını SİLDİK (Çünkü artık BaseEntity'den otomatik geliyor)

        public string AdSoyad { get; set; }
        public string Telefon { get; set; }
        public string MusteriTuru { get; set; }
    }
}