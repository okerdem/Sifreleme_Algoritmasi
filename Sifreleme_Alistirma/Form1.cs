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

namespace Sifreleme_Alistirma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-Q087NT7\SQLEXPRESS01;Initial Catalog=SifrelemeAlistirma;Integrated Security=True");

        private string sifreleme(string i)
        {
            byte[] sDizi = ASCIIEncoding.ASCII.GetBytes(i);
            string sVeri = Convert.ToBase64String(sDizi);

            return sVeri;
        }

        private string sifreCoz(string i)
        {
            byte[] cDizi = Convert.FromBase64String(i);
            string cVeri = ASCIIEncoding.ASCII.GetString(cDizi);

            return cVeri;
        }

        public void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLVERILER", con);
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                row["adsoyad"] = sifreCoz(row["adsoyad"] as string);
                row["MAIL"] = sifreCoz(row["MAIL"] as string);
                row["SIFRE"] = sifreCoz(row["SIFRE"] as string);
                row["hesapno"] = sifreCoz(row["hesapno"] as string);
            }

            dataGridView1.DataSource = dt;
        }

        private void buttonKaydet_Click(object sender, EventArgs e)
        {

            SqlCommand komut1 = new SqlCommand("insert into TBLVERILER(ADSOYAD,MAIL,SIFRE,HESAPNO) values(@ads,@m,@s,@hn)", con);
            komut1.Parameters.AddWithValue("@ads", sifreleme(textBoxAdSoyad.Text));
            komut1.Parameters.AddWithValue("@m", sifreleme(textBoxMail.Text));
            komut1.Parameters.AddWithValue("@s", sifreleme(textBoxSifre.Text));
            komut1.Parameters.AddWithValue("@hn", sifreleme(textBoxHesapNo.Text));

            con.Open();
            komut1.ExecuteNonQuery();
            con.Close();

            listele();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxAdSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBoxMail.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBoxSifre.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBoxHesapNo.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }
    }
}
