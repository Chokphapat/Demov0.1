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
        int index;

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
            string text5 = richTextBox5.Text;
            string text6 = richTextBox6.Text;
            string text7 = dateTimePicker1.Text;
            
            


            if (MessageBox.Show("ต้องการยืนยันข้อมูลหรือไม่", "ยืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               
                string insertQuery = @"
                    INSERT INTO Messages
                    (ชื่ออุปกรณ์, ชนิดอุปกรณ์, ประวัติการยืมคืน, วัน_เดือน_ปี, เวลา, ชื่อผู้ใช้, หมายเหตุ)
                    VALUES
                    (@Text1, @Text2, @Text3, @Text7, @Text4, @Text5, @Text6)";
                SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, sqlite_conn);
                insertCmd.Parameters.AddWithValue("@Text1", text1);
                insertCmd.Parameters.AddWithValue("@Text2", text2);
                insertCmd.Parameters.AddWithValue("@Text3", text3);
                insertCmd.Parameters.AddWithValue("@Text4", text4);
                insertCmd.Parameters.AddWithValue("@Text5", text5);
                insertCmd.Parameters.AddWithValue("@Text6", text6);
                insertCmd.Parameters.AddWithValue("@Text7", text7);
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
            index = e.RowIndex;


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
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns["id"].Width = 30;
                dataGridView1.Columns["วัน_เดือน_ปี"].Width = 130;
                dataGridView1.Columns["เวลา"].Width = 70;

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
            if (index >= 0)
            {
                var confirmResult = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลนี้?",
                                                     "ยืนยันการลบ",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    DataGridViewRow row = dataGridView1.Rows[index];
                    int id = Convert.ToInt32(row.Cells["Id"].Value);

                    string deleteQuery = "DELETE FROM Messages WHERE Id = @Id";
                    SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, sqlite_conn);
                    deleteCmd.Parameters.AddWithValue("@Id", id);
                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show("ลบข้อมูลสำเร็จ");
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการลบ", "ข้อผิดพลาด");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (index >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[index];
                int id = Convert.ToInt32(row.Cells["Id"].Value);

                string updateQuery = @"
            UPDATE Messages 
            SET ชื่ออุปกรณ์ = @Text1, 
                ชนิดอุปกรณ์ = @Text2, 
                ประวัติการยืมคืน = @Text3, 
                วัน_เดือน_ปี = @Text4, 
                เวลา = @Text5, 
                ชื่อผู้ใช้ = @Text6, 
                หมายเหตุ = @Text7 
            WHERE Id = @Id";
                SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, sqlite_conn);
                updateCmd.Parameters.AddWithValue("@Text1", richTextBox1.Text);
                updateCmd.Parameters.AddWithValue("@Text2", richTextBox2.Text);
                updateCmd.Parameters.AddWithValue("@Text3", richTextBox3.Text);
                updateCmd.Parameters.AddWithValue("@Text4", dateTimePicker1.Text);
                updateCmd.Parameters.AddWithValue("@Text5", richTextBox4.Text);
                updateCmd.Parameters.AddWithValue("@Text6", richTextBox5.Text);
                updateCmd.Parameters.AddWithValue("@Text7", richTextBox6.Text);
                updateCmd.Parameters.AddWithValue("@Id", id);

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("อัปเดตข้อมูลสำเร็จ");
                LoadData();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการแก้ไข", "ข้อผิดพลาด");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                // อัพเดตตัวแปร index ให้ตรงกับแถวที่ถูกคลิก
                index = e.RowIndex;
                DataGridViewRow Row = dataGridView1.Rows[index];

                richTextBox1.Text = Row.Cells[1].Value?.ToString();
                richTextBox2.Text = Row.Cells[2].Value?.ToString();
                richTextBox3.Text = Row.Cells[3].Value?.ToString();
                richTextBox4.Text = Row.Cells[5].Value?.ToString();
                richTextBox5.Text = Row.Cells[6].Value?.ToString();
                richTextBox6.Text = Row.Cells[7].Value?.ToString();
                dateTimePicker1.Text = Row.Cells[4].Value?.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
