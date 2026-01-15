using System;
using System.Drawing;
using System.Windows.Forms;
using DataAccessLayer;

namespace StokTakipUI
{
    public partial class ReportForm : Form
    {
        ReportRepository reportRepo = new ReportRepository();

        // HAFIZA DEĞİŞKENİ: 
        // 1: Çok Satanlar, 2: Aylık Satış, 3: Kritik Stok, 4: Kar/Zarar, 5: Müşteri Raporu
        int aktifRaporNumarasi = 1;

        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            // Görsel Ayarlar
            if (dataGridViewRapor != null)
            {
                dataGridViewRapor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewRapor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            // Açılışta varsayılan olarak Çok Satanları getir
            EnCokSatanlariGetir();
        }

        // --- YENİ EKLENEN KISIM: ACTIVATED ---
        // Bu Event'i Properties -> Şimşek Simgesi -> Activated kısmından bağlamayı unutma!
        private void ReportForm_Activated(object sender, EventArgs e)
        {
            // Ekrana geri dönüldüğünde en son hangi raporda kaldıysak onu yeniler
            switch (aktifRaporNumarasi)
            {
                case 1: EnCokSatanlariGetir(); break;
                case 2: AylıkSatisGetir(); break;
                case 3: KritikStokGetir(); break;
                case 4: KarZararGetir(); break;
                case 5: MusteriRaporuGetir(); break;
            }
        }

        // 1. BUTON: ÇOK SATANLAR
        private void btnCokSatanlar_Click(object sender, EventArgs e)
        {
            aktifRaporNumarasi = 1; // Hafızaya al
            EnCokSatanlariGetir();
        }

        void EnCokSatanlariGetir()
        {
            dataGridViewRapor.DataSource = null;
            dataGridViewRapor.DataSource = reportRepo.EnCokSatilanUrunler();
            TabloyuTemizle();

            if (dataGridViewRapor.Columns["UrunAdi"] != null)
            {
                dataGridViewRapor.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                if (dataGridViewRapor.Columns["ToplamSatisAdedi"] != null)
                {
                    dataGridViewRapor.Columns["ToplamSatisAdedi"].HeaderText = "Toplam Satış Miktarı";
                    dataGridViewRapor.Columns["ToplamSatisAdedi"].DefaultCellStyle.Format = "#,##0.##";
                }
            }
        }

        // 2. BUTON: AYLIK SATIŞ (CİRO)
        private void btnAylikSatis_Click(object sender, EventArgs e)
        {
            aktifRaporNumarasi = 2; // Hafızaya al
            AylıkSatisGetir();
        }

        void AylıkSatisGetir()
        {
            dataGridViewRapor.DataSource = null;
            dataGridViewRapor.DataSource = reportRepo.AylikSatisRaporu();
            TabloyuTemizle();

            if (dataGridViewRapor.Columns["Ciro"] != null)
            {
                dataGridViewRapor.Columns["Ciro"].DefaultCellStyle.Format = "C2";
                dataGridViewRapor.Columns["Ciro"].HeaderText = "Aylık Ciro";
            }
        }

        // 3. BUTON: KRİTİK STOK
        private void btnKritikStok_Click(object sender, EventArgs e)
        {
            aktifRaporNumarasi = 3; // Hafızaya al
            KritikStokGetir();
        }

        void KritikStokGetir()
        {
            dataGridViewRapor.DataSource = null;
            dataGridViewRapor.DataSource = reportRepo.KritikStokListesi();
            TabloyuTemizle();

            if (dataGridViewRapor.Columns["UrunAdi"] != null)
            {
                dataGridViewRapor.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridViewRapor.Columns["StokMiktari"].HeaderText = "Mevcut Stok";
                dataGridViewRapor.Columns["KritikStokSeviyesi"].HeaderText = "Kritik Eşik";

                dataGridViewRapor.Columns["StokMiktari"].DefaultCellStyle.Format = "0.##";

                // Kırmızı uyarı rengi
                dataGridViewRapor.DefaultCellStyle.BackColor = Color.MistyRose;
            }
        }

        // 4. BUTON: KÂR / ZARAR
        private void btnKarZarar_Click(object sender, EventArgs e)
        {
            aktifRaporNumarasi = 4; // Hafızaya al
            KarZararGetir();
        }

        void KarZararGetir()
        {
            dataGridViewRapor.DataSource = null;
            dataGridViewRapor.DataSource = reportRepo.KarZararRaporu();
            TabloyuTemizle();

            if (dataGridViewRapor.Columns["ToplamKar"] != null)
            {
                dataGridViewRapor.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridViewRapor.Columns["ToplamSatis"].HeaderText = "Satılan Miktar";
                dataGridViewRapor.Columns["ToplamSatis"].DefaultCellStyle.Format = "#,##0.##";

                dataGridViewRapor.Columns["ToplamKar"].HeaderText = "Toplam Kâr";
                dataGridViewRapor.Columns["ToplamKar"].DefaultCellStyle.Format = "C2";
                dataGridViewRapor.Columns["ToplamKar"].DefaultCellStyle.ForeColor = Color.DarkGreen;
                dataGridViewRapor.Columns["ToplamKar"].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            }
        }

        // 5. BUTON: MÜŞTERİ RAPORU
        private void btnMusteriRapor_Click(object sender, EventArgs e)
        {
            aktifRaporNumarasi = 5; // Hafızaya al
            MusteriRaporuGetir();
        }

        void MusteriRaporuGetir()
        {
            dataGridViewRapor.DataSource = null;
            dataGridViewRapor.DataSource = reportRepo.MusteriBazliSatisRaporu();
            TabloyuTemizle();

            if (dataGridViewRapor.Columns["ToplamHarcama"] != null)
            {
                dataGridViewRapor.Columns["AdSoyad"].HeaderText = "Müşteri Adı";
                dataGridViewRapor.Columns["ToplamHarcama"].HeaderText = "Toplam Alışveriş";
                dataGridViewRapor.Columns["ToplamHarcama"].DefaultCellStyle.Format = "C2";
                dataGridViewRapor.Columns["ToplamHarcama"].DefaultCellStyle.ForeColor = Color.Blue;
                dataGridViewRapor.Columns["ToplamHarcama"].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            }
        }

        // Yardımcı Metod
        void TabloyuTemizle()
        {
            dataGridViewRapor.DefaultCellStyle.BackColor = Color.White;
            dataGridViewRapor.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewRapor.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
        }
    }
}