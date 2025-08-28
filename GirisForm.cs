using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class GirisForm : Form
    {
        public GirisForm()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-PS08KAK\\SQLEXPRESS;Initial Catalog=StokDB;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Role FROM Users WHERE Username=@kadi AND Password=@sifre";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kadi", txtKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@sifre", txtSifre.Text);

                con.Open();
                object roleObj = cmd.ExecuteScalar();
                con.Close();

                if (roleObj != null)
                {
                    string role = roleObj.ToString();
                    MessageBox.Show($"Giriş başarılı! Rol: {role}");

                    this.Hide();
                    Form1 anaForm = new Form1(role); // Role parametre olarak gönderiliyor
                    anaForm.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre yanlış.");
                }
            }
        }
    }
}
