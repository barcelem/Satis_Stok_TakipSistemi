using System;
using System.Data;
using EntityLayer;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class SalesManager
    {
        SalesRepository repo = new SalesRepository();

        public DataTable GunlukSatisListesi()
        {
            return repo.GunlukSatislariGetir();
        }

        public void SatisYap(Sales s)
        {
            repo.SatisYap(s);
        }
    }
}