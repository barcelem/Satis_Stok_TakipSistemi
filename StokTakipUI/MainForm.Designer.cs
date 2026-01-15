namespace StokTakipUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuUrunIslemleri = new System.Windows.Forms.MenuStrip();
            this.ürünİşlemleriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMusteriIslemleri = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSatisYap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRaporlar = new System.Windows.Forms.ToolStripMenuItem();
            this.çıkışToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKullaniciIslemleri = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUrunIslemleri.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuUrunIslemleri
            // 
            this.menuUrunIslemleri.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuUrunIslemleri.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ürünİşlemleriToolStripMenuItem,
            this.menuMusteriIslemleri,
            this.menuSatisYap,
            this.menuRaporlar,
            this.çıkışToolStripMenuItem,
            this.menuKullaniciIslemleri});
            this.menuUrunIslemleri.Location = new System.Drawing.Point(0, 0);
            this.menuUrunIslemleri.Name = "menuUrunIslemleri";
            this.menuUrunIslemleri.Size = new System.Drawing.Size(800, 28);
            this.menuUrunIslemleri.TabIndex = 1;
            this.menuUrunIslemleri.Text = "menuStrip1";
            // 
            // ürünİşlemleriToolStripMenuItem
            // 
            this.ürünİşlemleriToolStripMenuItem.Name = "ürünİşlemleriToolStripMenuItem";
            this.ürünİşlemleriToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.ürünİşlemleriToolStripMenuItem.Text = "Ürün İşlemleri";
            this.ürünİşlemleriToolStripMenuItem.Click += new System.EventHandler(this.menuUrunIslemleri_Click);
            // 
            // menuMusteriIslemleri
            // 
            this.menuMusteriIslemleri.Name = "menuMusteriIslemleri";
            this.menuMusteriIslemleri.Size = new System.Drawing.Size(132, 24);
            this.menuMusteriIslemleri.Text = "Müşteri İşlemleri";
            this.menuMusteriIslemleri.Click += new System.EventHandler(this.menuMusteriIslemleri_Click);
            // 
            // menuSatisYap
            // 
            this.menuSatisYap.Name = "menuSatisYap";
            this.menuSatisYap.Size = new System.Drawing.Size(82, 24);
            this.menuSatisYap.Text = "Satış Yap";
            this.menuSatisYap.Click += new System.EventHandler(this.menuSatisYap_Click);
            // 
            // menuRaporlar
            // 
            this.menuRaporlar.Name = "menuRaporlar";
            this.menuRaporlar.Size = new System.Drawing.Size(80, 24);
            this.menuRaporlar.Text = "Raporlar";
            this.menuRaporlar.Click += new System.EventHandler(this.menuRaporlar_Click);
            // 
            // çıkışToolStripMenuItem
            // 
            this.çıkışToolStripMenuItem.Name = "çıkışToolStripMenuItem";
            this.çıkışToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.çıkışToolStripMenuItem.Text = "Çıkış";
            this.çıkışToolStripMenuItem.Click += new System.EventHandler(this.çıkışToolStripMenuItem_Click);
            // 
            // menuKullaniciIslemleri
            // 
            this.menuKullaniciIslemleri.Name = "menuKullaniciIslemleri";
            this.menuKullaniciIslemleri.Size = new System.Drawing.Size(139, 24);
            this.menuKullaniciIslemleri.Text = "Kullanıcı İşlemleri";
            this.menuKullaniciIslemleri.Click += new System.EventHandler(this.menuKullaniciIslemleri_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuUrunIslemleri);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuUrunIslemleri;
            this.Name = "MainForm";
            this.Text = "Stok ve Satış Takip Sistemi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuUrunIslemleri.ResumeLayout(false);
            this.menuUrunIslemleri.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuUrunIslemleri;
        private System.Windows.Forms.ToolStripMenuItem ürünİşlemleriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMusteriIslemleri;
        private System.Windows.Forms.ToolStripMenuItem menuSatisYap;
        private System.Windows.Forms.ToolStripMenuItem menuRaporlar;
        private System.Windows.Forms.ToolStripMenuItem çıkışToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuKullaniciIslemleri;
    }
}