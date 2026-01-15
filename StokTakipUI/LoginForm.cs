using System;
using System.Windows.Forms;
using BusinessLogicLayer; // Manager katmanını ekledik
using EntityLayer;        // User sınıfı için
using System.Collections.Generic;

namespace StokTakipUI
{
    public partial class LoginForm : Form
    {
        // DÜZELTME: Direkt Repository yerine Manager sınıfını kullanıyoruz
        UserManager userManager = new UserManager();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            

            // 1. KULLANICILARI ÇEK VE COMBOBOX'A DOLDUR
            try
            {
                // Manager üzerinden listeyi istiyoruz
                List<User> kullanicilar = userManager.TumKullanicilariGetir();
                cmbUsers.DataSource = kullanicilar;
                // User.cs içinde ToString() metodunu ezdiğimiz için DisplayMember gerekmez.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcı listesi yüklenemedi: " + ex.Message);
            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            // 2. KULLANICI SEÇİM KONTROLÜ
            if (cmbUsers.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir kullanıcı seçiniz.");
                return;
            }

            // 3. ŞİFRE KONTROLÜ
            // ComboBox'tan seçilen nesneyi 'User' tipine dönüştürüyoruz
            User secilenKullanici = (User)cmbUsers.SelectedItem;

            // DÜZELTME: Entity'de isimleri İngilizce yapmıştık (Password)
            if (txtSifre.Text == secilenKullanici.Password)
            {
                // --- GİRİŞ BAŞARILI ---

                // DÜZELTME: UserManager içindeki Static değişkene atıyoruz (Oturum Açma)
                UserManager.AktifKullanici = secilenKullanici;

                MessageBox.Show("Hoşgeldin: " + secilenKullanici.Username +
                                "\nYetki: " + secilenKullanici.Role, "Giriş Başarılı");

                // Ana Formu Aç
                MainForm anaForm = new MainForm();
                anaForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- GÖRSEL AYARLAR ---
        private void LoginForm_Resize(object sender, EventArgs e)
        {
            Ortala();
        }

        void Ortala()
        {
            //if (groupBox1 != null)
            //{
              //  groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
              //  groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2;
           // }
        }
    }
}