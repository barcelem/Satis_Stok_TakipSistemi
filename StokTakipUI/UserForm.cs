using System;
using System.Windows.Forms;
using BusinessLogicLayer;
using EntityLayer;
using System.Collections.Generic;

namespace StokTakipUI
{
    public partial class UserForm : Form
    {
        // Manager sınıfımızı çağırıyoruz
        UserManager userManager = new UserManager();

        public UserForm()
        {
            InitializeComponent();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            // 1. GÜVENLİK KONTROLÜ: Kullanıcı giriş yapmış mı?
            if (UserManager.AktifKullanici == null)
            {
                // Eğer programı direkt UserForm'dan başlatırsan bu hatayı almamak için:
                MessageBox.Show("Lütfen önce giriş yapınız! (Program.cs'den LoginForm'u başlangıç yapın)");
                this.Close();
                return;
            }

            // 2. YETKİ KONTROLÜ: Sadece Yöneticiler girebilir
            if (UserManager.AktifKullanici.Role != "Yonetici")
            {
                MessageBox.Show("Bu sayfaya erişim yetkiniz yok!", "Yetkisiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            // 3. COMBOBOX DOLDURMA (Eğer tasarımda boş bıraktıysan burası doldurur)
            if (cmbRol.Items.Count == 0)
            {
                cmbRol.Items.AddRange(new object[] { "Yonetici", "Satış Personeli", "Depo Personeli" });
            }

            // Listeyi getir
            Listele();
        }

        // --- LİSTELEME METODU ---
        void Listele()
        {
            dataGridView1.DataSource = userManager.TumKullanicilariGetir();

            // Tablo Görsel Ayarları
            if (dataGridView1.Columns["Password"] != null) dataGridView1.Columns["Password"].Visible = false; // Şifre gizli
            if (dataGridView1.Columns["Id"] != null) dataGridView1.Columns["Id"].Visible = false; // ID gizli

            if (dataGridView1.Columns["Username"] != null) dataGridView1.Columns["Username"].HeaderText = "Kullanıcı Adı";
            if (dataGridView1.Columns["Role"] != null) dataGridView1.Columns["Role"].HeaderText = "Yetki";

            // Tüm alana yay
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Satırın tamamını seç
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // --- EKLEME BUTONU ---
        private void btnEkle_Click(object sender, EventArgs e)
        {
            // Boş alan kontrolü
            if (string.IsNullOrEmpty(txtKullaniciAdi.Text) || string.IsNullOrEmpty(txtSifre.Text) || cmbRol.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Kullanıcı Adı, Şifre ve Yetki alanlarını doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                User yeniUser = new User();
                yeniUser.Username = txtKullaniciAdi.Text;
                yeniUser.Password = txtSifre.Text;
                yeniUser.Role = cmbRol.SelectedItem.ToString();

                userManager.KullaniciEkle(yeniUser);

                MessageBox.Show("Kullanıcı başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        // --- SİLME BUTONU ---
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int seciliId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                string seciliKadi = dataGridView1.SelectedRows[0].Cells["Username"].Value.ToString();

                // Kendini silmeyi engelle
                if (seciliKadi == UserManager.AktifKullanici.Username)
                {
                    MessageBox.Show("Şu an giriş yapmış olduğunuz hesabı silemezsiniz!", "İşlem Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show(seciliKadi + " kullanıcısını silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    userManager.KullaniciSil(seciliId);
                    Listele();
                    Temizle();
                    MessageBox.Show("Kullanıcı silindi.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen listeden silinecek kişiyi seçiniz.");
            }
        }

        // --- GÜNCELLEME BUTONU ---
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (string.IsNullOrEmpty(txtKullaniciAdi.Text) || string.IsNullOrEmpty(txtSifre.Text) || cmbRol.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bilgileri eksiksiz giriniz.");
                    return;
                }

                try
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                    User guncellenecekUser = new User();
                    guncellenecekUser.Id = id;
                    guncellenecekUser.Username = txtKullaniciAdi.Text;
                    guncellenecekUser.Password = txtSifre.Text;
                    guncellenecekUser.Role = cmbRol.SelectedItem.ToString();

                    // UserManager içindeki güncelleme metodunu çağırıyoruz
                    userManager.KullaniciGuncelle(guncellenecekUser);

                    MessageBox.Show("Kullanıcı güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                    Temizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek satırı seçiniz.");
            }
        }

        // --- TEMİZLEME METODU ---
        void Temizle()
        {
            txtKullaniciAdi.Text = "";
            txtSifre.Text = "";
            cmbRol.SelectedIndex = -1;
            dataGridView1.ClearSelection();
        }

        // --- TABLOYA TIKLAYINCA BİLGİLERİ KUTULARA DOLDUR ---
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtKullaniciAdi.Text = dataGridView1.Rows[e.RowIndex].Cells["Username"].Value.ToString();

                // Şifre hücresi boş değilse getir
                if (dataGridView1.Rows[e.RowIndex].Cells["Password"].Value != null)
                {
                    txtSifre.Text = dataGridView1.Rows[e.RowIndex].Cells["Password"].Value.ToString();
                }

                string rol = dataGridView1.Rows[e.RowIndex].Cells["Role"].Value.ToString();
                if (cmbRol.Items.Contains(rol))
                {
                    cmbRol.SelectedItem = rol;
                }
            }
        }
    }
}