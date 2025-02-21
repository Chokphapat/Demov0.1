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
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string text1 = comboBox1.SelectedItem?.ToString(); // ชื่ออุปกรณ์
            //string text2 = comboBox2.Text; // ชนิดอุปกรณ์
            string user = richTextBox5.Text; // ชื่อผู้ใช้งาน
            string note = richTextBox6.Text; // หมายเหตุ
            string action = "ยืม"; // ตั้งค่าการกระทำเป็น "ยืม"
            string many = richTextBox1.Text;
            string hours = textBox2.Text;
            //string minutes = textBox3.Text;
            string Time = dateTimePicker1.Text;
            int amount = 0;

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(action) || !int.TryParse(richTextBox1.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("กรุณาเลือกชื่ออุปกรณ์ ประเภท และกรอกจำนวนที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // ดำเนินการอัปเดตการยืม
                string insertBorrowMessage = @"INSERT INTO Messages 
(ชื่ออุปกรณ์, วัน_เดือน_ปี, ชื่อผู้ใช้, เบอร์โทร, ที่อยู่, หมายเหตุ) 
VALUES (@Text1, @Date, @User, @Phone, @Address, @Note)";

                SQLiteCommand borrowMessageCmd = new SQLiteCommand(insertBorrowMessage, sqlite_conn);
                borrowMessageCmd.Parameters.AddWithValue("@Text1", text1);
                borrowMessageCmd.Parameters.AddWithValue("@Date", Time);
                borrowMessageCmd.Parameters.AddWithValue("@User", user);
                borrowMessageCmd.Parameters.AddWithValue("@Phone", textBox2.Text); // เบอร์โทร
                borrowMessageCmd.Parameters.AddWithValue("@Address", textBox3.Text); // ที่อยู่
                borrowMessageCmd.Parameters.AddWithValue("@Note", note);
                borrowMessageCmd.ExecuteNonQuery();


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
            comboBox1.Items.Clear(); // ล้างข้อมูลใน ComboBox
            richTextBox1.Clear();
            textBox2.Clear();
            //textBox3.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            //comboBox2.SelectedIndex = -1; // รีเซ็ต ComboBox อื่น (ถ้ามี)
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;


        }
        private void LoadData()
        {
            try
            {
                
                string selectQuery = "SELECT Id, ชื่ออุปกรณ์, วัน_เดือน_ปี, ชื่อผู้ใช้, เบอร์โทร, ที่อยู่, หมายเหตุ FROM Messages";

                SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, sqlite_conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns["id"].Width = 50;
                dataGridView1.Columns["วัน_เดือน_ปี"].Width = 150;
                //dataGridView1.Columns["ชนิดอุปกรณ์"].Visible = false;
                //dataGridView1.Columns["เวลา"].Visible = false;
                //dataGridView1.Columns["จำนวน"].Visible = false;
                //dataGridView1.Columns["ประวัติการยืมคืน"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
 
        private void Form1_Activated(object sender, EventArgs e)
        {
            LoadData();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // ตรวจสอบว่ามีแถวที่ถูกเลือกหรือไม่
            {
                var confirmResult = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบข้อมูลที่เลือกทั้งหมด?",
                                                     "ยืนยันการลบ",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                        {
                            if (!selectedRow.IsNewRow) // ข้ามแถวที่เป็นแถวเปล่า
                            {
                                int id = Convert.ToInt32(selectedRow.Cells["Id"].Value);

                                string deleteQuery = "DELETE FROM Messages WHERE Id = @Id";
                                SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, sqlite_conn);
                                deleteCmd.Parameters.AddWithValue("@Id", id);
                                deleteCmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("ลบข้อมูลสำเร็จ");
                        LoadData(); // โหลดข้อมูลใหม่หลังลบเสร็จ
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาดในการลบข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการลบ", "ข้อผิดพลาด");
            }
        }


        private void button4_Click(object sender, EventArgs e)//ปุ่มเเก้ไข
        {
            string hours = textBox2.Text;
            //string minutes = textBox3.Text;

            try
            {
                if (index >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[index];
                    int id = Convert.ToInt32(row.Cells["Id"].Value);

                    // ดึงค่าก่อนหน้า (จาก DataGridView)
                    int b = Convert.ToInt32(row.Cells["จำนวน"].Value); // จำนวนก่อนหน้า
                    string actionType = row.Cells["ประวัติการยืมคืน"].Value?.ToString(); // ตัวอย่าง: "ยืม" หรือ "คืน"

                    // ค่าที่แก้ไขใหม่
                    int a = int.Parse(richTextBox1.Text); // ค่าจำนวนใหม่
                    int c = a - b; // คำนวณความต่าง

                    // อัปเดตในฐานข้อมูล Equipment
                    string updateEquipmentQuery = "";
                    if (actionType.Contains("ยืม"))
                    {
                        if (c > 0)
                        {
                            // กรณีเพิ่มการยืม
                            updateEquipmentQuery = @"
                    UPDATE Equipment 
                    SET จำนวนการยืม = จำนวนการยืม + @c, 
                        จำนวนพร้อมใช้งาน = จำนวนพร้อมใช้งาน - @c
                    WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                        }
                        else if (c < 0)
                        {
                            // กรณีลดการยืม
                            updateEquipmentQuery = @"
                    UPDATE Equipment 
                    SET จำนวนการยืม = จำนวนการยืม - @c, 
                        จำนวนพร้อมใช้งาน = จำนวนพร้อมใช้งาน + @c
                    WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                        }
                    }
                    else if (actionType.Contains("คืน"))
                    {
                        if (c > 0)
                        {
                            // กรณีเพิ่มการคืน
                            updateEquipmentQuery = @"
                    UPDATE Equipment 
                    SET จำนวนการคืน = จำนวนการคืน + @c, 
                        จำนวนพร้อมใช้งาน = จำนวนพร้อมใช้งาน + @c,
                        จำนวนการยืม = จำนวนการยืม -@c
                    WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                        }
                        else if (c < 0)
                        {
                            // กรณีลดการคืน
                            updateEquipmentQuery = @"
                    UPDATE Equipment 
                    SET จำนวนการคืน = จำนวนการคืน - @c, 
                        จำนวนพร้อมใช้งาน = จำนวนพร้อมใช้งาน - @c,
                        จำนวนการยืม = จำนวนการยืม +@c
                    WHERE ชื่ออุปกรณ์ = @ชื่ออุปกรณ์";
                        }
                    }

                    if (!string.IsNullOrEmpty(updateEquipmentQuery))
                    {
                        SQLiteCommand updateEquipmentCmd = new SQLiteCommand(updateEquipmentQuery, equipment_conn);
                        updateEquipmentCmd.Parameters.AddWithValue("@c", Math.Abs(c)); // ใช้ค่าความต่าง (บวก)
                        updateEquipmentCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", comboBox1.Text);
                        updateEquipmentCmd.ExecuteNonQuery();
                    }

                    // อัปเดตข้อมูลใน Messages
                    string updateQuery = @"
            UPDATE Messages 
            SET ชื่ออุปกรณ์ = @Text1, 
                ชนิดอุปกรณ์ = @Text2, 
                ประวัติการยืมคืน = @Text3, 
                วัน_เดือน_ปี = @Text4, 
                เวลา = @Time, 
                ชื่อผู้ใช้ = @Text6, 
                หมายเหตุ = @Text7,
                จำนวน = @rich1
            WHERE Id = @Id";
                    SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, sqlite_conn);
                    updateCmd.Parameters.AddWithValue("@Text1", comboBox1.Text);
                    //updateCmd.Parameters.AddWithValue("@Text2", comboBox2.Text);
                    //updateCmd.Parameters.AddWithValue("@Text3", combobox2.Text);
                    updateCmd.Parameters.AddWithValue("@Text4", dateTimePicker1.Text);
                    //updateCmd.Parameters.AddWithValue("@Time", $"{hours}:{minutes}"); ;
                    updateCmd.Parameters.AddWithValue("@Text6", richTextBox5.Text);
                    updateCmd.Parameters.AddWithValue("@Text7", richTextBox6.Text);
                    updateCmd.Parameters.AddWithValue("@rich1", richTextBox1.Text);
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
            catch (Exception )
            {
                MessageBox.Show($"เลือกสิ่งที่จะเเก้ไขก่อนทำการกดปุ่ม", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                index = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[index];

                comboBox1.SelectedItem = row.Cells["ชื่ออุปกรณ์"].Value?.ToString();
                //comboBox2.Text = row.Cells[2].Value?.ToString();
                //combobox2.Text = row.Cells[3].Value?.ToString();
                dateTimePicker1.Text = row.Cells[4].Value?.ToString();
                //richTextBox4.Text = row.Cells[5].Value?.ToString();
                richTextBox5.Text = row.Cells[6].Value?.ToString();
                richTextBox6.Text = row.Cells[7].Value?.ToString();
                richTextBox1.Text= row.Cells[8].Value?.ToString();

                string value = row.Cells[5].Value?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    
                    string[] parts = value.Split(':');

                    // ตรวจสอบว่ามีข้อมูลเพียงพอ
                    if (parts.Length == 2)
                    {
                        textBox2.Text = parts[0]; // รับส่วนแรก เช่น 12
                        //textBox3.Text = parts[1]; // รับส่วนหลัง เช่น 16
                    }
                    else
                    {
                        textBox2.Text = ""; // กรณีที่ไม่มีข้อมูลที่ต้องการ
                        //textBox3.Text = "";
                    }
                }
                else
                {
                    textBox2.Text = ""; // กรณีค่าเป็น null หรือว่าง
                    //textBox3.Text = "";
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBox3.SelectedItem.ToString();
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
            if (comboBox3.SelectedItem != null && int.TryParse(comboBox3.SelectedItem.ToString(), out int selectedPageSize))
            {
                pageSize = selectedPageSize;
                currentPage = 1;
                LoadPagedData();
            }

            LoadPagedData();
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
                    currentPage = 1; // รีเซ็ตเป็นหน้าแรกเมื่อมีการค้นหาใหม่
                    LoadPagedData();
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
        private int currentPage = 1; // หน้าปัจจุบัน
        private int pageSize = 100; // จำนวนแถวต่อหน้า
        private int totalRecords = 0; // จำนวนแถวทั้งหมด
        private int totalPages = 0; // จำนวนหน้าทั้งหมด

        private void LoadPagedData()
        {
            try
            {
                string searchValue = textBox1.Text.Trim();
                string query = $@"
            SELECT * FROM Messages
            WHERE ชื่ออุปกรณ์ LIKE '%{searchValue}%'
            OR ชนิดอุปกรณ์ LIKE '%{searchValue}%'
            OR ประวัติการยืมคืน LIKE '%{searchValue}%'
            OR วัน_เดือน_ปี LIKE '%{searchValue}%'
            OR ชื่อผู้ใช้ LIKE '%{searchValue}%'
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

        private void UpdatePaginationInfo(string searchValue)
        {
            string countQuery = $@"
        SELECT COUNT(*) FROM Messages
        WHERE ชื่ออุปกรณ์ LIKE '%{searchValue}%'
        OR ชนิดอุปกรณ์ LIKE '%{searchValue}%'
        OR ประวัติการยืมคืน LIKE '%{searchValue}%'
        OR วัน_เดือน_ปี LIKE '%{searchValue}%'
        OR ชื่อผู้ใช้ LIKE '%{searchValue}%'";

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
        private void combobox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            /*if (textBox2.Text.Length > 2)
            {
                // ตัดข้อความให้เหลือเพียง 2 ตัวแรก
                textBox2.Text = textBox2.Text.Substring(0, 2);

                // ย้ายตำแหน่งเคอร์เซอร์ไปยังตำแหน่งท้ายสุด
                textBox2.SelectionStart = textBox2.Text.Length;
            }*/
            //เก็บข้อมูลเบอร์โทร
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            /*if (textBox3.Text.Length > 2)
            {
                // ตัดข้อความให้เหลือเพียง 2 ตัวแรก
                textBox3.Text = textBox3.Text.Substring(0, 2);

                // ย้ายตำแหน่งเคอร์เซอร์ไปยังตำแหน่งท้ายสุด
                textBox3.SelectionStart = textBox3.Text.Length;
            }*/
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            /* ตรวจสอบว่ากำลังเลือก "ทั้งหมด" หรือมีการกรองด้วยคำค้นหา
            string searchQuery = string.IsNullOrEmpty(textBox1.Text) ? "%" : textBox1.Text;
            string query = $"SELECT * FROM Messages LIMIT {pageSize} OFFSET {(currentPage - 1) * pageSize}";
            DataTable table = (DataTable)dataGridView1.DataSource;

            // เปิดหน้ารายงานและส่งข้อมูลที่จำเป็น
            var reportForm = new Report(sqlite_conn, "CRUD", totalPages, currentPage, pageSize, searchQuery);
            reportForm.Show();*/
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }


        private void label10_Click_1(object sender, EventArgs e)
        {

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

        private void label11_Click(object sender, EventArgs e)
        {
            //เเสงจำนวนหน้า #
        }

        private void label12_Click(object sender, EventArgs e)
        {
            //เเสดงจำนวนที่หาเจอ
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ตรวจสอบว่ามีการเลือกข้อมูลใน ComboBox1 หรือไม่
            if (comboBox1.SelectedItem != null)
            {
                string selectedDeviceName = comboBox1.SelectedItem.ToString(); // ชื่ออุปกรณ์ที่เลือก
                LoadDeviceType(selectedDeviceName); // ดึงข้อมูลชนิดอุปกรณ์และเติมใน ComboBox2
            }
        }

        private void LoadDeviceType(string deviceName)
        {
            try
            {
                // ล้างข้อมูลใน ComboBox2 ก่อน
                //comboBox2.Items.Clear();

                // สร้างคำสั่ง SQL เพื่อดึงชนิดของอุปกรณ์ที่สอดคล้องกับชื่ออุปกรณ์
                string query = "SELECT DISTINCT ชนิดอุปกรณ์ FROM Equipment WHERE ชื่ออุปกรณ์ = @DeviceName";
                SQLiteCommand cmd = new SQLiteCommand(query, equipment_conn);
                cmd.Parameters.AddWithValue("@DeviceName", deviceName);

                // อ่านข้อมูลจากฐานข้อมูล
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string deviceType = reader["ชนิดอุปกรณ์"].ToString();
                        //comboBox2.Items.Add(deviceType); // เติมข้อมูลลงใน ComboBox2
                    }
                }

                // ตั้งค่าให้เลือกตัวแรกโดยอัตโนมัติ (ถ้าจำเป็น)
                /*if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 0;
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            //เก็บข้อมูลที่อยู่
        }
    }
    
}
