using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EntityLayer;

namespace DataAccessLayer
{
    public class UserRepository : IRepository<User>
    {
        DatabaseContext db = new DatabaseContext();

        // 1. LİSTELEME
        public List<User> Listele()
        {
            List<User> kullanicilar = new List<User>();
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("SELECT * FROM Users", baglanti);
                MySqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    kullanicilar.Add(new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = dr["Username"].ToString(),

                        // --- DÜZELTME: Yorum satırını kaldırdık ---
                        // Giriş ekranında kontrol yapabilmek için şifreyi çekmek zorundayız.
                        Password = dr["Password"].ToString(),

                        Role = dr["Role"].ToString(),
                        OlusturmaTarihi = dr["OlusturmaTarihi"] != DBNull.Value
                                          ? Convert.ToDateTime(dr["OlusturmaTarihi"])
                                          : DateTime.MinValue
                    });
                }
            }
            return kullanicilar;
        }

        // 2. EKLEME (Tarih Eklendi)
        public void Ekle(User t)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // OlusturmaTarihi sütununu da ekledik
                string sql = "INSERT INTO Users (Username, Password, Role, OlusturmaTarihi) VALUES (@p1, @p2, @p3, @p4)";
                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", t.Username);
                komut.Parameters.AddWithValue("@p2", t.Password);
                komut.Parameters.AddWithValue("@p3", t.Role);
                komut.Parameters.AddWithValue("@p4", DateTime.Now); // Şu anki zaman
                komut.ExecuteNonQuery();
            }
        }

        // 3. SİLME
        public void Sil(int id)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("DELETE FROM Users WHERE Id=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", id);
                komut.ExecuteNonQuery();
            }
        }

        // 4. GÜNCELLEME
        public void Guncelle(User t)
        {
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                string sql = "UPDATE Users SET Username=@p1, Password=@p2, Role=@p3 WHERE Id=@p4";
                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", t.Username);
                komut.Parameters.AddWithValue("@p2", t.Password);
                komut.Parameters.AddWithValue("@p3", t.Role);
                komut.Parameters.AddWithValue("@p4", t.Id);
                komut.ExecuteNonQuery();
            }
        }

        // 5. ID İLE GETİRME (Admin panelinde düzenleme yaparken lazım olacak)
        public User GetirId(int id)
        {
            User user = null;
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                MySqlCommand komut = new MySqlCommand("SELECT * FROM Users WHERE Id=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", id);
                MySqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    user = new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = dr["Username"].ToString(),
                        Password = dr["Password"].ToString(),
                        Role = dr["Role"].ToString(),
                        OlusturmaTarihi = dr["OlusturmaTarihi"] != DBNull.Value
                                          ? Convert.ToDateTime(dr["OlusturmaTarihi"])
                                          : DateTime.MinValue
                    };
                }
            }
            return user;
        }

        // 6. GİRİŞ KONTROLÜ (Login Ekranı İçin)
        public User GirisYap(string kadi, string sifre)
        {
            User user = null;
            using (var baglanti = db.GetConnection())
            {
                baglanti.Open();
                // Username ve Password eşleşiyor mu diye bakıyoruz
                string sql = "SELECT * FROM Users WHERE Username=@p1 AND Password=@p2";
                MySqlCommand komut = new MySqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@p1", kadi);
                komut.Parameters.AddWithValue("@p2", sifre);

                MySqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    user = new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Username = dr["Username"].ToString(),
                        Role = dr["Role"].ToString()
                    };
                }
            }
            return user;
        }
    }
}