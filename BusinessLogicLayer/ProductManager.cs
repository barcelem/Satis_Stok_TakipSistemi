using System.Collections.Generic;
using EntityLayer;
using DataAccessLayer;
using System;

namespace BusinessLogicLayer
{
    public class ProductManager
    {
        ProductRepository repo = new ProductRepository();

        public List<Product> TumUrunleriGetir()
        {
            return repo.Listele();
        }

        public void UrunEkle(Product p)
        {
            if (p.AlisFiyati < 0 || p.SatisFiyati < 0)
                throw new Exception("Fiyat 0'dan küçük olamaz!");

            if (string.IsNullOrEmpty(p.UrunAdi))
                throw new Exception("Ürün adı boş olamaz!");

            repo.Ekle(p);
        }

        public void UrunSil(int id)
        {
            if (id > 0) repo.Sil(id);
        }

        public void StokDus(int urunId, decimal satilanAdet)
        {
            repo.StokAdediGuncelle(urunId, satilanAdet);
        }

        public List<Product> UrunAra(string kelime)
        {
            return repo.Ara(kelime);
        }

        public void UrunGuncelle(Product p)
        {
            if (p.AlisFiyati < 0 || p.SatisFiyati < 0)
                throw new Exception("Fiyat 0'dan küçük olamaz!");
            repo.Guncelle(p);
        }

        public Product UrunGetir(int id)
        {
            return repo.GetirId(id);
        }

        public void StokEkle(int urunId, decimal eklenecekMiktar, decimal yeniGirisFiyati)
        {
            // 1. Mevcut ürünü bul
            Product mevcutUrun = repo.GetirId(urunId);

            if (mevcutUrun != null)
            {
                // 2. MATEMATİK: Ağırlıklı Ortalama (Decimal hassasiyetiyle)
                decimal eskiToplamDeger = mevcutUrun.StokMiktari * mevcutUrun.AlisFiyati;
                decimal yeniGirisDegeri = eklenecekMiktar * yeniGirisFiyati;

                decimal toplamDeger = eskiToplamDeger + yeniGirisDegeri;
                decimal toplamAdet = mevcutUrun.StokMiktari + eklenecekMiktar;

                // Bölme hatası (DivideByZero) olmasın diye kontrol
                decimal yeniOrtalamaMaliyet = (toplamAdet > 0) ? (toplamDeger / toplamAdet) : yeniGirisFiyati;

                // 3. Veritabanına kaydet (Yeni stok ve yeni ortalama maliyet)
                repo.StokVeFiyatGuncelle(urunId, toplamAdet, yeniOrtalamaMaliyet);
            }
        }

        // --- YENİ EKLENEN ÖZELLİK ---
        // Formdan gelen isteği Repository'ye ileten köprü metod
        public int UrunIdGetir(string urunKodu)
        {
            return repo.UrunIdGetir(urunKodu);
        }
    }
}