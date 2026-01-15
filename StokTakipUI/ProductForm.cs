using BusinessLogicLayer;
using EntityLayer;
using System;
using System.Drawing;
using System.Windows.Forms;
using DataAccessLayer; // Repository için (Gerekirse)

namespace StokTakipUI
{
    public partial class ProductForm : Form
    {
        ProductManager productManager = new ProductManager();
        StockMovementRepository historyRepo = new StockMovementRepository();
        int secilenId = 0;

        public ProductForm()
        {
            InitializeComponent();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            // 1. Önce Listeleme yap (Sol taraf ve ComboBox dolar)
            Listele();

            // --- EKRAN DÜZENLEME AYARLARI (Sığma Sorunu İçin) ---
            // Listelerin sütunlarını formun genişliğine göre otomatik yayar
            if (dataGridView1 != null)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridViewGecmis != null)
                dataGridViewGecmis.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 2. Birim seçimi için varsayılan değer
            try { if (cmbBirim != null && cmbBirim.Items.Count > 0) cmbBirim.SelectedIndex = 0; } catch { }

            // 3. YETKİ KONTROLÜ
            if (CurrentSession.CurrentUser != null)
            {
                EntityLayer.User aktifKullanici = CurrentSession.CurrentUser;
                this.Text = "Stok Takip Sistemi - Hoşgeldin: " + aktifKullanici.Username;

                if (aktifKullanici.Role != "Yonetici")
                {
                    if (btnSil != null) { btnSil.Visible = false; btnSil.Enabled = false; }
                    if (btnGuncelle != null) btnGuncelle.Visible = false;
                }
            }

            // Olay bağlantısını garantiye al (Eğer tıklama hala çalışmıyorsa bu satır kalsın)
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        // --- ANA METOD: Sadece özel geçmişi yönetir ---
        void GecmisListesiniGetir(int urunId)
        {
            System.Data.DataTable dt;

            try
            {
                if (urunId > 0)
                {
                    // ID varsa SADECE O ÜRÜNÜ getir
                    dt = historyRepo.GecmisiGetir(urunId);

                    dataGridViewGecmis.DataSource = dt;

                    // Tablo Sütun Ayarları
                    if (dataGridViewGecmis.Columns.Count > 0)
                    {
                        if (dataGridViewGecmis.Columns.Contains("UrunAdi"))
                            dataGridViewGecmis.Columns["UrunAdi"].HeaderText = "Ürün Adı";

                        if (dataGridViewGecmis.Columns.Contains("Quantity"))
                        {
                            dataGridViewGecmis.Columns["Quantity"].HeaderText = "Miktar";
                            dataGridViewGecmis.Columns["Quantity"].DefaultCellStyle.Format = "#,##0.##";
                        }
                        else if (dataGridViewGecmis.Columns.Contains("Miktar"))
                            dataGridViewGecmis.Columns["Miktar"].HeaderText = "Miktar";

                        if (dataGridViewGecmis.Columns.Contains("UnitPrice"))
                        {
                            dataGridViewGecmis.Columns["UnitPrice"].HeaderText = "Fiyat";
                            dataGridViewGecmis.Columns["UnitPrice"].DefaultCellStyle.Format = "C2";
                        }
                        else if (dataGridViewGecmis.Columns.Contains("Fiyat"))
                        {
                            dataGridViewGecmis.Columns["Fiyat"].HeaderText = "Fiyat";
                            dataGridViewGecmis.Columns["Fiyat"].DefaultCellStyle.Format = "C2";
                        }

                        if (dataGridViewGecmis.Columns.Contains("MovementDate"))
                            dataGridViewGecmis.Columns["MovementDate"].HeaderText = "Tarih";
                        else if (dataGridViewGecmis.Columns.Contains("Tarih"))
                            dataGridViewGecmis.Columns["Tarih"].HeaderText = "Tarih";

                        if (dataGridViewGecmis.Columns.Contains("Description"))
                            dataGridViewGecmis.Columns["Description"].HeaderText = "Açıklama";
                        else if (dataGridViewGecmis.Columns.Contains("Aciklama"))
                            dataGridViewGecmis.Columns["Aciklama"].HeaderText = "Açıklama";

                        // Gizlenecekler
                        if (dataGridViewGecmis.Columns.Contains("Id")) dataGridViewGecmis.Columns["Id"].Visible = false;
                        if (dataGridViewGecmis.Columns.Contains("ProductId")) dataGridViewGecmis.Columns["ProductId"].Visible = false;
                        if (dataGridViewGecmis.Columns.Contains("UrunId")) dataGridViewGecmis.Columns["UrunId"].Visible = false;
                    }
                }
                else
                {
                    // ID yoksa veya 0 ise tabloyu temizle
                    dataGridViewGecmis.DataSource = null;
                }
            }
            catch { }
        }

