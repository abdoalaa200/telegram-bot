using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using System.Data.SqlClient;
using System.IO;

namespace telegram
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data Source=LAPTOP-D3TV88PP;Initial Catalog=telegram;User ID=sa;Password=123@123");
        SqlDataReader rdr = null;
        private void button4_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("select * from person order by name", cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string name = ds.Tables[0].Rows[i][1].ToString();
                string id = ds.Tables[0].Rows[i][0].ToString();
                string c_box = name + " - " + id;
                comboBox1.Items.Add(c_box);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("select * from person order by name", cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string name = ds.Tables[0].Rows[i][1].ToString();
                string id = ds.Tables[0].Rows[i][0].ToString();
                string c_box = name + " - " + id;
                comboBox1.Items.Add(c_box);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = comboBox1.SelectedItem.ToString();
            if (listBox1.Items.Contains(s)==false)
            listBox1.Items.Add(s);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("delete_group", cnn);
                cmd.Parameters.Add(new SqlParameter("@telegram_group", textBox1.Text.ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                rdr = cmd.ExecuteReader();
                cnn.Close();
            }
            else
            {
                MessageBox.Show("enter a group name");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (textBox1.Text != "")
            {
                cnn.Open();
                string s = textBox1.Text;
                SqlCommand cmd1 = new SqlCommand("select * from groups where gname=@name;", cnn);
                cmd1.Parameters.AddWithValue("@name", s);
                SqlDataReader reader = cmd1.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("this group had made already !");
                }
                else
                {
                    flag = true;
                }
                cnn.Close();
                if (flag==true)
                {
                    foreach (string i in listBox1.Items)
                    {
                        cnn.Open();
                        SqlCommand cmd = new SqlCommand("insert_into_group", cnn);
                        cmd.Parameters.Add(new SqlParameter("@telegram_group", textBox1.Text.ToString()));
                        cmd.Parameters.Add(new SqlParameter("@telegram_person", i));
                        cmd.CommandType = CommandType.StoredProcedure;
                        rdr = cmd.ExecuteReader();
                        cnn.Close();
                    }
                }

            }
            else
            {
                MessageBox.Show("enter a group name");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            if (textBox2.Text != "")
            {
                cnn.Open();
                string s = textBox2.Text;
                SqlCommand cmd1 = new SqlCommand("select * from person_per_group where gname=@name;", cnn);
                cmd1.Parameters.AddWithValue("@name", s);
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                DataSet ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string q = ds.Tables[0].Rows[i][0].ToString();
                    listBox2.Items.Add(q);
                }
                cnn.Close();
            }
            else
            {
                MessageBox.Show("enter a group name");
            }
        }
    }
}
