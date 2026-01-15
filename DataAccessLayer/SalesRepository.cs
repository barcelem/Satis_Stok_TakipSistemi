using System;
using System.Data;
using MySql.Data.MySqlClient;
using EntityLayer;

namespace DataAccessLayer
{
    public class SalesRepository
    {
        DatabaseContext db = new DatabaseContext();

        public DataTable GunlukSatislariGetir()
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = @"
                    SELECT 
                        s.Id, 
                        p.ProductName as UrunAdi,     
                        c.Name as Musteri,            
                        s.Quantity as Adet,           
                        s.SatisFiyati as BirimFiyat,  
                        (s.Quantity * s.SatisFiyati) as ToplamTutar, 
                        s.SaleDate as Tarih           
                    FROM Sales s
                    JOIN Products p ON s.ProductId = p.Id   
                    JOIN Customers c ON s.CustomerId = c.Id 
                    WHERE DATE(s.SaleDate) = CURDATE()
                    ORDER BY s.SaleDate DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void SatisYap(Sales s)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();

                string sqlSatis = @"
                    INSERT INTO Sales (ProductId, CustomerId, Quantity, SatisFiyati, SaleDate) 
                    VALUES (@uid, @mid, @adet, @fiyat, @tarih)";

                MySqlCommand cmd = new MySqlCommand(sqlSatis, baglanti);
                cmd.Parameters.AddWithValue("@uid", s.UrunId);
                cmd.Parameters.AddWithValue("@mid", s.MusteriId);

                // DEĞİŞİKLİK: Adet artık 1.5 gibi virgüllü olabilir
                cmd.Parameters.AddWithValue("@adet", s.Adet);

                cmd.Parameters.AddWithValue("@fiyat", s.SatisFiyati);
                cmd.Parameters.AddWithValue("@tarih", s.Tarih);

                cmd.ExecuteNonQuery();

                // Stoktan düşme işlemi (ondalıklı düşecek)
                string sqlStok = "UPDATE Products SET StockQuantity = StockQuantity - @adet WHERE Id = @uid";
                MySqlCommand cmdStok = new MySqlCommand(sqlStok, baglanti);
                cmdStok.Parameters.AddWithValue("@adet", s.Adet);
                cmdStok.Parameters.AddWithValue("@uid", s.UrunId);
                cmdStok.ExecuteNonQuery();
            }
        }
    }
}