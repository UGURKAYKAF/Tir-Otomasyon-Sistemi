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
using Word = Microsoft.Office.Interop.Word;
using System.Data.OleDb;
using Microsoft.Office.Interop;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;


namespace Tır_Otomasyonu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy-MM-dd";
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "yyyy-MM-dd";
            dateTimePicker5.Format = DateTimePickerFormat.Custom;
            dateTimePicker5.CustomFormat = "yyyy-MM-dd";


        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=.; Initial Catalog=Otomasyon;Integrated Security=True");
        string sorgu;

        private void button1_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                double kdv = Convert.ToDouble(richTextBox11.Text);
                double fiyat = Convert.ToDouble(richTextBox7.Text);
                double yatmabedeli = Convert.ToDouble(richTextBox9.Text);
                double toplam = fiyat * kdv / 100;
                double kdvfiyat = fiyat + toplam;
                
                

                sorgu = "insert into otms(Tarih,Isi_Veren_Firma,Yuk_Bosaltigi_Yer,Konteyner_Aldigi_Yer,Konteyner_Biraktigi_Yer,Konteyner_Bosaltigi_Yer,Fiyat,Yatma_Bedeli,Kdv_Oranı,Kdv_Fiyat,Konteyner_no,Odeme_Durumu)VALUES(@tarih,@Isi_Veren_Firma,@Yuk_Bosaltigi_Yer,@Konteyner_Aldigi_Yer,@Konteyner_Biraktigi_Yer,@Konteyner_Bosaltigi_Yer,@Fiyat,@Yatma_Bedeli,@Kdv_Oranı,@Kdv_Fiyat,@Konteyner_No,@Odeme_Durumu)";

                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tarih",dateTimePicker1.Text);
                komut.Parameters.AddWithValue("@Isi_Veren_Firma", richTextBox1.Text.ToUpper());
                komut.Parameters.AddWithValue("@Konteyner_Bosaltigi_Yer", richTextBox2.Text.ToUpper());
                komut.Parameters.AddWithValue("@Yuk_Bosaltigi_Yer", richTextBox3.Text.ToUpper());
                komut.Parameters.AddWithValue("@Konteyner_Aldigi_Yer", richTextBox4.Text.ToUpper());
                komut.Parameters.AddWithValue("@Konteyner_Biraktigi_Yer", richTextBox6.Text.ToUpper());
                komut.Parameters.AddWithValue("@Fiyat", fiyat);
                komut.Parameters.AddWithValue("@Yatma_Bedeli", yatmabedeli);
                komut.Parameters.AddWithValue("@Kdv_Fiyat", kdvfiyat);
                komut.Parameters.AddWithValue("@Konteyner_No", richTextBox8.Text.ToUpper());
                komut.Parameters.AddWithValue("@Kdv_Oranı", kdv.ToString());
                if (radioButton1.Checked)
                {
                    komut.Parameters.AddWithValue("@Odeme_Durumu", radioButton1.Text);
                }
                else
                {
                    komut.Parameters.AddWithValue("@Odeme_Durumu", radioButton2.Text);
                }
                komut.ExecuteNonQuery();
                baglanti.Close();

                richTextBox1.Text = "";
                richTextBox2.Text = "";
                richTextBox3.Text = "";
                richTextBox4.Text = "";
                richTextBox6.Text = "";
                richTextBox7.Text = "";
                richTextBox8.Text = "";
                richTextBox9.Text = "";
                richTextBox11.Text = "";
                label23.Text = dataGridView1.Rows.Count.ToString();
                
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            verilerigoster();
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            
            button5.Enabled = false;
       
            string formattedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            string formattedDate1 = dateTimePicker2.Value.ToString("yyyyMMdd");
            string formattedDate2 = dateTimePicker3.Value.ToString("yyyyMMdd");
            string formattedDate3 = dateTimePicker4.Value.ToString("yyyyMMdd");
            string formattedDate4 = dateTimePicker5.Value.ToString("yyyyMMdd");

           // destekTalepleriniGörToolStripMenuItem.Enabled = false;
            //destekToolStripMenuItem.Enabled = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                    // SqlDataAdapter dAdapter = new SqlDataAdapter("Select * From otms", baglanti);
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            double kdvtoplam = 0;

            double ykdv = 0;
            double ybdl = 0;
            double kdv = 0;
            double toplam = 0;
            double tytb = 0;

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms", baglanti);
            SqlDataReader oku = sorgu.ExecuteReader();


            while (oku.Read())
            {
                kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
                ykdv = ybdl * kdv / 100;
                tytb += ykdv + ybdl;

                for (int i = 0; i < 1; ++i)
                {
                    kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                }
                toplam = kdvtoplam + tytb;
            }
            label30.Text = toplam.ToString();
            oku.Close();
            baglanti.Close();
            int colums = dataGridView1.RowCount - 1;
            label23.Text = colums.ToString();
            button5.Enabled = false;

        }
        //private void kdvtopla()
        //{
        //    double kdvtoplam = 0;

        //    double ykdv = 0;
        //    double ybdl = 0;
        //    double kdv = 0;
        //    double tplm = 0;
        //    double tytb = 0;

        //    if (baglanti.State == ConnectionState.Closed)
        //        baglanti.Open();
        //    SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms Where Odeme_Durumu Like '%Ödendi%'", baglanti);
        //    SqlDataReader oku = sorgu.ExecuteReader();


        //    while (oku.Read())
        //    {
        //        kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
        //        ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
        //        ykdv = ybdl * kdv / 100;
        //        tytb += ykdv + ybdl;

        //        for (int i = 0; i < 1; ++i)
        //        {
        //            kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
        //        }
        //        tplm = kdvtoplam + tytb;
        //    }

        //    oku.Close();
        //    baglanti.Close();

        //}
        private void button5_Click(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToLongDateString();
            label10.Text = DateTime.Now.ToLongTimeString();
        }

        private void richTextBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                    // SqlDataAdapter dAdapter = new SqlDataAdapter("Select * From otms", baglanti);
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms",baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            double kdvtoplam = 0;

            double ykdv = 0;
            double ybdl = 0;
            double kdv = 0;
            double toplam = 0;
            double tytb = 0;

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms", baglanti);
            SqlDataReader oku = sorgu.ExecuteReader();


            while (oku.Read())
            {
                kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
                ykdv = ybdl * kdv / 100;
                tytb += ykdv + ybdl;

                for (int i = 0; i < 1; ++i)
                {
                    kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                }
                toplam = kdvtoplam + tytb;
            }
            label30.Text = toplam.ToString();
            oku.Close();
            baglanti.Close();
            int colums = dataGridView1.RowCount-1;
            label23.Text = colums.ToString();
            button5.Enabled = false;
        }
        private void verilerigoster()
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                    //SqlDataAdapter dAdapter = new SqlDataAdapter("Select * From otms ", baglanti);
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
         
            try
            {
                if (comboBox1.Text=="Tarihe Göre")
                {
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                        SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Tarih Like '%" + dateTimePicker2.Text + "%'", baglanti);
                        DataTable dtable = new DataTable();
                        dAdapter.Fill(dtable);
                        dataGridView1.DataSource = dtable;
                        baglanti.Close();

                    }
                }
                else if(comboBox1.Text=="Firmaya Göre")
                {
                    string firma = richTextBox12.Text;
                    baglanti.Open();
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Isi_Veren_Firma Like '%" + firma + "%'", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();
                }
                else if (comboBox1.Text== "Konteyner No'ya Göre")
                {
                    string firma = richTextBox14.Text;
                    baglanti.Open();
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Konteyner_No Like '%" + firma + "%'", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();
                }
                else if (comboBox1.Text=="")
                {
                    MessageBox.Show("Filtreleme Türü Seçilmedi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                int colums = dataGridView1.RowCount - 1;
                label23.Text = colums.ToString();

                
                button5.Enabled = true;
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            comboBox1.Text = "";
            richTextBox12.Text = "";


            double kdvtoplam = 0;

            double ykdv = 0;
            double ybdl = 0;
            double kdv = 0;
            double toplam = 0;
            double tytb = 0;

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms where Tarih = '"+dateTimePicker2.Text+"'", baglanti);
            SqlDataReader oku = sorgu.ExecuteReader();


            while (oku.Read())
            {
                kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
                ykdv = ybdl * kdv / 100;
                tytb += ykdv + ybdl;

                for (int i = 0; i < 1; ++i)
                {
                    kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                }
                toplam = kdvtoplam + tytb;
            }
            label30.Text = toplam.ToString();
            oku.Close();
            baglanti.Close();

        }
    
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string firma = richTextBox10.Text;
                string verdigifirma = richTextBox5.Text;


            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            

                double kdv = Convert.ToDouble(richTextBox11.Text);
            double fiyat = Convert.ToDouble(richTextBox7.Text);
            double yatmabedeli = Convert.ToDouble(richTextBox9.Text);
            double kdvfiyat = fiyat * kdv/100;
                double toplam = fiyat + kdvfiyat;

                String verdigiyer = richTextBox13.Text;

                //if (firma==""||verdigifirma=="")
                //{
                //    MessageBox.Show("Firma İsmi Girmek Zorunludur", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //}
                //else if (verdigiyer=="")
                //{
                //    MessageBox.Show("İş Verilen Yer Boş Olamaz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //}
                //else
               // {
            sorgu = "update otms set Tarih=@tarih,Isi_Veren_Firma=@Isi_Veren_Firma,Konteyner_Bosaltigi_Yer=@Yukledigi_Firma,Yuk_Bosaltigi_Yer=@Yuk_Bosaltigi_Yer,Konteyner_Aldigi_Yer=@Konteyner_Aldigi_Yer,Konteyner_Biraktigi_Yer=@Konteyner_Biraktigi_Yer,Fiyat=@Fiyat,Yatma_Bedeli=@Yatma_Bedeli,Kdv_Fiyat=@Kdv_Fiyat,Kdv_Oranı=@Kdv_Oranı,Konteyner_No=@Konteyner_No,Odeme_Durumu=@Odeme_Durumu Where Tarih Like '%" + dateTimePicker3.Text + "%' And Isi_Veren_Firma Like '%"+firma+ "%' And Konteyner_Bosaltigi_Yer Like '%"+verdigifirma+ "%' And Yuk_Bosaltigi_Yer Like '%"+richTextBox13.Text+"%'";

            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@tarih", dateTimePicker1.Text);
            komut.Parameters.AddWithValue("@Isi_Veren_Firma", richTextBox1.Text.ToUpper());
            komut.Parameters.AddWithValue("@Yukledigi_Firma", richTextBox2.Text.ToUpper());
            komut.Parameters.AddWithValue("@Yuk_Bosaltigi_Yer", richTextBox3.Text.ToUpper());
            komut.Parameters.AddWithValue("@Konteyner_Aldigi_Yer", richTextBox4.Text.ToUpper());
            komut.Parameters.AddWithValue("@Konteyner_Biraktigi_Yer", richTextBox6.Text.ToUpper());
            komut.Parameters.AddWithValue("@Fiyat", fiyat);
            komut.Parameters.AddWithValue("@Yatma_Bedeli", yatmabedeli);
            komut.Parameters.AddWithValue("@Kdv_Fiyat",toplam);
            komut.Parameters.AddWithValue("@Kdv_Oranı",kdv);
            komut.Parameters.AddWithValue("@Konteyner_No", richTextBox8.Text.ToUpper());
                    if (radioButton1.Checked)
                    {
                        komut.Parameters.AddWithValue("@Odeme_Durumu", radioButton1.Text);
                    }
                    else
                    {
                        komut.Parameters.AddWithValue("@Odeme_Durumu", radioButton2.Text);
                    }
                    komut.ExecuteNonQuery();

                // }
                verilerigoster();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
            richTextBox6.Text = "";
            richTextBox7.Text = "";
            richTextBox8.Text = "";
            richTextBox9.Text = "";
            richTextBox11.Text = "";

            int rows = dataGridView1.Rows.Count - 1;
            label23.Text = rows.ToString();

            verilerigoster();
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
            
            if (baglanti.State == ConnectionState.Closed)
            baglanti.Open();
                string firma = richTextBox10.Text;
                if (firma=="")
                {
                    MessageBox.Show("Firma İsmi Girmek Zorunludur","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    String verdigifirma = richTextBox5.Text;
                    String verdigiyer = richTextBox13.Text;

            sorgu = "Delete From otms Where Tarih Like '%" + dateTimePicker3.Text + "%' And Isi_Veren_Firma Like '%"+firma+ "%' And Yuk_Bosaltigi_Yer Like '%"+verdigiyer+ "%' And Yuk_Bosaltigi_Yer Like '%" + richTextBox13.Text + "%'";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.ExecuteNonQuery();
                   
            baglanti.Close();
                }
            
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            int rows = dataGridView1.Rows.Count - 1;
            label23.Text = rows.ToString();
            verilerigoster();
            baglanti.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
            double fiyat = 0;
            double ykdv = 0;
            double ybdl = 0;
            double kdv = 0;
            double fyt = 0;
            double kdvfiyat = 0;
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                SqlCommand sorgu = new SqlCommand("Select Fiyat,Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms", baglanti);
                SqlDataReader oku = sorgu.ExecuteReader();
                
              

                while (oku.Read())
                {
                    //Yatma Bedeli Alınır
                    ybdl += Convert.ToDouble(oku["Yatma_Bedeli"]);
                    //Kdv Oranı Alınır
                    kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                    //Yatma Bedelinin Kdvsi Hesaplanıp Eklenir
                    ykdv = ybdl*kdv/100;
                    double tkdv = ykdv + ybdl;
                    //Kdvli Fiyat Alınır
                   kdvfiyat += Convert.ToDouble(oku["Kdv_Fiyat"]);
                    //Fiyat Alınır
                    fiyat += Convert.ToDouble(oku["Fiyat"]);
                    //Kdvsiz Fiyat Hesaplanır
                    double toplam = fiyat + ybdl;
                    //Kdvli Fiyat Hesaplanır
                    fyt = kdvfiyat + tkdv;

                label18.Text = toplam.ToString();
                label19.Text = fyt.ToString();
                }
                

                baglanti.Close();
                
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Yer',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms ORDER BY Tarih ", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            double kdvtoplam = 0;

            double ykdv = 0;
            double ybdl = 0;
            double kdv = 0;
            double toplam = 0;
            double tytb = 0;

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms", baglanti);
            SqlDataReader oku = sorgu.ExecuteReader();


            while (oku.Read())
            {
                kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
                ykdv = ybdl * kdv / 100;
                tytb += ykdv + ybdl;

                for (int i = 0; i < 1; ++i)
                {
                    kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                }
                toplam = kdvtoplam + tytb;
            }
            label30.Text = toplam.ToString();
            oku.Close();
            baglanti.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

                app.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook book = app.Workbooks.Add(System.Reflection.Missing.Value);

                Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)book.Sheets[1];

                

                sheet.Name = dateTimePicker1.Text.ToString();

                
                
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[1, i + 1];
                    
                    range.Value2 = dataGridView1.Columns[i].HeaderText;
                    range.Interior.Color = Color.FromArgb(235,127,80);
                    range.Borders.Color = Color.Black;
                   
                }
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[j + 2, i + 1];
                        rng.Value2 = dataGridView1[i, j].Value;
                        rng.Interior.Color = Color.Orange;
                        rng.Borders.Color = Color.Black;
                       
                    }
                }
               
               
            }
            catch (Exception hata1)
            {
                MessageBox.Show(hata1.Message,"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
           

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToLongDateString();
            label10.Text = DateTime.Now.ToLongTimeString();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //PrintPreviewDialog onizleme = new PrintPreviewDialog();
            //onizleme.Document = pdYazici;
            //onizleme.ShowDialog();
            PrintPreviewDialog onizleme = new PrintPreviewDialog();
            onizleme.Document = pdYazici;
            ((Form)onizleme).WindowState = FormWindowState.Maximized; 
            onizleme.PrintPreviewControl.Zoom = 1.0;
            onizleme.ShowDialog();
        }

        StringFormat strFormat;
        ArrayList arrColumnLefts = new ArrayList();
        ArrayList arrColumnWidths = new ArrayList();
        int iCellHeight = 0;
        int iTotalWidth = 0;
        int iRow = 0;
        bool bFirstPage = false;
        bool bNewPage = false;
        int iHeaderHeight = 0;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (dateTimePicker5.Text == dateTimePicker4.Text)
            {
                MessageBox.Show("İki Tarih Arasını Seçtikten Sonra Yazdırılabilir", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {


                try
                {
                    int iLeftMargin = e.MarginBounds.Left;
                    int iTopMargin = e.MarginBounds.Top;
                    bool bMorePagesToPrint = false;
                    int iTmpWidth = 0;
                    bFirstPage = true;




                    if (bFirstPage)
                    {
                        foreach (DataGridViewColumn GridCol in dataGridView1.Columns)
                        {
                            iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                           (double)iTotalWidth * (double)iTotalWidth *
                                           ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                            iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                        GridCol.InheritedStyle.Font, iTmpWidth).Height);


                            arrColumnLefts.Add(iLeftMargin);
                            arrColumnWidths.Add(iTmpWidth);
                            iLeftMargin += iTmpWidth;
                        }
                    }

                    while (iRow <= dataGridView1.Rows.Count - 1)
                    {
                        DataGridViewRow GridRow = dataGridView1.Rows[iRow];

                        iCellHeight = GridRow.Height + 10;
                        int iCount = 0;

                        if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                        {
                            bNewPage = true;
                            bFirstPage = false;
                            bMorePagesToPrint = true;
                            break;
                        }
                        else
                        {
                            if (bNewPage)
                            {
                               
                                double kdvtoplam = 0;
                     
                                double ykdv = 0;
                                double ybdl = 0;
                                double kdv = 0;
                                double toplam = 0;
                                double tytb = 0;

                                

                                if (baglanti.State == ConnectionState.Closed)
                                        baglanti.Open();
                                    SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms Where Tarih BETWEEN '" + dateTimePicker4.Text + "' And '" + dateTimePicker5.Text+ "'", baglanti);
                                    SqlDataReader oku = sorgu.ExecuteReader();

                                


                                while (oku.Read())
                                    {
                                    kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                                    ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);

                                    ykdv = ybdl * kdv / 100;
                                    tytb += ykdv + ybdl;


                                    for (int i = 0; i < 1; ++i)
                                    {
                                        kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                                        

                                    }
                                        toplam = kdvtoplam + tytb;
                                   
                                    }
                               

                                oku.Close();
                               baglanti.Close();

                                if (baglanti.State == ConnectionState.Closed)
                                    baglanti.Open();

                                SqlCommand sorgu2 = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms Where Odeme_Durumu Like '%Ödendi%'",baglanti);
                                SqlDataReader Read = sorgu2.ExecuteReader();

                                double fiyat = 0;
                                double kdvoran = 0;
                                double yatmabedel = 0;
                                double yatmakdvsi = 0;
                                double toplamfyt = 0;

                                while (Read.Read())
                                {
                                    kdvoran = Convert.ToDouble(Read["Kdv_Oranı"]);
                                    yatmabedel = Convert.ToDouble(Read["Yatma_Bedeli"]);
                                    yatmakdvsi += (yatmabedel * kdvoran / 100) + yatmabedel;
                                    for (int i = 0; i < 1; ++i)
                                    {
                                        fiyat += Convert.ToDouble(Read["Kdv_Fiyat"]);
                                    }
                                    toplamfyt = fiyat + yatmakdvsi;
                                }
                                Read.Close();
                                baglanti.Close();


                                if (dataGridView1.Rows.Count-1==0)
                                {
                                    e.Graphics.DrawString(dateTimePicker4.Text + " / " + " " + dateTimePicker5.Text + " Arasında Yapılan İşler        Listelenen İş Sayısı : " + label23.Text + "     Listelenen İşlerin Toplam Fiyatı : " + toplam.ToString() , new System.Drawing.Font(dataGridView1.Font, FontStyle.Regular),
                                      Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                      e.Graphics.MeasureString(dateTimePicker4.Text + " / " + " " + dateTimePicker5.Text + " Arasında Yapılan İşler        Listelenen İş Sayısı : " + label23.Text + "Listelenen İşlerin Toplam Fiyatı : " + toplam.ToString() , new System.Drawing.Font(dataGridView1.Font,
                                      FontStyle.Regular), e.MarginBounds.Width).Height - 13);

                                    String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();

                                    e.Graphics.DrawString(strDate, new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold),
                                            Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                            e.Graphics.MeasureString(strDate, new System.Drawing.Font(dataGridView1.Font,
                                            FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                            e.Graphics.MeasureString(dateTimePicker4.Text + " / " + " " + dateTimePicker5.Text + " Arasında Yapılan İşler        Listelenen İş Saysısı" + label23.Text + "     Listelenen İşlerin Toplam Fiyatı : " + toplam.ToString() , new System.Drawing.Font(new System.Drawing.Font(dataGridView1.Font,
                                            FontStyle.Regular), FontStyle.Regular), e.MarginBounds.Width).Height - 13);
                                }
                                else
                                {
                                    e.Graphics.DrawString(dateTimePicker4.Text + " / " + " " + dateTimePicker5.Text + " Arasında Yapılan İşler        Listelenen İş Sayısı : " + label23.Text + "     Listelenen İşlerin Toplam Fiyatı : " + toplam.ToString() + "\n Ödenen İşlerin Toplamı : " + toplamfyt, new System.Drawing.Font(dataGridView1.Font, FontStyle.Regular),
                                       Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                       e.Graphics.MeasureString(dateTimePicker4.Text + " / " + " " + dateTimePicker5.Text + " Arasında Yapılan İşler        Listelenen İş Sayısı : " + label23.Text + "Listelenen İşlerin Toplam Fiyatı : " + toplam.ToString() + "\n Ödenen İşlerin Toplamı : " + toplamfyt, new System.Drawing.Font(dataGridView1.Font,
                                       FontStyle.Regular), e.MarginBounds.Width).Height - 13);

                                    String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();

                                    e.Graphics.DrawString(strDate, new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold),
                                            Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                            e.Graphics.MeasureString(strDate, new System.Drawing.Font(dataGridView1.Font,
                                            FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                            e.Graphics.MeasureString(dateTimePicker4.Text + " / " + " " + dateTimePicker5.Text + " Arasında Yapılan İşler        Listelenen İş Saysısı" + label23.Text + "     Listelenen İşlerin Toplam Fiyatı : " + toplam.ToString() + "\n Ödenen İşlerin Toplamı : " + toplamfyt, new System.Drawing.Font(new System.Drawing.Font(dataGridView1.Font,
                                            FontStyle.Regular), FontStyle.Regular), e.MarginBounds.Width).Height - 13);

                                }


                                iTopMargin = e.MarginBounds.Top;
                                foreach (DataGridViewColumn GridCol in dataGridView1.Columns)
                                {
                                    e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                        new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                    e.Graphics.DrawRectangle(Pens.Black,
                                        new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                    e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                        new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                        new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);

                                   
                                    iCount++;
                                }
                                

                                bNewPage = false;
                                iTopMargin += iHeaderHeight;
                            }
                            iCount = 0;

                            foreach (DataGridViewCell Cel in GridRow.Cells)
                            {
                                if (Cel.Value != null)
                                {
                                    e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                                new SolidBrush(Cel.InheritedStyle.ForeColor),
                                                new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                                (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);


                                }

                                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle((int)arrColumnLefts[iCount],
                                 iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));



                                iCount++;
                            }
                        }
                        iRow++;
                        iTopMargin += iCellHeight;
                    }


                    if (bMorePagesToPrint)
                        e.HasMorePages = true;
                    else
                        e.HasMorePages = false;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pdYazici_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (dateTimePicker5.Text == dateTimePicker4.Text)
            {
                MessageBox.Show("İki Tarih Arasını Seçtikten Sonra Yazdırılabilir", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBoxManager.Yes = "Yatay";
                MessageBoxManager.No = "Dikey";
                MessageBoxManager.Register();

             DialogResult scm=MessageBox.Show("Sayfayı Nasıl Yazdırmak İstersiniz ?","Tır Otomasyonu",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (scm==DialogResult.Yes)
                {
                   
                    try
                    {
                        strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Near;
                        strFormat.LineAlignment = StringAlignment.Center;
                        strFormat.Trimming = StringTrimming.EllipsisCharacter;

                        arrColumnLefts.Clear();
                        arrColumnWidths.Clear();

                        iCellHeight = 0;
                        iRow = 0;
                        bFirstPage = true;
                        bNewPage = true;

                        iTotalWidth = 0;
                        foreach (DataGridViewColumn dgvGridCol in dataGridView1.Columns)
                        {
                            iTotalWidth += dgvGridCol.Width;
                        }
                        pdYazici.DefaultPageSettings.Landscape = true;
                        pdYazici.OriginAtMargins = true;
                        pdYazici.DefaultPageSettings.Margins.Left = 8;
                        pdYazici.DefaultPageSettings.Margins.Right = 20;
                        pdYazici.DefaultPageSettings.Margins.Top = 40;
                        pdYazici.DefaultPageSettings.Margins.Bottom = 50;
                    
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglanti.Close();
                    }
                }
                else if(scm==DialogResult.No)
                {
                    try
                    {
                        strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Near;
                        strFormat.LineAlignment = StringAlignment.Center;
                        strFormat.Trimming = StringTrimming.EllipsisCharacter;

                        arrColumnLefts.Clear();
                        arrColumnWidths.Clear();

                        iCellHeight = 0;
                        iRow = 0;
                        bFirstPage = true;
                        bNewPage = true;

                        iTotalWidth = 0;
                        foreach (DataGridViewColumn dgvGridCol in dataGridView1.Columns)
                        {
                            iTotalWidth += dgvGridCol.Width;
                        }
                        if (dataGridView1.Rows.Count - 1 >= 22)
                        {
                            bNewPage = true;
                        }
                        pdYazici.DefaultPageSettings.Landscape = false;
                        pdYazici.OriginAtMargins = true;
                        pdYazici.DefaultPageSettings.Margins.Left = 10;
                        pdYazici.DefaultPageSettings.Margins.Right = 20;
                        pdYazici.DefaultPageSettings.Margins.Top = 40;
                        pdYazici.DefaultPageSettings.Margins.Bottom = 15;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglanti.Close();
                    }
                }
               
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
           DialogResult Scm= MessageBox.Show("Programı Kapatmak İstiyormusunuz", "Tır Otomasyonu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (Scm==DialogResult.OK)
            {
            Application.Exit();
            }
           
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                if (dateTimePicker4.Text==dateTimePicker5.Text)
                {
                    MessageBox.Show("İki Tarih Aynı Olamaz","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                baglanti.Open();
                   
                   // string tarih1 = dateTimePicker4.Text;
                    //string tarih2 = dateTimePicker5.Text;
                SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Tarih BETWEEN '" + dateTimePicker4.Text+"' And '"+dateTimePicker5.Text+"'", baglanti);
                DataTable dtable = new DataTable();
                dAdapter.Fill(dtable);
                dataGridView1.DataSource = dtable;
                baglanti.Close();
                }
                double kdvtoplam = 0;

                double ykdv = 0;
                double ybdl = 0;
                double kdv = 0;
                double toplam = 0;
                double tytb = 0;

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms Where Tarih BETWEEN '" + dateTimePicker4.Text + "' And '" + dateTimePicker5.Text + "'", baglanti);
                SqlDataReader oku = sorgu.ExecuteReader();


                while (oku.Read())
                {
                    kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                    ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
                    ykdv = ybdl * kdv / 100;
                    tytb += ykdv + ybdl;

                    for (int i = 0; i < 1; ++i)
                    {
                        kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                    }
                    toplam = kdvtoplam + tytb;
                }
                label30.Text = toplam.ToString();
                oku.Close();
                baglanti.Close();
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                
                int x = dataGridView1.Rows.Count - 1;
                label23.Text = x.ToString();
            }
            button5.Enabled = true;
        }

        private void dövizHesaplamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (dateTimePicker4.Text == dateTimePicker5.Text)
            {
                button11.Enabled = false;
            }
            else if (dateTimePicker4.Value < dateTimePicker5.Value)
            {
                button11.Enabled = true;
            }
            if (dateTimePicker4.Value > dateTimePicker5.Value)
            {
                button5.Enabled = false;
                button11.Enabled = false;
            }
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about abt = new about();
            abt.Show();
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void richTextBox12_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text== "Firmaya Göre")
                {
                    string firma = richTextBox12.Text;
                    baglanti.Open();
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Isi_Veren_Firma Like '%" + firma + "%'", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            label23.Text = ((dataGridView1.Rows.Count) - 1).ToString();

            double kdvtoplam = 0;

            double ykdv = 0;
            double ybdl = 0;
            double kdv = 0;
            double toplam = 0;
            double tytb = 0;

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
            SqlCommand sorgu = new SqlCommand("Select Kdv_Fiyat,Yatma_Bedeli,Kdv_Oranı From otms Where Isi_Veren_Firma Like '%"+richTextBox12.Text+"%'", baglanti);
            SqlDataReader oku = sorgu.ExecuteReader();


            while (oku.Read())
            {
                kdv = Convert.ToDouble(oku["Kdv_Oranı"]);
                ybdl = Convert.ToDouble(oku["Yatma_Bedeli"]);
                ykdv = ybdl * kdv / 100;
                tytb += ykdv + ybdl;

                for (int i = 0; i < 1; ++i)
                {
                    kdvtoplam += Convert.ToDouble(oku["Kdv_Fiyat"]);
                }
                toplam = kdvtoplam + tytb;
            }
            label30.Text = toplam.ToString();
            oku.Close();
            baglanti.Close();

        }
        private void textaktar()
        {
            dateTimePicker1.Text= dataGridView1.CurrentRow.Cells[0].Value.ToString();
            richTextBox1.Text= dataGridView1.CurrentRow.Cells[1].Value.ToString();
            richTextBox2.Text= dataGridView1.CurrentRow.Cells[4].Value.ToString();
            richTextBox3.Text= dataGridView1.CurrentRow.Cells[2].Value.ToString();
            richTextBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            richTextBox6.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            richTextBox7.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            richTextBox9.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            richTextBox11.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            richTextBox8.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
            string odeme = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            if (odeme=="Ödendi")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            //Güncelleme Sekmesi
            dateTimePicker3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            richTextBox10.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            richTextBox5.Text= dataGridView1.CurrentRow.Cells[4].Value.ToString();
            richTextBox13.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

        }
        
        private void sevk()
        {

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textaktar();

        }

        private void programıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Scm = MessageBox.Show("Programı Kapatmak İstiyormusunuz", "Tır Otomasyonu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (Scm == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void iletişimBilgileriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inf inff = new inf();
            inff.Show();
        }

        private void destekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Destek dst = new Destek();
            dst.Show();
        }

        private void destekTalepleriniGörToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help tlp = new help();
            tlp.Show();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
        
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBoxManager.OK = "Tamam";
            //MessageBoxManager.Register();
            
            //MessageBox.Show("Geliştirme Aşamasında","Tır Otomasyonu",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void richTextBox14_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Konteyner No'ya Göre")
                {
                    string firma = richTextBox14.Text;
                    baglanti.Open();
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Konteyner_No Like '%" + firma + "%'", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            label23.Text = ((dataGridView1.Rows.Count) - 1).ToString();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Tarihe Göre")
                {
                    string firma = dateTimePicker2.Text;
                    baglanti.Open();
                    SqlDataAdapter dAdapter = new SqlDataAdapter("Select  Tarih as 'Tarih',Isi_Veren_Firma as 'İşi Veren Firma',Yuk_Bosaltigi_Yer as 'Yükü Boşaltığı Yer',Konteyner_Aldigi_Yer as 'Konteyner Aldığı Yer',Konteyner_Bosaltigi_Yer as 'Konteyner Boşaltığı Firma',Konteyner_Biraktigi_Yer as 'Konteyner Bıraktığı Yer',Fiyat,Yatma_Bedeli as 'Yatma Bedeli',Kdv_Oranı as 'Kdv Oranı',Kdv_Fiyat as 'Kdvli Fiyat',Konteyner_No as 'Konteyner No',Odeme_Durumu as 'Ödeme Durumu' From otms Where Tarih Like '%" + firma + "%'", baglanti);
                    DataTable dtable = new DataTable();
                    dAdapter.Fill(dtable);
                    dataGridView1.DataSource = dtable;
                    baglanti.Close();

                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close();
            }
            label23.Text = ((dataGridView1.Rows.Count) - 1).ToString();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hakkındaToolStripMenuItem.Text = "About";
            label5.Text = "Date";
            button1.Text = "Add";
            türkçeToolStripMenuItem.BackColor = Color.White;
            türkçeToolStripMenuItem.ForeColor = Color.Black;
            englishToolStripMenuItem.BackColor = Color.Red;
            englishToolStripMenuItem.ForeColor = Color.White;
        }

        private void türkçeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hakkındaToolStripMenuItem.Text = "Hakkında";
            label5.Text = "Tarih";
            button1.Text = "Ekle";
            türkçeToolStripMenuItem.BackColor = Color.Red;
            türkçeToolStripMenuItem.ForeColor = Color.White;
            englishToolStripMenuItem.BackColor = Color.White;
            englishToolStripMenuItem.ForeColor = Color.Black;

        }

        private void adminGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }
    }
}
