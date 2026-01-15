using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataAccessLayer
{
    public class ReportRepository
    {
        DatabaseContext db = new DatabaseContext();

        // 1. EN ÇOK SATAN ÜRÜNLER
        public DataTable EnCokSatilanUrunler()
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // SQL: Products tablosu İngilizce sütunlara sahip (ProductName)
                // Form: 'UrunAdi' bekliyor -> "AS UrunAdi" diyerek çeviriyoruz.
                string sql = @"
                    SELECT 
                        p.ProductName as UrunAdi, 
                        SUM(s.Quantity) as ToplamSatisAdedi
                    FROM Sales s
                    JOIN Products p ON s.ProductId = p.Id
                    GROUP BY p.ProductName
                    ORDER BY ToplamSatisAdedi DESC
                    LIMIT 5";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // 2. AYLIK SATIŞ CİROSU
        public DataTable AylikSatisRaporu()
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();

                // DÜZELTME: ORDER BY kısmına MAX() ekledik.
                // "s.SaleDate" yerine "MAX(s.SaleDate)" diyerek sorunu çözüyoruz.
                string sql = @"
            SELECT 
                DATE_FORMAT(s.SaleDate, '%Y-%M') as Ay, 
                SUM(s.Quantity * s.SatisFiyati) as Ciro
            FROM Sales s
            GROUP BY DATE_FORMAT(s.SaleDate, '%Y-%M')
            ORDER BY MAX(s.SaleDate) DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // 3. KRİTİK STOK LİSTESİ (SORUNUN ÇÖZÜLDÜĞÜ YER)
        public DataTable KritikStokListesi()
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();

                // DİKKAT: Küçük Eşittir (<=) kullanıyoruz ki tam sınırda olanlar da gelsin.
                string sql = @"
                    SELECT 
                        ProductName as UrunAdi, 
                        StockQuantity as StokMiktari, 
                        MinStockLevel as KritikStokSeviyesi 
                    FROM Products 
                    WHERE StockQuantity <= MinStockLevel";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // 4. KÂR / ZARAR RAPORU
        public DataTable KarZararRaporu()
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // SQL: BuyPrice (Alış Fiyatı) İngilizce
                string sql = @"
                    SELECT 
                        p.ProductName as UrunAdi, 
                        SUM(s.Quantity) as ToplamSatis, 
                        SUM((s.SatisFiyati - p.BuyPrice) * s.Quantity) as ToplamKar
                    FROM Sales s
                    JOIN Products p ON s.ProductId = p.Id
                    GROUP BY p.ProductName
                    ORDER BY ToplamKar DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // 5. MÜŞTERİ BAZLI HARCAMA RAPORU
        public DataTable MusteriBazliSatisRaporu()
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // SQL: Name (Müşteri Adı) İngilizce
                string sql = @"
                    SELECT 
                        c.Name as AdSoyad, 
                        SUM(s.Quantity * s.SatisFiyati) as ToplamHarcama
                    FROM Sales s
                    JOIN Customers c ON s.CustomerId = c.Id
                    GROUP BY c.Name
                    ORDER BY ToplamHarcama DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}