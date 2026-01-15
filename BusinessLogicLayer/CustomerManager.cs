using System;
using System.Collections.Generic;
using EntityLayer;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class CustomerManager
    {
        CustomerRepository repo = new CustomerRepository();

        public List<Customer> TumMusterileriGetir()
        {
            return repo.Listele();
        }

        public void MusteriEkle(Customer c)
        {
            // İsim boş olamaz kontrolü
            if (string.IsNullOrEmpty(c.AdSoyad))
            {
                throw new Exception("Müşteri adı boş olamaz!");
            }
            c.OlusturmaTarihi = DateTime.Now;
            repo.Ekle(c);
        }

        public void MusteriSil(int id)
        {
            if (id > 0)
            {
                repo.Sil(id);
            }
        }

        // --- İŞTE EKSİK OLAN METOD BU ---
        public void MusteriGuncelle(Customer c)
        {
            // Ad boş mu kontrolü
            if (string.IsNullOrEmpty(c.AdSoyad))
            {
                throw new Exception("Müşteri adı boş olamaz!");
            }
            repo.Guncelle(c);
        }

        // UI katmanı Repository'ye direkt erişemez, bu yüzden aracı oluyoruz
        public Customer MusteriGetir(int id)
        {
            return repo.GetirId(id);
        }
    }
}