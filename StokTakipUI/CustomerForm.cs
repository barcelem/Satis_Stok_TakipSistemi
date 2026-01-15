using BusinessLogicLayer; // İş katmanı
using EntityLayer;        // Varlık katmanı
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StokTakipUI
{
    public partial class CustomerForm : Form
    {
        // Yönetici sınıfını çağırıyoruz
        CustomerManager customerManager = new CustomerManager();

        // Güncelleme işlemi için seçilen ID
        int secilenId = 0;

        public CustomerForm()
        {
            InitializeComponent();
        }

        // --- FORM AÇILINCA ÇALIŞACAK KISIM (Load Event) ---
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            // 1. Listeyi doldur
            Listele();

            // 2. Tablo Görsel Ayarları
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- LİSTELEME METODU ---
        void Listele()
        {
            // 1. VERİYİ ÇEK VE BAĞLA
            dataGridView1.DataSource = customerManager.TumMusterileriGetir();

            // --- ORTAK GÖRSEL STİL FONKSİYONU ---
            // Ürünler sayfasındaki tasarımı buraya aynen uyguluyoruz
            void GridStilUygula(DataGridView dgv)
            {
                // Başlıklar -> KALIN, SİYAH, GRİ ARKA PLAN
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold); // Kalın Font
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Satırlar -> NORMAL (Kalın Değil)
                dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular); // Normal Font
                dgv.DefaultCellStyle.ForeColor = Color.Black;

                // Genel Ayarlar
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ekrana yay
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                dgv.RowHeadersVisible = false; // Soldaki boşluğu gizle
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            // Stili Uygula
            GridStilUygula(dataGridView1);

            // --- SÜTUN ÖZEL AYARLARI ---
            if (dataGridView1.Columns.Count > 0)
            {
                // 1. Başlık İsimleri
                if (dataGridView1.Columns["AdSoyad"] != null)
                    dataGridView1.Columns["AdSoyad"].HeaderText = "Müşteri Ünvanı";

                if (dataGridView1.Columns["Telefon"] != null)
                    dataGridView1.Columns["Telefon"].HeaderText = "Telefon No";

                if (dataGridView1.Columns["MusteriTuru"] != null)
                    dataGridView1.Columns["MusteriTuru"].HeaderText = "Müşteri Türü";

                // 2. TARİH SÜTUNU AYARLARI
                if (dataGridView1.Columns["OlusturmaTarihi"] != null)
                {
                    dataGridView1.Columns["OlusturmaTarihi"].Visible = true;
                    dataGridView1.Columns["OlusturmaTarihi"].HeaderText = "Kayıt Tarihi";
                    dataGridView1.Columns["OlusturmaTarihi"].DefaultCellStyle.Format = "g"; // 10.01.2026 14:12 gibi
                    dataGridView1.Columns["OlusturmaTarihi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // 3. Gizlenecek Sütunlar
                if (dataGridView1.Columns["Id"] != null) dataGridView1.Columns["Id"].Visible = false;

                // Eğer BaseEntity'den gelen başka gizli alanlar varsa onları da buraya ekleyebilirsin
            }

            // Seçimi temizle
            dataGridView1.ClearSelection();
        }
        // --- EKLEME BUTONU ---
        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAdSoyad.Text) || string.IsNullOrEmpty(cmbTur.Text))
                {
                    MessageBox.Show("Lütfen Ad Soyad ve Müşteri Türü alanlarını doldurunuz.");
                    return;
                }

                Customer yeniMusteri = new Customer();
                yeniMusteri.AdSoyad = txtAdSoyad.Text;
                yeniMusteri.Telefon = txtTelefon.Text;
                yeniMusteri.MusteriTuru = cmbTur.Text;

                // Manager sınıfındaki ekleme metodu
                customerManager.MusteriEkle(yeniMusteri);

                MessageBox.Show("Müşteri başarıyla eklendi.");
                Listele(); // Listeyi yenile ki yeni kayıt görünsün
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // --- SİLME BUTONU ---
        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DialogResult cevap = MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (cevap == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                        // Manager sınıfındaki silme metodu
                        customerManager.MusteriSil(id);

                        MessageBox.Show("Müşteri silindi.");
                        Listele(); // Listeyi yenile
                        Temizle();
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silinecek satırı seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // --- GÜNCELLEME BUTONU ---
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilenId != 0)
                {
                    Customer guncellenecek = new Customer();
                    guncellenecek.Id = secilenId;
                    guncellenecek.AdSoyad = txtAdSoyad.Text;
                    guncellenecek.Telefon = txtTelefon.Text;
                    guncellenecek.MusteriTuru = cmbTur.Text;

                    // Manager sınıfındaki güncelleme metodu
                    customerManager.MusteriGuncelle(guncellenecek);

                    MessageBox.Show("Bilgiler güncellendi.");
                    Listele(); // Listeyi yenile
                    Temizle();
                }
                else
                {
                    MessageBox.Show("Lütfen listeden bir kayıt seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // --- TABLOYA TIKLAYINCA KUTULARA DOLDUR ---
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Başlığa tıklanırsa hata vermesin diye kontrol ediyoruz
            if (e.RowIndex >= 0 && dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Seçilen kaydın ID'sini alıyoruz (Güncelleme için lazım)
                secilenId = Convert.ToInt32(row.Cells["Id"].Value);

                // Kutuları dolduruyoruz
                txtAdSoyad.Text = row.Cells["AdSoyad"].Value.ToString();
                txtTelefon.Text = row.Cells["Telefon"].Value.ToString();
                cmbTur.Text = row.Cells["MusteriTuru"].Value.ToString();
            }
        }

        // --- TEMİZLEME BUTONU (İsteğe Bağlı) ---
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        void Temizle()
        {
            txtAdSoyad.Text = "";
            txtTelefon.Text = "";
            cmbTur.SelectedIndex = -1;
            secilenId = 0;

            // Seçimi kaldır ki kullanıcı yeni kayıt gireceğini anlasın
            dataGridView1.ClearSelection();
        }

        private void CustomerForm_Activated(object sender, EventArgs e)
        {
            Listele();
        }
    }
}