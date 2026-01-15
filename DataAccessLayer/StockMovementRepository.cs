using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using EntityLayer;

namespace DataAccessLayer
{
    public class StockMovementRepository
    {
        DatabaseContext db = new DatabaseContext();

        // 1. HAREKET EKLEME (İngilizce Sütun Adları ile)
        public void HareketEkle(StockMovement m)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // DÜZELTME: 'UrunId' yerine 'ProductId', 'Miktar' yerine 'Quantity' vs. yazıldı.
                string sql = "INSERT INTO StockMovements (ProductId, Quantity, UnitPrice, Description, MovementDate) VALUES (@p1, @p2, @p3, @p4, @p5)";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", m.ProductId);
                komut.Parameters.AddWithValue("@p2", m.Quantity);
                komut.Parameters.AddWithValue("@p3", m.UnitPrice);
                komut.Parameters.AddWithValue("@p4", m.Description);
                komut.Parameters.AddWithValue("@p5", DateTime.Now);

                komut.ExecuteNonQuery();
            }
        }

        // 2. GEÇMİŞİ GETİRME
        public DataTable GecmisiGetir(int urunId)
        {
            DataTable dt = new DataTable();
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();

                // DÜZELTME: Sorguda 'ProductId' kullanıldı.
                string sql = "SELECT * FROM StockMovements WHERE ProductId = @p1 ORDER BY Id DESC";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", urunId);

                MySqlDataAdapter da = new MySqlDataAdapter(komut);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable TumHareketleriGetir()
        {
            DataTable dt = new DataTable();
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = "SELECT * FROM StockMovements ORDER BY Id DESC";
                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                da.Fill(dt);
            }
            return dt;
        }
    }
}