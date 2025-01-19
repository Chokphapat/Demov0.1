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
        private SQLiteConnection equipment_conn;
        int index;

        public Crud()
        {
            InitializeComponent();
            sqlite_conn = new SQLiteConnection("Data Source=your_database_v4.db;Version=3;");
            equipment_conn = new SQLiteConnection("Data Source=DatabaseAll.db;Version=3;");
            sqlite_conn.Open();
            equipment_conn.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Messages (Id INTEGER PRIMARY KEY, ชื่ออุปกรณ์ TEXT, ชนิดอุปกรณ์ TEXT, ประวัติการยืมคืน TEXT, วัน เดือน ปี TEXT, เวลา TEXT, ชื่อผู้ใช้ TEXT, หมายเหตุ TEXT)";
            SQLiteCommand createTableCmd = new SQLiteCommand(createTableQuery, sqlite_conn);
            createTableCmd.ExecuteNonQuery();

            LoadComboBoxData();

            
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;




        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxData()
        {
            comboBox1.Items.Clear(); // เพิ่มการเคลียร์ข้อมูลก่อนโหลดใหม่
            HashSet<string> uniqueItems = new HashSet<string>();

            string selectQuery = "SELECT DISTINCT ชื่ออุปกรณ์ FROM Equipment";
            SQLiteCommand cmd = new SQLiteCommand(selectQuery, equipment_conn);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string item = reader["ชื่ออุปกรณ์"].ToString();
                    if (!uniqueItems.Contains(item))
                    {
                        uniqueItems.Add(item);
                        comboBox1.Items.Add(item);
                    }
                }
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string text1 = comboBox1.SelectedItem?.ToString(); // ชื่ออุปกรณ์
            string text2 = richTextBox2.Text; // ชนิดอุปกรณ์
            string user = richTextBox5.Text; // ชื่อผู้ใช้งาน
            string note = richTextBox6.Text; // หมายเหตุ
            string action = combobox2.SelectedItem?.ToString(); // เลือกยืม/คืน
            string many = richTextBox1.Text;
            int amount = 0;

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(action) || !int.TryParse(richTextBox1.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("กรุณาเลือกชื่ออุปกรณ์ ประเภท และกรอกจำนวนที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (action == "ยืม")
                {
                    // ดำเนินการอัปเดตการยืม
                    string updateBorrowQuery = @"UPDATE Equipment 
                                         SET จำนวนการยืม = จำนวนการยืม + @amount, 
                                             จำนวนพร้อมใช้งาน = จำนวนพร้อมใช้งาน - @amount
                                         WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                    SQLiteCommand updateBorrowCmd = new SQLiteCommand(updateBorrowQuery, equipment_conn);
                    updateBorrowCmd.Parameters.AddWithValue("@amount", amount);
                    updateBorrowCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", text1);
                    updateBorrowCmd.ExecuteNonQuery();

                    // บันทึกข้อมูลใน Messages
                    string insertBorrowMessage = @"INSERT INTO Messages (ชื่ออุปกรณ์, ชนิดอุปกรณ์, ประวัติการยืมคืน, วัน_เดือน_ปี, เวลา, ชื่อผู้ใช้, หมายเหตุ, จำนวน) 
                                           VALUES (@Text1, @Text2, @History, @Date, @Time, @User, @Note, @many)";
                    SQLiteCommand borrowMessageCmd = new SQLiteCommand(insertBorrowMessage, sqlite_conn);
                    borrowMessageCmd.Parameters.AddWithValue("@Text1", text1);
                    borrowMessageCmd.Parameters.AddWithValue("@Text2", text2);
                    borrowMessageCmd.Parameters.AddWithValue("@History", $"ยืม จำนวน({amount})");
                    borrowMessageCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString());
                    borrowMessageCmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortTimeString());
                    borrowMessageCmd.Parameters.AddWithValue("@User", user);
                    borrowMessageCmd.Parameters.AddWithValue("@Note", note);
                    borrowMessageCmd.Parameters.AddWithValue("@many", many);
                    borrowMessageCmd.ExecuteNonQuery();
                }
                else if (action == "คืน")
                {
                    // ดำเนินการอัปเดตการคืน
                    string updateReturnQuery = @"UPDATE Equipment 
                                         SET จำนวนการคืน = จำนวนการคืน + @amount, 
                                             จำนวนพร้อมใช้งาน = จำนวนพร้อมใช้งาน + @amount,
                                             จำนวนการยืม = จำนวนการยืม - @amount
                                         WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                    SQLiteCommand updateReturnCmd = new SQLiteCommand(updateReturnQuery, equipment_conn);
                    updateReturnCmd.Parameters.AddWithValue("@amount", amount);
                    updateReturnCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", text1);
                    updateReturnCmd.ExecuteNonQuery();

                    // บันทึกข้อมูลใน Messages
                    string insertReturnMessage = @"INSERT INTO Messages (ชื่ออุปกรณ์, ชนิดอุปกรณ์, ประวัติการยืมคืน, วัน_เดือน_ปี, เวลา, ชื่อผู้ใช้, หมายเหตุ, จำนวน) 
                                           VALUES (@Text1, @Text2, @History, @Date, @Time, @User, @Note, @many)";
                    SQLiteCommand returnMessageCmd = new SQLiteCommand(insertReturnMessage, sqlite_conn);
                    returnMessageCmd.Parameters.AddWithValue("@Text1", text1);
                    returnMessageCmd.Parameters.AddWithValue("@Text2", text2);
                    returnMessageCmd.Parameters.AddWithValue("@History", $"คืน จำนวน({amount})");
                    returnMessageCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString());
                    returnMessageCmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortTimeString());
                    returnMessageCmd.Parameters.AddWithValue("@User", user);
                    returnMessageCmd.Parameters.AddWithValue("@Note", note);
                    returnMessageCmd.Parameters.AddWithValue("@many", many);
                    returnMessageCmd.ExecuteNonQuery();
                }

                // เคลียร์ฟอร์ม
                ClearForm();

                // โหลดข้อมูลใหม่
                LoadData();

                MessageBox.Show($"บันทึกข้อมูลการ{action}สำเร็จ");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            comboBox1.SelectedIndex = -1;
            richTextBox2.Clear();
            richTextBox1.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            combobox2.SelectedIndex = -1;
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
                dataGridView1.Columns["จำนวน"].Visible = false;
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
                updateCmd.Parameters.AddWithValue("@Text1", comboBox1.Text);
                updateCmd.Parameters.AddWithValue("@Text2", richTextBox2.Text);
                updateCmd.Parameters.AddWithValue("@Text3", combobox2.Text);
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
                index = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[index];

                comboBox1.SelectedItem = row.Cells["ชื่ออุปกรณ์"].Value?.ToString();
                richTextBox2.Text = row.Cells[2].Value?.ToString();
                combobox2.Text = row.Cells[3].Value?.ToString();
                dateTimePicker1.Text = row.Cells[4].Value?.ToString();
                richTextBox4.Text = row.Cells[5].Value?.ToString();
                richTextBox5.Text = row.Cells[6].Value?.ToString();
                richTextBox6.Text = row.Cells[7].Value?.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedItem != null)
            {
                string selectedValue = comboBox1.SelectedItem.ToString();

                
                richTextBox2.Clear();

                try
                {
                    string selectQuery = "SELECT ชนิดอุปกรณ์ FROM Equipment WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                    SQLiteCommand cmd = new SQLiteCommand(selectQuery, equipment_conn);
                    cmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", selectedValue);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            richTextBox2.Text = reader["ชนิดอุปกรณ์"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("ไม่พบข้อมูลชนิดอุปกรณ์สำหรับอุปกรณ์นี้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }
        }


        private void ค้นหา_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string searchValue = textBox1.Text;
            try
            {
                string searchQuery = "SELECT * FROM Messages WHERE ชื่ออุปกรณ์ LIKE @SearchValue OR ชนิดอุปกรณ์ LIKE @SearchValue OR ประวัติการยืมคืน LIKE @SearchValue OR วัน_เดือน_ปี LIKE @SearchValue OR ชื่อผู้ใช้ LIKE @SearchValue ";
                using (SQLiteCommand searchCmd = new SQLiteCommand(searchQuery, sqlite_conn))
                {
                    searchCmd.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(searchCmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.Message);
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (sqlite_conn != null)
            {
                sqlite_conn.Close();
                sqlite_conn.Dispose();
            }

            if (equipment_conn != null)
            {
                equipment_conn.Close();
                equipment_conn.Dispose();
            }
            base.OnFormClosed(e);
        }

        private void combobox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