        void Listele()
        {
            // 1. GRID GÜNCELLEME (MEVCUT KOD)
            var guncelListe = productManager.TumUrunleriGetir();
            dataGridView1.DataSource = guncelListe;

            // NOT: Eğer sağdaki tabloyu da veritabanından çekiyorsanız burada bağlayın.
            // Örn: dataGridViewGecmis.DataSource = ...

            // --- YENİ EKLENEN: ORTAK GÖRSEL STİL FONKSİYONU ---
            // Bu fonksiyon her iki tabloya da aynı profesyonel görünümü verir.
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
                dgv.RowHeadersVisible = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            // Stili her iki tabloya da uygula
            GridStilUygula(dataGridView1);
            GridStilUygula(dataGridViewGecmis);

            // --- MEVCUT TABLO 1 (SOL) SÜTUN AYARLARI ---
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                // Sütun Başlıkları Düzenleme (Mevcut kodlarınız)
                if (dataGridView1.Columns["UrunKodu"] != null)
                {
                    dataGridView1.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                    dataGridView1.Columns["UrunKodu"].DisplayIndex = 0;
                    dataGridView1.Columns["UrunKodu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }

                if (dataGridView1.Columns["UrunAdi"] != null)
                {
                    dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                    dataGridView1.Columns["UrunAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dataGridView1.Columns["StokMiktari"] != null) dataGridView1.Columns["StokMiktari"].HeaderText = "Stok";
                if (dataGridView1.Columns["Birim"] != null) dataGridView1.Columns["Birim"].HeaderText = "Birim";
                if (dataGridView1.Columns["AlisFiyati"] != null) dataGridView1.Columns["AlisFiyati"].HeaderText = "Alış (₺)";
                if (dataGridView1.Columns["SatisFiyati"] != null) dataGridView1.Columns["SatisFiyati"].HeaderText = "Satış (₺)";
                if (dataGridView1.Columns["KritikStokSeviyesi"] != null) dataGridView1.Columns["KritikStokSeviyesi"].HeaderText = "Kritik";

                // Formatlama ve Hizalama (Mevcut kodlarınız)
                if (dataGridView1.Columns["AlisFiyati"] != null) dataGridView1.Columns["AlisFiyati"].DefaultCellStyle.Format = "C2";
                if (dataGridView1.Columns["SatisFiyati"] != null) dataGridView1.Columns["SatisFiyati"].DefaultCellStyle.Format = "C2";
                if (dataGridView1.Columns["StokMiktari"] != null) dataGridView1.Columns["StokMiktari"].DefaultCellStyle.Format = "N2";

                foreach (string col in new[] { "StokMiktari", "AlisFiyati", "SatisFiyati", "KritikStokSeviyesi" })
                {
                    if (dataGridView1.Columns[col] != null)
                        dataGridView1.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Gizlenecek Sütunlar (Tarih dahil)
                string[] gizlenecekler = { "Id", "Barkod", "OlusturmaTarihi" };
                foreach (string colName in gizlenecekler)
                {
                    if (dataGridView1.Columns[colName] != null) dataGridView1.Columns[colName].Visible = false;
                }
            }

            // --- YENİ EKLENEN: TABLO 2 (SAĞ/GEÇMİŞ) ÖZEL AYARLARI ---
            if (dataGridViewGecmis.Columns.Count > 0)
            {
                // Fiyat sütununun adını "Birim Fiyat" yap
                if (dataGridViewGecmis.Columns["Fiyat"] != null)
                {
                    dataGridViewGecmis.Columns["Fiyat"].HeaderText = "Birim Fiyat";
                    dataGridViewGecmis.Columns["Fiyat"].DefaultCellStyle.Format = "C2";
                }
                else if (dataGridViewGecmis.Columns["SatisFiyati"] != null)
                {
                    dataGridViewGecmis.Columns["SatisFiyati"].HeaderText = "Birim Fiyat";
                    dataGridViewGecmis.Columns["SatisFiyati"].DefaultCellStyle.Format = "C2";
                }
            }

            // 2. COMBOBOX GÜNCELLEME (MEVCUT KOD)
            cmbHizliUrun.DataSource = null;
            cmbHizliUrun.DataSource = productManager.TumUrunleriGetir();
            cmbHizliUrun.DisplayMember = "UrunAdi";
            cmbHizliUrun.ValueMember = "Id";

            // 3. SEÇİMLERİ SIFIRLA
            cmbHizliUrun.SelectedIndex = -1;
            cmbHizliUrun.Text = "";

            dataGridView1.ClearSelection();
            dataGridViewGecmis.ClearSelection();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. KONTROLLER
                if (string.IsNullOrEmpty(txtUrunKodu.Text) || string.IsNullOrEmpty(txtUrunAdi.Text) ||
                    string.IsNullOrEmpty(txtStok.Text) || string.IsNullOrEmpty(txtAlis.Text) || string.IsNullOrEmpty(txtSatis.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurunuz.");
                    return;
                }

                // 2. NESNE OLUŞTURMA
                Product yeniUrun = new Product();
                yeniUrun.UrunKodu = txtUrunKodu.Text.ToUpper();
                yeniUrun.UrunAdi = txtUrunAdi.Text;

                decimal stok; int kritik; decimal alis, satis;
                bool stokGecerli = decimal.TryParse(txtStok.Text, out stok);
                bool kritikGecerli = int.TryParse(txtKritik.Text, out kritik);
                bool alisGecerli = decimal.TryParse(txtAlis.Text, out alis);
                bool satisGecerli = decimal.TryParse(txtSatis.Text, out satis);

                if (!stokGecerli || !kritikGecerli || !alisGecerli || !satisGecerli)
                {
                    MessageBox.Show("Sayısal alanları kontrol ediniz.");
                    return;
                }

                yeniUrun.StokMiktari = stok;
                yeniUrun.KritikStokSeviyesi = kritik;
                yeniUrun.AlisFiyati = alis;
                yeniUrun.SatisFiyati = satis;
                yeniUrun.Birim = (cmbBirim != null) ? cmbBirim.Text : "Adet";

                // 3. KAYDETME İŞLEMİ
                productManager.UrunEkle(yeniUrun);

                // 4. LİSTEYİ YENİLE (Soldaki tablo güncellenir)
                Listele();

                // 5. GEÇMİŞ KAYDI OLUŞTURMA
                int yeniUrunId = productManager.UrunIdGetir(yeniUrun.UrunKodu);

                if (yeniUrunId > 0)
                {
                    EntityLayer.StockMovement ilkHareket = new EntityLayer.StockMovement();
                    ilkHareket.ProductId = yeniUrunId;
                    ilkHareket.Quantity = stok;
                    ilkHareket.UnitPrice = alis;
                    ilkHareket.Description = "İlk Stok Girişi";

                    historyRepo.HareketEkle(ilkHareket);

                    // İstediğin üzere: Ekleyince sağ taraf dolmasın, temiz kalsın.
                    // Sadece tıklayınca gelmesi için burayı boş bırakıyoruz.
                    dataGridViewGecmis.DataSource = null;
                }

                MessageBox.Show("Ürün başarıyla eklendi!");

                // 6. TEMİZLİK
                // TemizlikYap(true) diyerek hem formu hem sağdaki geçmiş tablosunu sıfırlıyoruz.
                TemizlikYap(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (secilenId == 0) return;

            try
            {
                Product guncellenecek = new Product();
                guncellenecek.Id = secilenId;
                guncellenecek.UrunKodu = txtUrunKodu.Text.ToUpper();
                guncellenecek.UrunAdi = txtUrunAdi.Text;
                guncellenecek.StokMiktari = decimal.Parse(txtStok.Text);
                guncellenecek.AlisFiyati = decimal.Parse(txtAlis.Text);
                guncellenecek.SatisFiyati = decimal.Parse(txtSatis.Text);
                guncellenecek.KritikStokSeviyesi = int.Parse(txtKritik.Text);

                productManager.UrunGuncelle(guncellenecek);
                MessageBox.Show("Ürün güncellendi!");
                Listele();
                btnTemizle_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnStokEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbHizliUrun.SelectedValue == null) { MessageBox.Show("Ürün seçiniz."); return; }
                if (string.IsNullOrEmpty(txtEkStok.Text)) { MessageBox.Show("Miktar giriniz."); return; }

                int id = Convert.ToInt32(cmbHizliUrun.SelectedValue);
                decimal eklenecekMiktar = Convert.ToDecimal(txtEkStok.Text);
                decimal yeniFiyat = string.IsNullOrEmpty(txtEkAlisFiyat.Text) ? 0 : Convert.ToDecimal(txtEkAlisFiyat.Text);

                productManager.StokEkle(id, eklenecekMiktar, yeniFiyat);

                // GEÇMİŞE KAYDET
                EntityLayer.StockMovement hareket = new EntityLayer.StockMovement();
                hareket.ProductId = id;
                hareket.Quantity = eklenecekMiktar;
                hareket.UnitPrice = yeniFiyat;
                hareket.Description = "Stok Ekleme";
                historyRepo.HareketEkle(hareket);

                MessageBox.Show("Stok Eklendi!");
                Listele();

                // Hızlı stok ekledikten sonra sağ tarafı o ürünün geçmişine odakla
                GecmisListesiniGetir(id);

                // Sadece kutuları temizle
                txtEkStok.Text = "";
                txtEkAlisFiyat.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        int id = int.Parse(dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString());
                        productManager.UrunSil(id);

                        Listele();
                        btnTemizle_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            if (txtArama.Text == "") Listele();
            else dataGridView1.DataSource = productManager.UrunAra(txtArama.Text);
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            TemizlikYap(true); // True: Sağ tarafı da temizle
        }

        // --- YARDIMCI METOD: Temizlik İşlemleri ---
        void TemizlikYap(bool sagTarafiTemizle)
        {
            // Sol Tarafı Temizle
            txtUrunKodu.Text = "";
            txtUrunAdi.Text = ""; txtStok.Text = ""; txtAlis.Text = "";
            txtSatis.Text = ""; txtKritik.Text = "";
            secilenId = 0;

            // Hızlı Stok Kısmını Temizle
            cmbHizliUrun.SelectedIndex = -1;
            cmbHizliUrun.Text = "";
            txtEkStok.Text = "";
            txtEkAlisFiyat.Text = "";

            // Seçimleri Kaldır
            dataGridView1.ClearSelection();
            txtUrunKodu.Focus();

            if (sagTarafiTemizle)
            {
                // Sağ tarafı temizle (Boşalt)
                dataGridViewGecmis.DataSource = null;
            }
        }

        private void ProductForm_MouseClick(object sender, MouseEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            
            // 1. GÜVENLİK KONTROLÜ: Başlıklara veya geçersiz satıra tıklanırsa işlem yapma
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count) return;

            try
            {
                // Debug İçin Mesaj (Çalışıyorsa bu kutu çıkmalı!)
                // MessageBox.Show("Tıklama Algılandı! Satır: " + e.RowIndex); 

                // 2. ID ALMA (Null Kontrollü - En Güvenli Yöntem)
                var idCell = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                secilenId = (idCell != null) ? Convert.ToInt32(idCell) : 0;

                if (secilenId == 0) return; // ID yoksa devam etme

                // 3. KUTULARI DOLDUR (?? Operatörü ile boş gelirse patlamasın, boş yazsın)
                txtUrunKodu.Text = dataGridView1.Rows[e.RowIndex].Cells["UrunKodu"].Value?.ToString() ?? "";
                txtUrunAdi.Text = dataGridView1.Rows[e.RowIndex].Cells["UrunAdi"].Value?.ToString() ?? "";
                txtStok.Text = dataGridView1.Rows[e.RowIndex].Cells["StokMiktari"].Value?.ToString() ?? "0";
                txtAlis.Text = dataGridView1.Rows[e.RowIndex].Cells["AlisFiyati"].Value?.ToString() ?? "0";
                txtSatis.Text = dataGridView1.Rows[e.RowIndex].Cells["SatisFiyati"].Value?.ToString() ?? "0";
                txtKritik.Text = dataGridView1.Rows[e.RowIndex].Cells["KritikStokSeviyesi"].Value?.ToString() ?? "0";

                if (cmbBirim != null && dataGridView1.Rows[e.RowIndex].Cells["Birim"].Value != null)
                    cmbBirim.Text = dataGridView1.Rows[e.RowIndex].Cells["Birim"].Value.ToString();

                // 4. HIZLI STOK KISMINI AYARLA
                cmbHizliUrun.SelectedValue = secilenId;
                txtEkAlisFiyat.Text = txtAlis.Text;

                // 5. SAĞ TARAFI GÜNCELLE (Asıl İstediğin Yer)
                GecmisListesiniGetir(secilenId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hücre Tıklama Hatası: " + ex.Message);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["StokMiktari"].Value != null && row.Cells["KritikStokSeviyesi"].Value != null)
                {
                    decimal stok; int kritik;
                    bool stokOk = decimal.TryParse(row.Cells["StokMiktari"].Value.ToString(), out stok);
                    bool kritikOk = int.TryParse(row.Cells["KritikStokSeviyesi"].Value.ToString(), out kritik);

                    if (stokOk && kritikOk && stok <= kritik)
                    {
                        row.Cells["StokMiktari"].Style.BackColor = System.Drawing.Color.Red;
                        row.Cells["StokMiktari"].Style.ForeColor = System.Drawing.Color.White;
                        row.Cells["StokMiktari"].Style.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells["StokMiktari"].Style.BackColor = System.Drawing.Color.White;
                        row.Cells["StokMiktari"].Style.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            dataGridView1.ClearSelection();
        }

        private void ProductForm_Activated(object sender, EventArgs e)
        {
            Listele();
        }
    }
}