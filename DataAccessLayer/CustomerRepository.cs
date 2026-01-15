using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EntityLayer;

namespace DataAccessLayer
{
    public class CustomerRepository : IRepository<Customer>
    {
        DatabaseContext db = new DatabaseContext();

        public List<Customer> Listele()
        {
            List<Customer> musteriler = new List<Customer>();
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("SELECT * FROM Customers", baglanti);
                MySqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    musteriler.Add(new Customer
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        AdSoyad = dr["Name"].ToString(),
                        Telefon = dr["Phone"].ToString(),
                        MusteriTuru = dr["CustomerType"].ToString(),

                        // DÜZELTME: Veritabanındaki 'OlusturmaTarihi' sütununu okuyoruz
                        // Eğer sütun boşsa (NULL), hata vermemesi için kontrol ekledik.
                        OlusturmaTarihi = dr["OlusturmaTarihi"] != DBNull.Value
                                          ? Convert.ToDateTime(dr["OlusturmaTarihi"])
                                          : DateTime.MinValue
                    });
                }
            }
            return musteriler;
        }

        public void Ekle(Customer c)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();

                // DÜZELTME: INSERT sorgusuna 'OlusturmaTarihi' alanını ekledik
                string sql = "INSERT INTO Customers (Name, Phone, CustomerType, OlusturmaTarihi) VALUES (@p1, @p2, @p3, @p4)";

                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", c.AdSoyad);
                komut.Parameters.AddWithValue("@p2", c.Telefon);
                komut.Parameters.AddWithValue("@p3", c.MusteriTuru);

                // C#'tan gelen DateTime.Now verisini veritabanına gönderiyoruz
                komut.Parameters.AddWithValue("@p4", c.OlusturmaTarihi);

                komut.ExecuteNonQuery();
            }
        }

        public void Guncelle(Customer c)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // Güncelleme işleminde genellikle oluşturma tarihi değişmez, o yüzden eklemedim.
                string sql = "UPDATE Customers SET Name=@p1, Phone=@p2, CustomerType=@p3 WHERE Id=@p4";
                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", c.AdSoyad);
                komut.Parameters.AddWithValue("@p2", c.Telefon);
                komut.Parameters.AddWithValue("@p3", c.MusteriTuru);
                komut.Parameters.AddWithValue("@p4", c.Id);
                komut.ExecuteNonQuery();
            }
        }

        public void Sil(int id)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("DELETE FROM Customers WHERE Id=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", id);
                komut.ExecuteNonQuery();
            }
        }

        public Customer GetirId(int id)
        {
            Customer mus = null;
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("SELECT * FROM Customers WHERE Id=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", id);

                MySqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    mus = new Customer
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        AdSoyad = dr["Name"].ToString(),
                        Telefon = dr["Phone"].ToString(),
                        MusteriTuru = dr["CustomerType"].ToString(),

                        // DÜZELTME: Tekli getirme işleminde de tarihi okuyoruz
                        OlusturmaTarihi = dr["OlusturmaTarihi"] != DBNull.Value
                                          ? Convert.ToDateTime(dr["OlusturmaTarihi"])
                                          : DateTime.MinValue
                    };
                }
            }
            return mus;
        }
    }
}