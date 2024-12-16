using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demov0._1
{
    public partial class Crud : Form
    {
        private SQLiteConnection sqlite_conn;
        public Crud()
        {
            InitializeComponent();
            sqlite_conn = new SQLiteConnection("Data Source=your_database_v4.db;Version=3;");
            sqlite_conn.Open();

            
            string createTableQuery = "CREATE TABLE IF NOT EXISTS Messages (Id INTEGER PRIMARY KEY, ชื่ออุปกรณ์ TEXT, ชนิดอุปกรณ์ TEXT, ประวัติการยืมคืน TEXT, วัน เดือน ปี TEXT ,เวลา TEXT, ชื่อผู้ใช้ TEXT, หมายเหตุ TEXT)";
            SQLiteCommand createTableCmd = new SQLiteCommand(createTableQuery, sqlite_conn);
            createTableCmd.ExecuteNonQuery();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            string text1 = richTextBox1.Text;
            string text2 = richTextBox2.Text;
            string text3 = richTextBox3.Text;
            string text4 = richTextBox4.Text;

            
            if (MessageBox.Show("ต้องการยืนยันข้อมูลหรือไม่", "ยืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               
                string insertQuery = "INSERT INTO Messages (Text1, Text2, Text3, Text4) VALUES (@Text1, @Text2, @Text3, @Text4)";
                SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, sqlite_conn);
                insertCmd.Parameters.AddWithValue("@Text1", text1);
                insertCmd.Parameters.AddWithValue("@Text2", text2);
                insertCmd.Parameters.AddWithValue("@Text3", text3);
                insertCmd.Parameters.AddWithValue("@Text4", text4);
                insertCmd.ExecuteNonQuery();

                
                richTextBox1.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear();
                richTextBox4.Clear();
                richTextBox5.Clear();
                richTextBox6.Clear();


                LoadData();

                
                var apiClient = new ApiClient();
                await apiClient.SendDataToApiAsync(text1, text2, text3, text4);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadData()
        {
            try
            {
                string selectQuery = "SELECT * FROM Messages";
                SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, sqlite_conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            LoadData();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
