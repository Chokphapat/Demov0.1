using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demov0._1
{
    public partial class Return : Form
    {
        private SQLiteConnection sqlite_conn;
        
        int index;
        public Return()
        {
            InitializeComponent();
            this.Load += Return_Load_1;
            sqlite_conn = new SQLiteConnection("Data Source=DB.db;Version=3;");
            
            sqlite_conn.Open();


           

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                index = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[index];

                comboBox3.Text = row.Cells["ชื่อ"].Value?.ToString();
                comboBox1.Text = row.Cells["อุปกรณ์"].Value?.ToString();
                comboBox4.Text = row.Cells["วันที่"].Value?.ToString();


                textBox3.Text = row.Cells["จำนวนคืน"].Value?.ToString();
                textBox5.Text = row.Cells["รายละเอียด"].Value?.ToString();
                textBox2.Text = row.Cells["ประวัติการคืน"].Value?.ToString();



                string value = row.Cells[6].Value?.ToString();


            }

        }
        private void LoadData()
        {
            try
            {
                string selectQuery = "SELECT * FROM Return";
                SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, sqlite_conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns["ลำดับ"].Width = 50;
               // dataGridView1.Columns["วัน_เดือน_ปี"].Width = 130;
                //dataGridView1.Columns["เวลา"].Width = 70;
                dataGridView1.Columns["รายการ"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedUser = comboBox3.SelectedItem?.ToString();
            string selectedDevice = comboBox1.SelectedItem?.ToString();

            comboBox4.Items.Clear();
            comboBox4.Text = "";
            textBox3.Clear();
            textBox2.Clear();
            textBox5.Clear();

            if (!string.IsNullOrEmpty(selectedUser) && !string.IsNullOrEmpty(selectedDevice))
            {
                string query = "SELECT DISTINCT วันที่ FROM Return WHERE ชื่อ = @User AND อุปกรณ์ = @Device ORDER BY วันที่";
                using (SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn))
                {
                    cmd.Parameters.AddWithValue("@User", selectedUser);
                    cmd.Parameters.AddWithValue("@Device", selectedDevice);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox4.Items.Add(reader["วันที่"].ToString());
                        }
                    }
                }
            }
        }
        private int currentPage = 1; // หน้าปัจจุบัน
        private int pageSize = 100; // จำนวนแถวต่อหน้า
        private int totalRecords = 0; // จำนวนแถวทั้งหมด
        private int totalPages = 0; // จำนวนหน้าทั้งหมด
        private void UpdatePaginationInfo(string searchValue)
        {
            string countQuery = $@"
        SELECT COUNT(*) FROM Return
        WHERE อุปกรณ์ LIKE '%{searchValue}%'
         
        
        OR วันที่ LIKE '%{searchValue}%'
        OR ชื่อ LIKE '%{searchValue}%'";

            SQLiteCommand countCmd = new SQLiteCommand(countQuery, sqlite_conn);
            totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());

            // ตรวจสอบค่าที่ถูกต้อง
            if (totalRecords > 0 && pageSize > 0)
            {
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            }
            else
            {
                totalPages = 1; // กำหนดค่าหน้าทั้งหมดเป็น 1 อย่างน้อย
            }

            // อัปเดต Label
            label11.Text = $"หน้าที่ {currentPage} จาก {totalPages}";
            label12.Text = $"จำนวนที่ค้นหาเจอ: {totalRecords}";
        }
        private void LoadPagedData()
        {
            try
            {
                string searchValue = textBox1.Text.Trim();
                string query = $@"
            SELECT * FROM Return
            WHERE อุปกรณ์ LIKE '%{searchValue}%'
           
            
            OR วันที่ LIKE '%{searchValue}%'
            OR ชื่อ LIKE '%{searchValue}%'
            LIMIT @PageSize OFFSET @Offset";

                SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@Offset", (currentPage - 1) * pageSize);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    //MessageBox.Show("ไม่มีข้อมูลที่ตรงกับการค้นหา");
                }

                // อัปเดตข้อมูล Pagination
                UpdatePaginationInfo(searchValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string searchValue = textBox1.Text;
            try
            {
                string searchQuery = "SELECT * FROM Return WHERE อุปกรณ์  LIKE @SearchValue OR วันที่ LIKE @SearchValue OR ชื่อ LIKE @SearchValue OR รายละเอียด LIKE @SearchValue ";
                using (SQLiteCommand searchCmd = new SQLiteCommand(searchQuery, sqlite_conn))
                {
                    searchCmd.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(searchCmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                    
                    currentPage = 1; // รีเซ็ตเป็นหน้าแรกเมื่อมีการค้นหาใหม่
                    LoadPagedData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedValue = comboBox2.SelectedItem.ToString();
            if (selectedValue == "ทั้งหมด")
            {
                pageSize = totalRecords; // แสดงทั้งหมด
                currentPage = 1; // รีเซ็ตเป็นหน้าแรก
            }
            else if (int.TryParse(selectedValue, out int newPageSize))
            {
                pageSize = newPageSize;
                currentPage = 1; // รีเซ็ตเป็นหน้าแรก
            }
            if (comboBox2.SelectedItem != null && int.TryParse(comboBox2.SelectedItem.ToString(), out int selectedPageSize))
            {
                pageSize = selectedPageSize;
                currentPage = 1;
                LoadPagedData();
            }

            LoadPagedData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadPagedData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPagedData();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPagedData();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            LoadPagedData();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxData()
        {
            comboBox3.Items.Clear();
            string userQuery = "SELECT DISTINCT ชื่อ FROM Return ORDER BY ชื่อ";
            using (SQLiteCommand userCmd = new SQLiteCommand(userQuery, sqlite_conn))
            using (SQLiteDataReader reader = userCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox3.Items.Add(reader["ชื่อ"].ToString());
                }
            }
        }
        private void SetupComboBoxes()
        {
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private bool CheckDatabaseConnection()
        {
            try
            {
                if (sqlite_conn.State != ConnectionState.Open)
                {
                    sqlite_conn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message);
                return false;
            }
        }
        private void Return_Load_1(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxData();
            SetupComboBoxes();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryLoadReturnData();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void TryLoadReturnData()
        {
            if (comboBox3.SelectedItem != null && comboBox1.SelectedItem != null && comboBox4.SelectedItem != null)
            {
                string user = comboBox3.SelectedItem.ToString();
                string device = comboBox1.SelectedItem.ToString();
                string date = comboBox4.SelectedItem.ToString();

                LoadReturnRecord(user, device, date);
            }
        }

        private void LoadReturnRecord(string user, string device, string date)
        {
            try
            {
                string query = @"
            SELECT * FROM Return 
            WHERE ชื่อ = @User AND อุปกรณ์ = @Device AND วันที่ = @Date 
            ORDER BY ลำดับ ASC 
            LIMIT 1"; // ดึงรายการเก่าที่สุด

                using (SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn))
                {
                    cmd.Parameters.AddWithValue("@User", user);
                    cmd.Parameters.AddWithValue("@Device", device);
                    cmd.Parameters.AddWithValue("@Date", date);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox3.Text = reader["จำนวนคืน"].ToString();
                            textBox2.Text = reader["ประวัติการคืน"].ToString();
                            textBox5.Text = reader["รายละเอียด"].ToString();
                        }
                        else
                        {
                            textBox3.Clear();
                            textBox2.Clear();
                            textBox5.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูล: " + ex.Message);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUser = comboBox3.SelectedItem?.ToString();

            // เคลียร์ข้อมูลเก่า
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            comboBox4.Items.Clear();
            comboBox4.Text = "";
            textBox3.Clear(); // จำนวนคืน
            textBox2.Clear(); // ประวัติการคืน
            textBox5.Clear(); // รายละเอียด

            if (!string.IsNullOrEmpty(selectedUser))
            {
                string query = "SELECT DISTINCT อุปกรณ์ FROM Return WHERE ชื่อ = @User ORDER BY อุปกรณ์";
                using (SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn))
                {
                    cmd.Parameters.AddWithValue("@User", selectedUser);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["อุปกรณ์"].ToString());
                        }
                    }
                }
            }

        }
    }
}
