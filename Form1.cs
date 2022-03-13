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
using System.Net.NetworkInformation;


namespace telegram
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data Source=LAPTOP-D3TV88PP;Initial Catalog=telegram;User ID=sa;Password=123@123");
        SqlDataReader rdr = null;
        static TelegramBotClient bot = new TelegramBotClient("1828142168:AAGiAMmzShvt89h-L3TfgNjFAuabZ6478Pc");
        string[,] arr_reply=new string[100000,2];
        
        public async void send_m(string id,string m)
        {
            try
            {
                await bot.SendTextMessageAsync(chatId: id, text: m);
            }
            catch
            { MessageBox.Show("enter or select a valid id and enter a message to send"); }
        }
        public async void send_p(string id, InputOnlineFile f)
        {
            try { await bot.SendPhotoAsync(id, f); }
            catch
            { MessageBox.Show("enter a valid id to send to"); }

        }
        public async void send_f(string id, InputOnlineFile f)
        {
            try { await bot.SendDocumentAsync(id, f); }
            catch
            { MessageBox.Show("enter a valid id to send to"); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                SqlCommand cmd = new SqlCommand("select * from person;", cnn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = ds.Tables[0].Rows[i][0].ToString();
                    send_m(id, textBox2.Text);
                }
            }
            else if(checkBox3.Checked)
            {
                int x = comboBox2.SelectedIndex;
                if (x != -1)
                {
                    string s2 = comboBox2.SelectedItem.ToString();
                    SqlCommand cmd3 = new SqlCommand("select * from person_per_group where gname=@name;", cnn);
                    cmd3.Parameters.AddWithValue("@name", s2);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataSet ds3 = new DataSet();
                    da3.Fill(ds3);
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        string s = ds3.Tables[0].Rows[i][0].ToString();
                        string s1 = "";
                        bool flag = false;
                        for (int j = 0; j < s.Length; j++)
                        {
                            if (flag == true)
                                s1 += s[j];
                            if (s[j] == '-')
                                flag = true;
                        }
                        send_m(s1, textBox2.Text);
                    }
                }
                else
                {
                    MessageBox.Show("select group id to send to");
                }
            }
            else if (checkBox1.Checked)
            {
                if (textBox1.Text.Length > 0)
                {
                    string s = textBox1.Text;
                    send_m(s, textBox2.Text);
                }
                else
                {
                    MessageBox.Show("enter an id to send to");
                }
            }
            else
            {
                int x = comboBox1.SelectedIndex;
                if (x != -1)
                {
                    string s = comboBox1.SelectedItem.ToString();
                    string s1 = "";
                    bool flag = false;
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (flag == true)
                            s1 += s[i];
                        if (s[i] == '-')
                            flag = true;
                    }
                    SqlCommand cmd = new SqlCommand("select * from person where id = " + s1 + ";", cnn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string id = ds.Tables[0].Rows[i][0].ToString();
                        send_m(id, textBox2.Text);
                    }
                    c_load();
                }
                else
                {
                    MessageBox.Show("select an id to send to");
                }
            }
        }
        public void c_load()
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();
            cnn.Open();
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
            cnn.Close();
            cnn.Open();
            SqlCommand cmd2 = new SqlCommand("select gname from groups", cnn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2);
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                string name1 = ds2.Tables[0].Rows[i][0].ToString();
                comboBox2.Items.Add(name1);
            }
            cnn.Close();
            cnn.Open();
            SqlCommand cmd1 = new SqlCommand("select * from reply;", cnn);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                arr_reply[i, 0] = ds1.Tables[0].Rows[i][0].ToString();
                arr_reply[i, 1] = ds1.Tables[0].Rows[i][1].ToString();
            }
            cnn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if ((reply.Status == IPStatus.Success) == true)
                    MessageBox.Show("internet is good");
            }
            catch (Exception)
            {
                MessageBox.Show("check your internet connection");
                this.Close();
            }
            c_load();
            bot.StartReceiving();
            bot.OnMessage += Bot_OnMessage;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            c_load();
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            cnn.Open();
            SqlCommand cmd = new SqlCommand("insert_person", cnn);
            cmd.Parameters.Add(new SqlParameter("@telegram_id", e.Message.Chat.Id.ToString()));
            cmd.Parameters.Add(new SqlParameter("@telegram_name", e.Message.Chat.FirstName.ToString()));
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();
            cnn.Close();
            int x = arr_reply.Length / 2;
            string s = e.Message.Text.ToString();
            for (int i = 0; i < x; i++)
            {
                if (arr_reply[i,0] == s)
                {    
                    send_m(e.Message.Chat.Id.ToString(), arr_reply[i,1]);
                    break;
                }
            }       
        }

        private void button3_Click(object sender, EventArgs e)
        {   
            cnn.Open();
            SqlCommand cmd = new SqlCommand("insert_reply", cnn);
            cmd.Parameters.Add(new SqlParameter("@telegram_keyword", textBox3.Text.ToString()));
            cmd.Parameters.Add(new SqlParameter("@telegram_answer", textBox4.Text.ToString()));
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();
            cnn.Close();
            SqlCommand cmd1 = new SqlCommand("select * from reply;", cnn);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
               arr_reply[i,0] =ds1.Tables[0].Rows[i][0].ToString();
               arr_reply[i,1] = ds1.Tables[0].Rows[i][1].ToString();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand cmd = new SqlCommand("delete_reply", cnn);
            cmd.Parameters.Add(new SqlParameter("@telegram_keyword", textBox3.Text.ToString()));
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();
            cnn.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "PNG|*.png", ValidateNames = true, Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach(string f in ofd.FileNames)
                    { 
                    
                        if (checkBox2.Checked)
                        {
                            SqlCommand cmd = new SqlCommand("select * from person;", cnn);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string a = @f;
                                FileStream fs = File.OpenRead(a);
                                string b = f;
                                InputOnlineFile myfile = new InputOnlineFile(fs, b);
                                string id = ds.Tables[0].Rows[i][0].ToString();
                                send_p(id, myfile);
                            }
                        }
                        else if (checkBox3.Checked)
                        {
                            int x = comboBox2.SelectedIndex;
                            if (x != -1)
                            {
                                string s2 = comboBox2.SelectedItem.ToString();
                                SqlCommand cmd3 = new SqlCommand("select * from person_per_group where gname=@name;", cnn);
                                cmd3.Parameters.AddWithValue("@name", s2);
                                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                                DataSet ds3 = new DataSet();
                                da3.Fill(ds3);
                                for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                                {
                                    string a = @f;
                                    FileStream fs = File.OpenRead(a);
                                    string b = f;
                                    InputOnlineFile myfile = new InputOnlineFile(fs, b);
                                    string s = ds3.Tables[0].Rows[i][0].ToString();
                                    string s1 = "";
                                    bool flag = false;
                                    for (int j = 0; j < s.Length; j++)
                                    {
                                        if (flag == true)
                                            s1 += s[j];
                                        if (s[j] == '-')
                                            flag = true;
                                    }
                                    send_p(s1, myfile);
                                }
                            }
                            else
                            {
                                MessageBox.Show("select group id to send to");
                            }
                        }
                        else if (checkBox1.Checked)
                        {
                            string a = @f;
                            FileStream fs = File.OpenRead(a);
                            string b = f;
                            InputOnlineFile myfile = new InputOnlineFile(fs, b);
                            send_p(textBox1.Text, myfile); 
                        }
                        else
                        {
                            string a = @f;
                            FileStream fs = File.OpenRead(a);
                            string b = f;
                            InputOnlineFile myfile = new InputOnlineFile(fs, b);
                            int x = comboBox1.SelectedIndex;
                            if(x!=-1)
                            { 
                            string s = comboBox1.SelectedItem.ToString();
                            string s1 = "";
                            bool flag = false;
                            for (int i = 0; i < s.Length; i++)
                            {
                                if (flag == true)
                                    s1 += s[i];
                                if (s[i] == '-')
                                    flag = true;
                            }
                            send_p(s1, myfile);
                            }
                            else
                            {
                                MessageBox.Show("select an id to send to");
                            }
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "PDF|*.pdf", ValidateNames = true, Multiselect = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string f in ofd.FileNames)
                        {
                            
                            if (checkBox2.Checked)
                            {
                                SqlCommand cmd = new SqlCommand("select * from person;", cnn);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                string a = @f;
                                FileStream fs = File.OpenRead(a);
                                string b = f;
                                InputOnlineFile myfile = new InputOnlineFile(fs, b);
                                string id = ds.Tables[0].Rows[i][0].ToString();
                                    send_f(id,myfile);
                                }
                            }
                        else if (checkBox3.Checked)
                        {
                            int x = comboBox2.SelectedIndex;
                            if (x != -1)
                            {
                                string s2 = comboBox2.SelectedItem.ToString();
                                SqlCommand cmd3 = new SqlCommand("select * from person_per_group where gname=@name;", cnn);
                                cmd3.Parameters.AddWithValue("@name", s2);
                                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                                DataSet ds3 = new DataSet();
                                da3.Fill(ds3);
                                for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                                {
                                    string a = @f;
                                    FileStream fs = File.OpenRead(a);
                                    string b = f;
                                    InputOnlineFile myfile = new InputOnlineFile(fs, b);
                                    string s= ds3.Tables[0].Rows[i][0].ToString();
                                    string s1 = "";
                                    bool flag = false;
                                    for (int j = 0; j < s.Length; j++)
                                    {
                                        if (flag == true)
                                            s1 += s[j];
                                        if (s[j] == '-')
                                            flag = true;
                                    }
                                    send_f(s1, myfile);
                                }
                            }
                            else
                            {
                                MessageBox.Show("select group id to send to");
                            }
                        }
                        else if (checkBox1.Checked)
                            {
                            string a = @f;
                            FileStream fs = File.OpenRead(a);
                            string b = f;
                            InputOnlineFile myfile = new InputOnlineFile(fs, b); 
                            send_p(textBox1.Text, myfile); }
                            else
                            {
                            string a = @f;
                            FileStream fs = File.OpenRead(a);
                            string b = f;
                            InputOnlineFile myfile = new InputOnlineFile(fs, b);
                            int x = comboBox1.SelectedIndex;
                                if (x != -1)
                                {
                                    string s = comboBox1.SelectedItem.ToString();
                                    string s1 = "";
                                    bool flag = false;
                                    for (int i = 0; i < s.Length; i++)
                                    {
                                        if (flag == true)
                                            s1 += s[i];
                                        if (s[i] == '-')
                                            flag = true;
                                    }
                                    send_f(s1, myfile);
                                }
                                else
                                {
                                    MessageBox.Show("select an id to send to");
                                }
                            }
                        }
                    }
                }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
    }
}
