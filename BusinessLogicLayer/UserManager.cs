using System;
using System.Collections.Generic; // Listeleri kullanmak için
using EntityLayer;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class UserManager
    {
        UserRepository userRepo = new UserRepository();

        // --- SESSION (OTURUM) YÖNETİMİ ---
        // Giriş yapan kullanıcının bilgisi program kapanana kadar burada tutulacak.
        // "Static" olduğu için MainForm veya başka formlardan "UserManager.AktifKullanici" diyerek erişebileceksin.
        public static User AktifKullanici { get; set; }

        // 1. GİRİŞ YAP
        public User GirisYap(string kadi, string sifre)
        {
            // İş Kuralı: Boş kontrolü
            if (string.IsNullOrEmpty(kadi) || string.IsNullOrEmpty(sifre))
            {
                return null;
            }

            // Repository'deki metodun adı "GirisYap" olarak güncellendiği için buradan o ismi çağırıyoruz.
            User user = userRepo.GirisYap(kadi, sifre);

            // Eğer veritabanından kullanıcı dönerse (giriş başarılıysa), hafızaya alıyoruz.
            if (user != null)
            {
                AktifKullanici = user;
            }

            return user;
        }

        // 2. KULLANICI EKLE (Admin Paneli İçin Lazım Olacak)
        public void KullaniciEkle(User u)
        {
            // Entity'de isimleri İngilizce (Username, Password) yaptığımız için kontrolleri güncelledim
            if (string.IsNullOrEmpty(u.Username) || string.IsNullOrEmpty(u.Password))
            {
                throw new Exception("Kullanıcı adı ve şifre boş bırakılamaz!");
            }

            userRepo.Ekle(u);
        }

        // GÜNCELLEME METODU
        public void KullaniciGuncelle(User u)
        {
            // Boş alan kontrolü
            if (string.IsNullOrEmpty(u.Username) || string.IsNullOrEmpty(u.Password))
            {
                throw new Exception("Kullanıcı adı ve şifre boş bırakılamaz!");
            }

            // Repo üzerinden güncelle
            userRepo.Guncelle(u);
        }

        // 3. LİSTELEME
        public List<User> TumKullanicilariGetir()
        {
            return userRepo.Listele();
        }

        // 4. SİLME
        public void KullaniciSil(int id)
        {
            userRepo.Sil(id);
        }
    }
}