namespace StokTakip
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBoxKategori;
        private System.Windows.Forms.ComboBox comboBoxSatisDurumu;
        private System.Windows.Forms.TextBox txtUrunAdi;
        private System.Windows.Forms.TextBox txtFiyat;
        private System.Windows.Forms.TextBox txtMiktar;
        private System.Windows.Forms.TextBox txtVerilenKisi;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnGuncelle;
        private System.Windows.Forms.Button btnUrunVer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBoxKategori = new System.Windows.Forms.ComboBox();
            this.comboBoxSatisDurumu = new System.Windows.Forms.ComboBox();
            this.txtUrunAdi = new System.Windows.Forms.TextBox();
            this.txtFiyat = new System.Windows.Forms.TextBox();
            this.txtMiktar = new System.Windows.Forms.TextBox();
            this.txtVerilenKisi = new System.Windows.Forms.TextBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnGuncelle = new System.Windows.Forms.Button();
            this.btnUrunVer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // dataGridView1
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Size = new System.Drawing.Size(600, 200);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);

            // comboBoxKategori
            this.comboBoxKategori.Location = new System.Drawing.Point(620, 30);
            this.comboBoxKategori.Size = new System.Drawing.Size(150, 21);

            // comboBoxSatisDurumu
            this.comboBoxSatisDurumu.Location = new System.Drawing.Point(620, 70);
            this.comboBoxSatisDurumu.Size = new System.Drawing.Size(150, 21);

            // txtUrunAdi
            this.txtUrunAdi.Location = new System.Drawing.Point(620, 110);
            this.txtUrunAdi.Size = new System.Drawing.Size(150, 20);

            // txtFiyat
            this.txtFiyat.Location = new System.Drawing.Point(620, 140);
            this.txtFiyat.Size = new System.Drawing.Size(150, 20);

            // txtMiktar
            this.txtMiktar.Location = new System.Drawing.Point(620, 170);
            this.txtMiktar.Size = new System.Drawing.Size(150, 20);

            // txtVerilenKisi
            this.txtVerilenKisi.Location = new System.Drawing.Point(620, 200);
            this.txtVerilenKisi.Size = new System.Drawing.Size(150, 20);

            // btnEkle
            this.btnEkle.Location = new System.Drawing.Point(620, 230);
            this.btnEkle.Size = new System.Drawing.Size(70, 30);
            this.btnEkle.Text = "Ekle";
            this.btnEkle.Click += new System.EventHandler(this.BtnEkle_Click);

            // btnSil
            this.btnSil.Location = new System.Drawing.Point(700, 230);
            this.btnSil.Size = new System.Drawing.Size(70, 30);
            this.btnSil.Text = "Sil";
            this.btnSil.Click += new System.EventHandler(this.BtnSil_Click);

            // btnGuncelle
            this.btnGuncelle.Location = new System.Drawing.Point(620, 270);
            this.btnGuncelle.Size = new System.Drawing.Size(70, 30);
            this.btnGuncelle.Text = "Güncelle";
            this.btnGuncelle.Click += new System.EventHandler(this.BtnGuncelle_Click);

            // btnUrunVer
            this.btnUrunVer.Location = new System.Drawing.Point(700, 270);
            this.btnUrunVer.Size = new System.Drawing.Size(70, 30);
            this.btnUrunVer.Text = "Ürün Ver";
            this.btnUrunVer.Click += new System.EventHandler(this.BtnUrunVer_Click);

            // Form1
            this.ClientSize = new System.Drawing.Size(800, 320);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxKategori);
            this.Controls.Add(this.comboBoxSatisDurumu);
            this.Controls.Add(this.txtUrunAdi);
            this.Controls.Add(this.txtFiyat);
            this.Controls.Add(this.txtMiktar);
            this.Controls.Add(this.txtVerilenKisi);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnGuncelle);
            this.Controls.Add(this.btnUrunVer);
            this.Text = "Stok Takip";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
