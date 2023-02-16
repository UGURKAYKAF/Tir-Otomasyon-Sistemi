using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
namespace Tır_Otomasyonu
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void abaut_Load(object sender, EventArgs e)
        {
            string _s1 =System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); // versiyon
            string _s2 = "Demir Kardeşler";
            string _s3 = Application.ProductName;
            string _s8 = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;

            label5.Text = _s1;
            label6.Text = _s2;
            label7.Text = _s3;
            label8.Text = _s8;
        }
    }
}
