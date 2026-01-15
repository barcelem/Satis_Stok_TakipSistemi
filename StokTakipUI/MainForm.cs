using System;
using System.Windows.Forms;
using BusinessLogicLayer; // UserManager'a erişmek için
using EntityLayer;        // User sınıfı için

namespace StokTakipUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. ÖNCE MENÜLERİ GİZLE
            // (Designer'da isimlendirme hatası olsa bile bu kod hepsini bulup gizler)
            foreach (Control c in this.Controls)
            {
                if (c is MenuStrip)
                {
                    foreach (ToolStripItem item in ((MenuStrip)c).Items)
                    {
                        item.Visible = false;
                    }
                }
            }

            if (UserManager.AktifKullanici != null)
            {
                User aktifUser = UserManager.AktifKullanici;
                this.Text = "Stok Takip Sistemi - " + aktifUser.Username;
                string rol = aktifUser.Role.Trim();

                // 2. YÖNETİCİ KONTROLÜ
                if (rol.IndexOf("yon", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    rol.IndexOf("admin", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Yöneticiye her şeyi aç (İsimlere takılmadan Text üzerinden)
                    MenuyuAc("Ürün");
                    MenuyuAc("Müşteri");
                    MenuyuAc("Satış"); // Yönetici hala satış ekranını görebilir
                    MenuyuAc("Rapor");
                    MenuyuAc("Kullanıcı");
                }
                // 3. DEPO PERSONELİ KONTROLÜ
                else if (rol.IndexOf("dep", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    MenuyuAc("Ürün"); // Sadece ürün yönetimi
                }
                // 4. SATIŞ PERSONELİ KONTROLÜ (YENİ EKLENEN KISIM)
                else if (rol.IndexOf("sat", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    MenuyuAc("Satış");   // Satış ekranını görür
                    MenuyuAc("Müşteri"); // Müşteri ekleme/seçme ekranını görür
                }

                // Çıkış butonu herkese açık olsun
                MenuyuAc("Çıkış");
            }
            else
            {
                new LoginForm().Show();
                this.Close();
            }
        }

        // YARDIMCI METOT (Bunu Load metodunun dışına, class içine ekle)
        void MenuyuAc(string kelime)
        {
            foreach (Control c in this.Controls)
            {
                if (c is MenuStrip)
                {
                    foreach (ToolStripItem item in ((MenuStrip)c).Items)
                    {
                        if (item.Text.IndexOf(kelime, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            item.Visible = true;
                        }
                    }
                }
            }
        }

        // --- MENÜ TIKLAMA OLAYLARI ---

        private void menuUrunIslemleri_Click(object sender, EventArgs e)
        {
            FormAc(typeof(ProductForm));
        }

        private void menuMusteriIslemleri_Click(object sender, EventArgs e)
        {
            FormAc(typeof(CustomerForm));
        }

        private void menuSatisYap_Click(object sender, EventArgs e)
        {
            FormAc(typeof(SalesForm));
        }

        private void menuRaporlar_Click(object sender, EventArgs e)
        {
            FormAc(typeof(ReportForm));
        }

        private void menuKullaniciIslemleri_Click(object sender, EventArgs e)
        {
            FormAc(typeof(UserForm));
        }

        // PRATİK FORM AÇMA METODU
        private void FormAc(Type formType)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form.GetType() == formType)
                {
                    form.BringToFront();
                    return;
                }
            }

            Form yeniForm = (Form)Activator.CreateInstance(formType);
            yeniForm.MdiParent = this;
            yeniForm.Show();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}