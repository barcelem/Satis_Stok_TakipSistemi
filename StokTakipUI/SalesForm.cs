using BusinessLogicLayer;
using EntityLayer;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace StokTakipUI
{
    public partial class SalesForm : Form
    {
        SalesManager salesManager = new SalesManager();
        ProductManager productManager = new ProductManager();
        CustomerManager customerManager = new CustomerManager();

        public SalesForm()
        {
            InitializeComponent();
        }

        // --- EKRAN ÖNE GELİNCE ÇALIŞACAK KISIM (YENİ) ---
        private void SalesForm_Activated(object sender, EventArgs e)
        {
            // Başka ekranda ürün/müşteri eklendiyse buraya gelince listeyi tazele
            VerileriGetir();
            Listele(); // Satış geçmişini de tazele
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Tablo Ayarları
                if (dataGridView1 != null)
                {
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }

                // Olay Bağlama (Kod ile yapıyoruz ki tasarımda uğraşma)
                if (cmbUrun != null)
                    cmbUrun.SelectedIndexChanged += new EventHandler(cmbUrun_SelectedIndexChanged);

                // Verileri Yükle
                VerileriGetir();
                Listele();
                FiyatiKutuyaGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Açılış Hatası: " + ex.Message);
            }
        }

        // --- YENİ METOD: VERİLERİ TAZELEME ---
        void VerileriGetir()
        {
            try
            {
                // 1. Şu an seçili olanların ID'sini sakla (Liste yenilenince kaybolmasın diye)
                object seciliUrunId = cmbUrun.SelectedValue;
                object seciliMusteriId = cmbMusteri.SelectedValue;

                // 2. Ürünleri Yenile
                var urunListesi = productManager.TumUrunleriGetir();
                if (urunListesi != null)
                {
                    cmbUrun.DataSource = urunListesi;
                    cmbUrun.DisplayMember = "UrunAdi";
                    cmbUrun.ValueMember = "Id";
                }

                // 3. Müşterileri Yenile
                var musteriListesi = customerManager.TumMusterileriGetir();
                if (musteriListesi != null)
                {
                    cmbMusteri.DataSource = musteriListesi;
                    cmbMusteri.DisplayMember = "AdSoyad";
                    cmbMusteri.ValueMember = "Id";
                }

                // 4. Eski seçimleri geri yükle (Eğer hala listede varsa)
                if (seciliUrunId != null) cmbUrun.SelectedValue = seciliUrunId;
                if (seciliMusteriId != null) cmbMusteri.SelectedValue = seciliMusteriId;
            }
            catch { }
        }

        private void cmbUrun_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiyatiKutuyaGetir();
        }

        void FiyatiKutuyaGetir()
        {
            try
            {
                if (cmbUrun != null && cmbUrun.SelectedValue != null)
                {
                    int urunId;
                    // TryParse: Eğer seçilen değer sayı değilse (System.RowView gibiyse) hata vermesin
                    if (int.TryParse(cmbUrun.SelectedValue.ToString(), out urunId))
                    {
                        Product secilenUrun = productManager.UrunGetir(urunId);

                        if (secilenUrun != null)
                        {
                            if (txtSatisFiyati != null)
                                txtSatisFiyati.Text = secilenUrun.SatisFiyati.ToString();

                            if (lblMevcutStok != null)
                            {
                                string birim = secilenUrun.Birim ?? "";
                                lblMevcutStok.Text = secilenUrun.StokMiktari.ToString("#,##0.##") + " " + birim;

                                // Stok Azaldı Uyarısı
                                if (secilenUrun.StokMiktari <= secilenUrun.KritikStokSeviyesi)
                                    lblMevcutStok.ForeColor = System.Drawing.Color.Red;
                                else
                                    lblMevcutStok.ForeColor = System.Drawing.Color.Black;
                            }

                            if (lblAlisFiyati != null)
                            {
                                lblAlisFiyati.Text = secilenUrun.AlisFiyati.ToString("C2");
                            }
                        }
                    }
                }
            }
            catch { }
        }

        void Listele()
        {
            try
            {
                DataTable dt = salesManager.GunlukSatisListesi();
                if (dt == null) return;

                dataGridView1.DataSource = dt;

                // --- 1. ORTAK GÖRSEL TASARIM (DİĞER FORMLARLA AYNI) ---
                void GridStilUygula(DataGridView dgv)
                {
                    // Başlıklar -> KALIN, SİYAH, GRİ ARKA PLAN
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Satırlar -> NORMAL (Kalın Değil)
                    dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                    dgv.DefaultCellStyle.ForeColor = Color.Black;

                    // Genel Ayarlar
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgv.RowHeadersVisible = false; // Soldaki boşluğu gizle
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }

                // Tasarımı Uygula
                GridStilUygula(dataGridView1);

                // --- 2. CİRO HESAPLAMA (MEVCUT MANTIK) ---
                decimal gunlukCiro = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ToplamTutar"] != DBNull.Value)
                        gunlukCiro += Convert.ToDecimal(row["ToplamTutar"]);
                }
                if (lblCiro != null) lblCiro.Text = gunlukCiro.ToString("C2");

                // --- 3. SÜTUN AYARLARI VE FORMATLAMA ---
                if (dataGridView1.Columns.Count > 0)
                {
                    // Ürün Adı
                    if (dataGridView1.Columns.Contains("UrunAdi"))
                    {
                        dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün";
                        // Ürün adı alanı genişlesin
                        dataGridView1.Columns["UrunAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    // Müşteri Adı
                    if (dataGridView1.Columns.Contains("Musteri"))
                        dataGridView1.Columns["Musteri"].HeaderText = "Müşteri";

                    // Adet / Miktar
                    if (dataGridView1.Columns.Contains("Adet"))
                    {
                        dataGridView1.Columns["Adet"].HeaderText = "Miktar";
                        dataGridView1.Columns["Adet"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        // Adet tam sayı ise N0, ondalıklı ise N2 yapabilirsiniz
                        dataGridView1.Columns["Adet"].DefaultCellStyle.Format = "#,##0.##";
                    }

                    // Birim Fiyat (Veritabanından gelen isme göre kontrol)
                    if (dataGridView1.Columns.Contains("BirimFiyat"))
                    {
                        dataGridView1.Columns["BirimFiyat"].HeaderText = "Birim Fiyat";
                        dataGridView1.Columns["BirimFiyat"].DefaultCellStyle.Format = "C2";
                        dataGridView1.Columns["BirimFiyat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    else if (dataGridView1.Columns.Contains("SatisFiyati"))
                    {
                        dataGridView1.Columns["SatisFiyati"].HeaderText = "Birim Fiyat"; // Burayı da standartlaştırdık
                        dataGridView1.Columns["SatisFiyati"].DefaultCellStyle.Format = "C2";
                        dataGridView1.Columns["SatisFiyati"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    // Toplam Tutar
                    if (dataGridView1.Columns.Contains("ToplamTutar"))
                    {
                        dataGridView1.Columns["ToplamTutar"].HeaderText = "Toplam Tutar";
                        dataGridView1.Columns["ToplamTutar"].DefaultCellStyle.Format = "C2";
                        dataGridView1.Columns["ToplamTutar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        // Kalın yaparak toplam tutarı vurgulayabiliriz
                        dataGridView1.Columns["ToplamTutar"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }

                    // Gizlenecek Sütunlar
                    string[] gizlenecekler = { "Id", "UrunId", "MusteriId", "ProductId", "CustomerId", "Tarih", "SaleDate" };
                    foreach (var kolon in gizlenecekler)
                    {
                        if (dataGridView1.Columns.Contains(kolon)) dataGridView1.Columns[kolon].Visible = false;
                    }
                }

                dataGridView1.ClearSelection();
            }
            catch (Exception)
            {
                // Hata durumunda işlem yok
            }
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            // BÜTÜN İŞLEMLER TRY İÇİNDE OLMALI
            try
            {
                // 1. ÖNCE BOŞLUK KONTROLÜ (En başta yapılmalı)
                if (string.IsNullOrEmpty(txtAdet.Text) || string.IsNullOrEmpty(txtSatisFiyati.Text) || cmbUrun.SelectedValue == null)
                {
                    MessageBox.Show("Lütfen ürün seçiniz ve miktar giriniz.");
                    return;
                }

                // 2. VERİLERİ ÇEVİRME (Artık kutuların dolu olduğundan eminiz)
                int urunId = (int)cmbUrun.SelectedValue;
                decimal satilacakAdet = Convert.ToDecimal(txtAdet.Text);

                // 3. STOK KONTROLÜ (Veritabanına soruyoruz)
                Product secilenUrun = productManager.UrunGetir(urunId);

                if (secilenUrun.StokMiktari < satilacakAdet)
                {
                    // DÜZELTME BURADA YAPILDI:
                    // .ToString("0.##") sayesinde "500.000" yerine net olarak "500" yazar.
                    // Ama "1.5" ise "1,5" yazar.
                    MessageBox.Show($"Yetersiz Stok!\nDepoda sadece {secilenUrun.StokMiktari.ToString("0.##")} adet {secilenUrun.UrunAdi} var.",
                                    "Stok Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Stok yoksa işlemi burada kes, aşağıya inme!
                }

                // 4. SATIŞ İŞLEMİ (Her şey tamamsa burası çalışır)
                Sales satis = new Sales();
                satis.UrunId = urunId;
                satis.MusteriId = (int)cmbMusteri.SelectedValue;
                satis.Tarih = DateTime.Now;
                satis.Adet = satilacakAdet;
                satis.SatisFiyati = Convert.ToDecimal(txtSatisFiyati.Text);

                salesManager.SatisYap(satis);

                MessageBox.Show("Satış Başarılı!\nTutar: " + (satis.Adet * satis.SatisFiyati).ToString("C2"));

                // 5. EKRANI GÜNCELLE
                Listele();
                txtAdet.Text = "";

                // Stok düştüğü için ürün bilgilerini (kalan stoğu) ekranda güncelle
                FiyatiKutuyaGetir();
            }
            catch (Exception ex)
            {
                // Hangi aşamada hata olursa olsun burası yakalar ve program kapanmaz
                MessageBox.Show("Satış İşlemi Hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}