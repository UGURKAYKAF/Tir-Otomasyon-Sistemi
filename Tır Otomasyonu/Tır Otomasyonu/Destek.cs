using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections;

namespace Tır_Otomasyonu
{
    public partial class Destek : Form
    {
        public Destek()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=.; Initial Catalog=help;Integrated Security=True");
        string sorgu;
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                baglanti.Open();
            if (richTextBox1.Text==""|| richTextBox2.Text == ""|| maskedTextBox1.Text == ""|| richTextBox4.Text == ""||richTextBox5.Text=="")
            {
                MessageBox.Show("Bilgiler Boş Olamaz", "Tır Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                sorgu = "insert into hlp(ad,soyad,telefon,posta,sorun)VALUES(@ad,@soyad,@telefon,@eposta,@sorun)";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);

                komut.Parameters.AddWithValue("@ad",richTextBox1.Text.ToUpper());
                komut.Parameters.AddWithValue("@soyad", richTextBox2.Text.ToUpper());
                komut.Parameters.AddWithValue("@telefon", maskedTextBox1.Text.ToUpper());
                komut.Parameters.AddWithValue("@eposta", richTextBox4.Text.ToUpper());
                komut.Parameters.AddWithValue("@sorun", richTextBox5.Text.ToUpper());

                    
               MessageBox.Show("İşlem Başarılı","Tır Otomasyonu",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    komut.ExecuteNonQuery();
                    baglanti.Close();
            }
            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.Message,"Tır Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    maskedTextBox1.Text = "";
                    richTextBox4.Text = "";
                    richTextBox5.Text = "";

           
        }

        private void Destek_Load(object sender, EventArgs e)
        {

        }
    }
}
