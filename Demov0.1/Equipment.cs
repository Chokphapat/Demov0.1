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
    public partial class Equipment : Form
    {
        private SQLiteConnection sqlite_conn;
        private int selectedIndex = -1;

        private int currentPage = 1; // หน้าปัจจุบัน
        private int pageSize = 10; // จำนวนแถวต่อหน้า
        private int totalRecords = 0; // จำนวนรายการทั้งหมด
        private int totalPages = 0; // จำนวนหน้าทั้งหมด

        public Equipment()
        {
            InitializeComponent();
            sqlite_conn = new SQLiteConnection("Data Source=DB.db;Version=3;");
            sqlite_conn.Open();
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {   
                string searchValue = textBox1.Text.Trim();
                string query = $@"
                        SELECT * FROM Equipment
                        WHERE ลำดับ LIKE @SearchValue
                        OR ชื่ออุปกรณ์ LIKE @SearchValue
                        LIMIT @PageSize OFFSET @Offset";

                SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn);
                cmd.Parameters.AddWithValue("@SearchValue", $"%{searchValue}%");
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@Offset", (currentPage - 1) * pageSize);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                
                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = dataTable;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.Columns["ลำดับ"].Visible = false;
                //dataGridView2.Columns["จำนวนหาย"].Visible = false;
                //dataGridView2.Columns["จำนวนไม่พร้อมใช้งาน"].Visible = false;
                //dataGridView2.Columns["จำนวนการคืน"].Visible = false;

                
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        private void UpdatePaginationInfo()
        {
            try
            {
                // คำนวณจำนวนรายการทั้งหมด
                string countQuery = @"
                    SELECT COUNT(*) FROM Equipment
                    WHERE ลำดับ LIKE @SearchValue
                    OR ชื่ออุปกรณ์ LIKE @SearchValue";

                SQLiteCommand countCmd = new SQLiteCommand(countQuery, sqlite_conn);
                countCmd.Parameters.AddWithValue("@SearchValue", $"%{textBox1.Text.Trim()}%");
                totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());

                // คำนวณจำนวนหน้าทั้งหมด
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // อัปเดต Label
                label12.Text = $"หน้าที่ {currentPage} จาก {totalPages}";
                label13.Text = $"จำนวนทั้งหมด: {totalRecords}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating pagination info: " + ex.Message);
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            
            if (sqlite_conn != null)
            {
                sqlite_conn.Close();
                sqlite_conn.Dispose();
            }
            base.OnFormClosed(e);
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void History_Load(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        private void Equipment_Load(object sender, EventArgs e)
        {
            // ตั้งค่าคอมโบบ็อกซ์สำหรับจำนวนแถว
            comboBox3.Items.Clear();
            comboBox3.Items.Add("ทั้งหมด");
            comboBox3.Items.Add("20");
            comboBox3.Items.Add("30");
            comboBox3.Items.Add("50");
            comboBox3.SelectedIndex = 1; // ตั้งค่าเริ่มต้น (เช่น 20 แถว)
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.Rows.Count)
            {
                selectedIndex = e.RowIndex;
                DataGridViewRow row = dataGridView2.Rows[selectedIndex];

                รหัส.Text = row.Cells["ลำดับ"].Value.ToString();
                ชื่อ.Text = row.Cells["ชื่ออุปกรณ์"].Value.ToString();
                จำนวน.Text = row.Cells["จำนวนทั้งหมด"].Value.ToString();
                ยืม.Text = row.Cells["จำนวนการยืม"].Value.ToString();
                คงเหลือ.Text = row.Cells["คงเหลือ"].Value.ToString();
                หาย.Text = row.Cells["จำนวนหาย"].Value.ToString();
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
             try
            {
                string insertQuery = @"
                INSERT INTO Equipment (ลำดับ, ชื่ออุปกรณ์, จำนวนทั้งหมด, จำนวนการยืม, คงเหลือ, จำนวนหาย)
                VALUES (@ลำลับ, @ชื่ออุปกรณ์,  @จำนวนทั้งหมด, @จำนวนการยืม, @คงเหลือ,  @จำนวนหาย)";

                SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, sqlite_conn);
                insertCmd.Parameters.AddWithValue("@รหัสอุปกรณ์", รหัส.Text);
                insertCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", ชื่อ.Text);
                
                insertCmd.Parameters.AddWithValue("@จำนวนทั้งหมด", จำนวน.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนการยืม", ยืม.Text);
                insertCmd.Parameters.AddWithValue("@คงดหลือ", คงเหลือ.Text);
                insertCmd.Parameters.AddWithValue("@หาย", หาย.Text);
                //insertCmd.Parameters.AddWithValue("@จำนวนไม่พร้อมใช้งาน", ไม่พร้อม.Text);
                //insertCmd.Parameters.AddWithValue("@จำนวนหาย", หาย.Text);
                
                insertCmd.ExecuteNonQuery();
                MessageBox.Show("เพิ่มข้อมูลสำเร็จ!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding data: " + ex.Message);
            }
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // ลบข้อมูล
        {
            if (selectedIndex >= 0)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells["ลำดับ"].Value);

                    string deleteQuery = "DELETE FROM Equipment WHERE Id = @Id";
                    SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, sqlite_conn);
                    deleteCmd.Parameters.AddWithValue("@Id", id);
                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show("ลบข้อมูลสำเร็จ!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting data: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการลบ");
            }
        }

        private void button3_Click(object sender, EventArgs e) // แก้ไขข้อมูล
        {
            if (selectedIndex >= 0)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells["Id"].Value);

                    string updateQuery = @"
                    UPDATE Equipment 
                    SET รหัสอุปกรณ์ = @รหัสอุปกรณ์, 
                        ชื่ออุปกรณ์ = @ชื่ออุปกรณ์, 
                        
                        จำนวนทั้งหมด = @จำนวนทั้งหมด,
                        จำนวนการยืม = @จำนวนการยืม,
                        คงเหลือ = @คงเหลือ,
                        
                        จำนวนหาย = @จำนวนหาย
                    WHERE Id = @Id";

                    SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, sqlite_conn);
                    updateCmd.Parameters.AddWithValue("@รหัสอุปกรณ์", รหัส.Text);
                    updateCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", ชื่อ.Text);
                    
                    updateCmd.Parameters.AddWithValue("@จำนวนทั้งหมด", จำนวน.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนการยืม", ยืม.Text);
                    updateCmd.Parameters.AddWithValue("@คงเหลือ", คงเหลือ.Text);
                    updateCmd.Parameters.AddWithValue("@หาย", หาย.Text);
                    //updateCmd.Parameters.AddWithValue("@จำนวนไม่พร้อมใช้งาน", ไม่พร้อม.Text);
                    //updateCmd.Parameters.AddWithValue("@จำนวนหาย", หาย.Text);
                    updateCmd.Parameters.AddWithValue("@Id", id);

                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("แก้ไขข้อมูลสำเร็จ!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating data: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการแก้ไข");
            }
        }
    


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string searchValue = textBox1.Text;
            try
            {
                string searchQuery = "SELECT * FROM Equipment WHERE รหัสอุปกรณ์ LIKE @SearchValue OR ชื่ออุปกรณ์ LIKE @SearchValue OR ชนิดอุปกรณ์ LIKE @SearchValue";
                using (SQLiteCommand searchCmd = new SQLiteCommand(searchQuery, sqlite_conn))
                {
                    searchCmd.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(searchCmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable;
                    
                }
                currentPage = 1; // รีเซ็ตเป็นหน้าแรกเมื่อมีการค้นหาใหม่
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.Message);
            }
        }
        

        private void ชนิด_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBox3.SelectedItem?.ToString();

            if (selectedValue == "ทั้งหมด")
            {
                pageSize = totalRecords; // แสดงทั้งหมด
            }
            else if (int.TryParse(selectedValue, out int selectedPageSize))
            {
                pageSize = selectedPageSize; // ตั้งค่าจำนวนแถวใหม่
            }
            else
            {
                pageSize = 10; // ค่าดีฟอลต์ถ้าการเลือกผิดพลาด
            }

            currentPage = 1; // รีเซ็ตเป็นหน้าแรก
            LoadData(); // โหลดข้อมูลใหม่ตามจำนวนแถว
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadData();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
