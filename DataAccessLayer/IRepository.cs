using System.Collections.Generic;

namespace DataAccessLayer
{
    // <T> ifadesi "Generic" demektir. Yani bu kalıbı hem Ürün, hem Müşteri için kullanabiliriz.
    public interface IRepository<T>
    {
        List<T> Listele();
        void Ekle(T t);
        void Sil(int id);
        void Guncelle(T t);
        T GetirId(int id);
    }
}