using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EntityLayer;

namespace DataAccessLayer
{
    public class ProductRepository
    {
        DatabaseContext db = new DatabaseContext();

        // 1. LİSTELEME
        public List<Product> Listele()
        {
            List<Product> urunler = new List<Product>();
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = @"
                    SELECT 
                        Id, ProductName, ProductCode, StockQuantity, UnitType, 
                        BuyPrice, SellPrice, MinStockLevel, CreationDate 
                    FROM Products ORDER BY Id DESC";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                MySqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    urunler.Add(new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        UrunAdi = dr["ProductName"].ToString(),
                        UrunKodu = dr["ProductCode"] != DBNull.Value ? dr["ProductCode"].ToString() : "",
                        StokMiktari = Convert.ToDecimal(dr["StockQuantity"]),
                        Birim = dr["UnitType"] != DBNull.Value ? dr["UnitType"].ToString() : "Adet",
                        AlisFiyati = Convert.ToDecimal(dr["BuyPrice"]),
                        SatisFiyati = Convert.ToDecimal(dr["SellPrice"]),
                        KritikStokSeviyesi = Convert.ToInt32(dr["MinStockLevel"]),
                        OlusturmaTarihi = dr["CreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreationDate"]) : DateTime.Now
                    });
                }
            }
            return urunler;
        }

        // 2. EKLEME
        public void Ekle(Product t)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = "INSERT INTO Products (ProductName, ProductCode, StockQuantity, UnitType, BuyPrice, SellPrice, MinStockLevel, CreationDate) VALUES (@p1, @p6, @p2, @p8, @p3, @p4, @p5, @p7)";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", t.UrunAdi);
                komut.Parameters.AddWithValue("@p2", t.StokMiktari);
                komut.Parameters.AddWithValue("@p3", t.AlisFiyati);
                komut.Parameters.AddWithValue("@p4", t.SatisFiyati);
                komut.Parameters.AddWithValue("@p5", t.KritikStokSeviyesi);
                komut.Parameters.AddWithValue("@p6", t.UrunKodu ?? "KODSUZ");
                komut.Parameters.AddWithValue("@p7", DateTime.Now);
                komut.Parameters.AddWithValue("@p8", t.Birim ?? "Adet");

                komut.ExecuteNonQuery();
            }
        }

        // 3. SİLME
        public void Sil(int id)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("DELETE FROM Products WHERE Id=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", id);
                komut.ExecuteNonQuery();
            }
        }

        // 4. GÜNCELLEME
        public void Guncelle(Product t)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = "UPDATE Products SET ProductCode=@p7, ProductName=@p1, StockQuantity=@p2, BuyPrice=@p3, SellPrice=@p4, MinStockLevel=@p5 WHERE Id=@p6";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", t.UrunAdi);
                komut.Parameters.AddWithValue("@p2", t.StokMiktari);
                komut.Parameters.AddWithValue("@p3", t.AlisFiyati);
                komut.Parameters.AddWithValue("@p4", t.SatisFiyati);
                komut.Parameters.AddWithValue("@p5", t.KritikStokSeviyesi);
                komut.Parameters.AddWithValue("@p6", t.Id);
                komut.Parameters.AddWithValue("@p7", t.UrunKodu);

                komut.ExecuteNonQuery();
            }
        }

        // 5. TEK ÜRÜN GETİRME
        public Product GetirId(int id)
        {
            Product urun = null;
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = @"SELECT Id, ProductName, ProductCode, StockQuantity, UnitType, BuyPrice, SellPrice, MinStockLevel, CreationDate FROM Products WHERE Id=@p1";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", id);

                MySqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    urun = new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        UrunAdi = dr["ProductName"].ToString(),
                        UrunKodu = dr["ProductCode"] != DBNull.Value ? dr["ProductCode"].ToString() : "",
                        StokMiktari = Convert.ToDecimal(dr["StockQuantity"]),
                        Birim = dr["UnitType"] != DBNull.Value ? dr["UnitType"].ToString() : "Adet",
                        AlisFiyati = Convert.ToDecimal(dr["BuyPrice"]),
                        SatisFiyati = Convert.ToDecimal(dr["SellPrice"]),
                        KritikStokSeviyesi = Convert.ToInt32(dr["MinStockLevel"]),
                        OlusturmaTarihi = dr["CreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreationDate"]) : DateTime.Now
                    };
                }
            }
            return urun;
        }

        // 6. STOK DÜŞME
        public void StokAdediGuncelle(int urunId, decimal dusulecekAdet)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("UPDATE Products SET StockQuantity = StockQuantity - @p1 WHERE Id = @p2", baglanti);
                komut.Parameters.AddWithValue("@p1", dusulecekAdet);
                komut.Parameters.AddWithValue("@p2", urunId);
                komut.ExecuteNonQuery();
            }
        }

        // 7. ARAMA
        public List<Product> Ara(string aranacakKelime)
        {
            List<Product> urunler = new List<Product>();
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = @"SELECT * FROM Products WHERE ProductName LIKE @p1 OR ProductCode LIKE @p1";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", "%" + aranacakKelime + "%");

                MySqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    urunler.Add(new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        UrunAdi = dr["ProductName"].ToString(),
                        UrunKodu = dr["ProductCode"] != DBNull.Value ? dr["ProductCode"].ToString() : "",
                        StokMiktari = Convert.ToDecimal(dr["StockQuantity"]),
                        Birim = dr["UnitType"] != DBNull.Value ? dr["UnitType"].ToString() : "Adet",
                        AlisFiyati = Convert.ToDecimal(dr["BuyPrice"]),
                        SatisFiyati = Convert.ToDecimal(dr["SellPrice"]),
                        KritikStokSeviyesi = Convert.ToInt32(dr["MinStockLevel"]),
                        OlusturmaTarihi = dr["CreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreationDate"]) : DateTime.Now
                    });
                }
            }
            return urunler;
        }

        // 8. STOK VE FİYAT GÜNCELLEME
        public void StokVeFiyatGuncelle(int urunId, decimal yeniStokAdedi, decimal yeniAlisFiyati)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = "UPDATE Products SET StockQuantity=@p1, BuyPrice=@p2 WHERE Id=@p3";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", yeniStokAdedi);
                komut.Parameters.AddWithValue("@p2", yeniAlisFiyati);
                komut.Parameters.AddWithValue("@p3", urunId);
                komut.ExecuteNonQuery();
            }
        }

        // 9. ÜRÜN ID BULMA (YENİ EKLENEN ÖZELLİK)
        // Bu özellik sayesinde eklediğin ürünün ID'sini anında bulup geçmişe kaydedebileceğiz.
        public int UrunIdGetir(string urunKodu)
        {
            int id = 0;
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = "SELECT Id FROM Products WHERE ProductCode = @p1"; // Ürün Kodu veritabanında ProductCode diye geçer
                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", urunKodu);

                object sonuc = komut.ExecuteScalar();
                if (sonuc != null)
                {
                    id = Convert.ToInt32(sonuc);
                }
            }
            return id;
        }
    }
}