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
    public partial class help : Form
    {
        public help()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=.; Initial Catalog=help;Integrated Security=True");
       
        private void help_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter dAdapter = new SqlDataAdapter("Select ad as 'İsim' ,soyad as 'Soyad' , telefon as 'Telefon',posta as 'Eposta',sorun as 'Destek İsteme Nedeni' From hlp",baglanti);
            DataTable dtable = new DataTable();
            dAdapter.Fill(dtable);
            dataGridView1.DataSource = dtable;
            baglanti.Close();
        }
    }
}
