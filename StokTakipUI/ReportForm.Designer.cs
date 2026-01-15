namespace StokTakipUI
{
    partial class ReportForm
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
            this.btnCokSatanlar = new System.Windows.Forms.Button();
            this.btnAylikSatis = new System.Windows.Forms.Button();
            this.btnKritikStok = new System.Windows.Forms.Button();
            this.dataGridViewRapor = new System.Windows.Forms.DataGridView();
            this.btnKarZarar = new System.Windows.Forms.Button();
            this.btnMusteriRapor = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRapor)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCokSatanlar
            // 
            this.btnCokSatanlar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCokSatanlar.Location = new System.Drawing.Point(12, 51);
            this.btnCokSatanlar.Name = "btnCokSatanlar";
            this.btnCokSatanlar.Size = new System.Drawing.Size(167, 45);
            this.btnCokSatanlar.TabIndex = 0;
            this.btnCokSatanlar.Text = "En Çok Satanlar";
            this.btnCokSatanlar.UseVisualStyleBackColor = true;
            this.btnCokSatanlar.Click += new System.EventHandler(this.btnCokSatanlar_Click);
            // 
            // btnAylikSatis
            // 
            this.btnAylikSatis.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAylikSatis.Location = new System.Drawing.Point(12, 136);
            this.btnAylikSatis.Name = "btnAylikSatis";
            this.btnAylikSatis.Size = new System.Drawing.Size(167, 47);
            this.btnAylikSatis.TabIndex = 1;
            this.btnAylikSatis.Text = "Aylık Ciro";
            this.btnAylikSatis.UseVisualStyleBackColor = true;
            this.btnAylikSatis.Click += new System.EventHandler(this.btnAylikSatis_Click);
            // 
            // btnKritikStok
            // 
            this.btnKritikStok.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKritikStok.Location = new System.Drawing.Point(12, 221);
            this.btnKritikStok.Name = "btnKritikStok";
            this.btnKritikStok.Size = new System.Drawing.Size(167, 47);
            this.btnKritikStok.TabIndex = 2;
            this.btnKritikStok.Text = "Kritik Stok Uyarıları";
            this.btnKritikStok.UseVisualStyleBackColor = true;
            this.btnKritikStok.Click += new System.EventHandler(this.btnKritikStok_Click);
            // 
            // dataGridViewRapor
            // 
            this.dataGridViewRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRapor.Location = new System.Drawing.Point(241, 31);
            this.dataGridViewRapor.Name = "dataGridViewRapor";
            this.dataGridViewRapor.RowHeadersWidth = 51;
            this.dataGridViewRapor.RowTemplate.Height = 24;
            this.dataGridViewRapor.Size = new System.Drawing.Size(669, 514);
            this.dataGridViewRapor.TabIndex = 3;
            // 
            // btnKarZarar
            // 
            this.btnKarZarar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKarZarar.Location = new System.Drawing.Point(12, 307);
            this.btnKarZarar.Name = "btnKarZarar";
            this.btnKarZarar.Size = new System.Drawing.Size(167, 47);
            this.btnKarZarar.TabIndex = 4;
            this.btnKarZarar.Text = "Kâr / Zarar Analizi";
            this.btnKarZarar.UseVisualStyleBackColor = true;
            this.btnKarZarar.Click += new System.EventHandler(this.btnKarZarar_Click);
            // 
            // btnMusteriRapor
            // 
            this.btnMusteriRapor.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnMusteriRapor.Location = new System.Drawing.Point(12, 395);
            this.btnMusteriRapor.Name = "btnMusteriRapor";
            this.btnMusteriRapor.Size = new System.Drawing.Size(167, 47);
            this.btnMusteriRapor.TabIndex = 5;
            this.btnMusteriRapor.Text = "Müşteri Raporu";
            this.btnMusteriRapor.UseVisualStyleBackColor = true;
            this.btnMusteriRapor.Click += new System.EventHandler(this.btnMusteriRapor_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnCokSatanlar);
            this.panel1.Controls.Add(this.btnMusteriRapor);
            this.panel1.Controls.Add(this.btnKarZarar);
            this.panel1.Controls.Add(this.btnKritikStok);
            this.panel1.Controls.Add(this.btnAylikSatis);
            this.panel1.Location = new System.Drawing.Point(35, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 514);
            this.panel1.TabIndex = 6;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1503, 806);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridViewRapor);
            this.Name = "ReportForm";
            this.Text = "Rapor Ekranı";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.ReportForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRapor)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCokSatanlar;
        private System.Windows.Forms.Button btnAylikSatis;
        private System.Windows.Forms.Button btnKritikStok;
        private System.Windows.Forms.DataGridView dataGridViewRapor;
        private System.Windows.Forms.Button btnKarZarar;
        private System.Windows.Forms.Button btnMusteriRapor;
        private System.Windows.Forms.Panel panel1;
    }
}