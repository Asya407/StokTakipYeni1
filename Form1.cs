using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-PS08KAK\SQLEXPRESS;Initial Catalog=StokDB;Integrated Security=True;";
        private string userRole;

        public Form1()
        {
            InitializeComponent();
            userRole = "User"; // default rol
        }

        public Form1(string role)
        {
            InitializeComponent();
            userRole = role;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TestSqlConnection();
            KategorileriYukle();
            SatisDurumuYukle();
            UrunleriListele();

            if (userRole == "User")
            {
                btnSil.Enabled = false;
                btnGuncelle.Enabled = false;
                btnEkle.Enabled = false;
                btnUrunVer.Enabled = false;
            }

            comboBoxSatisDurumu.SelectedIndexChanged += (s, ev) => UrunleriListele();
        }

        private void TestSqlConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string serverName = (string)new SqlCommand("SELECT @@SERVERNAME", con).ExecuteScalar();
                    string dbName = (string)new SqlCommand("SELECT DB_NAME()", con).ExecuteScalar();
                    string userName = (string)new SqlCommand("SELECT SUSER_NAME()", con).ExecuteScalar();
                    MessageBox.Show($"Server: {serverName}\nDB: {dbName}\nUser: {userName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void KategorileriYukle()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Id, CategoryName FROM dbo.Categories", con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    var kategoriList = new List<KeyValuePair<int, string>>();

                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string name = reader["CategoryName"].ToString();
                        kategoriList.Add(new KeyValuePair<int, string>(id, name));
                    }
                    reader.Close();

                    comboBoxKategori.DataSource = kategoriList;
                    comboBoxKategori.DisplayMember = "Value";
                    comboBoxKategori.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void SatisDurumuYukle()
        {
            comboBoxSatisDurumu.Items.Clear();
            comboBoxSatisDurumu.Items.Add("Tümü");
            comboBoxSatisDurumu.Items.Add("Az Satan");
            comboBoxSatisDurumu.Items.Add("Orta Satan");
            comboBoxSatisDurumu.Items.Add("Çok Satan");
            comboBoxSatisDurumu.SelectedIndex = 0;
        }

        private void UrunleriListele()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string durum = comboBoxSatisDurumu.SelectedItem?.ToString() ?? "Tümü";

                string query = @"
                    SELECT u.Id, u.UrunAdi, u.Fiyat, u.Miktar, c.CategoryName,
                           CASE 
                               WHEN u.Miktar < 10 THEN 'Az Satan'
                               WHEN u.Miktar BETWEEN 10 AND 50 THEN 'Orta Satan'
                               ELSE 'Çok Satan'
                           END AS SatisDurumu
                    FROM dbo.Urunler u
                    LEFT JOIN dbo.Categories c ON u.CategoryId = c.Id";

                if (durum != "Tümü")
                    query += $" WHERE CASE WHEN u.Miktar < 10 THEN 'Az Satan' WHEN u.Miktar BETWEEN 10 AND 50 THEN 'Orta Satan' ELSE 'Çok Satan' END = '{durum}'";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void Temizle()
        {
            txtUrunAdi.Text = "";
            txtFiyat.Text = "";
            txtMiktar.Text = "";
            txtVerilenKisi.Text = "";
            if (comboBoxKategori.Items.Count > 0)
                comboBoxKategori.SelectedIndex = 0;
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (comboBoxKategori.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir kategori seçiniz!");
                return;
            }

            int kategoriId = ((KeyValuePair<int, string>)comboBoxKategori.SelectedItem).Key;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO dbo.Urunler (UrunAdi, Fiyat, Miktar, CategoryId, Status) VALUES (@ad, @fiyat, @miktar, @kategoriId, @status)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                    cmd.Parameters.AddWithValue("@fiyat", decimal.Parse(txtFiyat.Text));
                    cmd.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));
                    cmd.Parameters.AddWithValue("@kategoriId", kategoriId);
                    cmd.Parameters.AddWithValue("@status", "Yeni");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Ürün eklendi!");
                    UrunleriListele();
                    Temizle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
            DialogResult result = MessageBox.Show("Bu ürünü silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM dbo.Urunler WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ürün silindi!");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Silme işlemi başarısız. Ürün hareket kayıtlarına bağlı olabilir.\nHata: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                        UrunleriListele();
                        Temizle();
                    }
                }
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || comboBoxKategori.SelectedItem == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
            int kategoriId = ((KeyValuePair<int, string>)comboBoxKategori.SelectedItem).Key;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE dbo.Urunler SET UrunAdi=@ad, Fiyat=@fiyat, Miktar=@miktar, CategoryId=@kategoriId WHERE Id=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                cmd.Parameters.AddWithValue("@fiyat", decimal.Parse(txtFiyat.Text));
                cmd.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));
                cmd.Parameters.AddWithValue("@kategoriId", kategoriId);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Ürün güncellendi!");
                UrunleriListele();
                Temizle();
            }
        }

        private void BtnUrunVer_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || string.IsNullOrWhiteSpace(txtVerilenKisi.Text))
            {
                MessageBox.Show("Lütfen bir ürün seçin ve verilen kişiyi yazın.");
                return;
            }

            int urunId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
            int miktar = int.Parse(txtMiktar.Text);
            string verilenKisi = txtVerilenKisi.Text;

            UrunVer(urunId, verilenKisi, miktar);
        }

        private void UrunVer(int urunId, string verilenKisi, int miktar)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string updateQuery = "UPDATE dbo.Urunler SET Miktar = Miktar - @miktar WHERE Id = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con, tran);
                    updateCmd.Parameters.AddWithValue("@miktar", miktar);
                    updateCmd.Parameters.AddWithValue("@id", urunId);
                    updateCmd.ExecuteNonQuery();

                    string insertQuery = @"INSERT INTO dbo.UrunHareket (UrunId, VerilenKisi, Miktar, Yorum) 
                                           VALUES (@urunId, @verilenKisi, @miktar, @yorum)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, con, tran);
                    insertCmd.Parameters.AddWithValue("@urunId", urunId);
                    insertCmd.Parameters.AddWithValue("@verilenKisi", verilenKisi);
                    insertCmd.Parameters.AddWithValue("@miktar", miktar);
                    insertCmd.Parameters.AddWithValue("@yorum", $"{miktar} adet ürün {verilenKisi} kişisine verildi.");
                    insertCmd.ExecuteNonQuery();

                    tran.Commit();
                    MessageBox.Show("Ürün verildi ve hareket kaydı oluşturuldu.");
                    UrunleriListele();
                    Temizle();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            txtUrunAdi.Text = row.Cells["UrunAdi"].Value.ToString();
            txtFiyat.Text = row.Cells["Fiyat"].Value.ToString();
            txtMiktar.Text = row.Cells["Miktar"].Value.ToString();

            string kategoriAdi = row.Cells["CategoryName"].Value?.ToString();
            for (int i = 0; i < comboBoxKategori.Items.Count; i++)
            {
                var item = (KeyValuePair<int, string>)comboBoxKategori.Items[i];
                if (item.Value == kategoriAdi)
                {
                    comboBoxKategori.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
