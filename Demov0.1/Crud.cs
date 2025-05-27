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
using System.Diagnostics;

namespace Demov0._1
{
    public partial class Crud : Form
    {
        private SQLiteConnection sqlite_conn;
        //นำข้อมูลในฐานข้อมูลมาอยู่ในรูปแบบ List<string>
        private List<string> originalItems = new List<string>();
        private List<string> originalItems2 = new List<string>();

        int index;
        

        public Crud()
        {
            //ฐานข้อมูล
            InitializeComponent();
            sqlite_conn = new SQLiteConnection("Data Source=DB.db;Version=3;");
            
            sqlite_conn.Open();
            


            /*string createTableQuery = "CREATE TABLE IF NOT EXISTS Messages (Id INTEGER PRIMARY KEY, ชื่ออุปกรณ์ TEXT, ชนิดอุปกรณ์ TEXT, ประวัติการยืมคืน TEXT, วัน เดือน ปี TEXT, เวลา TEXT, ชื่อผู้ใช้ TEXT, หมายเหตุ TEXT)";
            SQLiteCommand createTableCmd = new SQLiteCommand(createTableQuery, sqlite_conn);
            createTableCmd.ExecuteNonQuery();*/

            //LoadComboBoxData();


            LoadDataFromDatabase(); // โหลดข้อมูล
            SetupComboBoxSearch(); // ตั้งค่าฟังก์ชันค้นหา
            LoadDataFromDatabase2(); // โหลดข้อมูลผู้ใช้
            SetupComboBoxSearch2(); // ตั้งค่าฟังก์ชันค้นหา ComboBox2
        }
        private void LoadDataFromDatabase()
        {
            // ตัวอย่างการเชื่อมต่อ SQLite
            string connectionString = "Data Source=DB.db;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ชื่ออุปกรณ์ FROM Equipment"; // เปลี่ยนชื่อตาราง/คอลัมน์ตามจริง
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        originalItems.Add(reader["ชื่ออุปกรณ์"].ToString());
                    }
                }
            }

            // ใส่รายการลง ComboBox
            comboBox1.Items.AddRange(originalItems.ToArray());
        }

        private void SetupComboBoxSearch()
        {
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBox1.TextChanged += comboBox1_SelectedIndexChanged;
        }

        private void LoadDataFromDatabase2()
        {
            // ตัวอย่างการเชื่อมต่อ SQLite
            string connectionString = "Data Source=DB.db;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ชื่อ FROM User"; // เปลี่ยนชื่อตาราง/คอลัมน์ตามจริง
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        originalItems2.Add(reader["ชื่อ"].ToString());
                    }
                }
            }

            // ใส่รายการลง ComboBox
            comboBox2.Items.AddRange(originalItems2.ToArray());
        }

        private void SetupComboBoxSearch2()
        {
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBox2.TextChanged += comboBox2_SelectedIndexChanged;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxData()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            // โหลดข้อมูลอุปกรณ์
            string equipmentQuery = "SELECT DISTINCT ชื่ออุปกรณ์ FROM Equipment";
            SQLiteCommand equipmentCmd = new SQLiteCommand(equipmentQuery, sqlite_conn);
            HashSet<string> uniqueEquipment = new HashSet<string>();

            using (SQLiteDataReader reader = equipmentCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string item = reader["ชื่ออุปกรณ์"].ToString();
                    if (!uniqueEquipment.Contains(item))
                    {
                        uniqueEquipment.Add(item);
                        comboBox1.Items.Add(item);
                    }
                }
            }

            // โหลดข้อมูลผู้ใช้
            string userQuery = "SELECT DISTINCT ชื่อ FROM User";
            SQLiteCommand userCmd = new SQLiteCommand(userQuery, sqlite_conn);
            HashSet<string> uniqueUsers = new HashSet<string>();

            using (SQLiteDataReader reader = userCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string item = reader["ชื่อ"].ToString();
                    if (!uniqueUsers.Contains(item))
                    {
                        uniqueUsers.Add(item);
                        comboBox2.Items.Add(item);
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
            // ดึงค่าจากฟอร์ม
            string text1 = comboBox1.SelectedItem?.ToString();
            string user = comboBox2.SelectedItem?.ToString();
            string note = richTextBox6.Text;
            string phone = textBox2.Text;
            string address = textBox3.Text;
            string time = dateTimePicker1.Text;

            string getLastCodeQuery = "SELECT MAX(ลำดับ) FROM Return";
            SQLiteCommand getCodeCmd = new SQLiteCommand(getLastCodeQuery, sqlite_conn);
            object result = getCodeCmd.ExecuteScalar();
            int newId = (result != DBNull.Value) ? Convert.ToInt32(result) + 1 : 0;
            string newCode = newId.ToString("D10");

            // แปลงจำนวนเป็นตัวเลข
            if (!int.TryParse(richTextBox1.Text, out int amount) || amount <= 0)
            {
                MessageBox.Show("กรุณากรอกจำนวนที่ถูกต้อง (ต้องเป็นตัวเลขมากกว่า 0)",
                              "ข้อผิดพลาด",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            try
            {
                // คำสั่ง SQL ที่ถูกต้อง
                string insertBorrowMessage = @"INSERT INTO Borrow 
        (ชื่ออุปกรณ์, วันที่, ชื่อผู้ยืม, เบอร์โทร, ที่อยู่, หมายเหตุ, จำนวน, ประวัติการยืม, รายการ) 
        VALUES 
        (@Text1, @Date, @User, @Phone, @Address, @Note, @Amount, @History ,@Code)";

                SQLiteCommand cmd = new SQLiteCommand(insertBorrowMessage, sqlite_conn);

                // กำหนดค่าพารามิเตอร์
                cmd.Parameters.AddWithValue("@Text1", text1);
                cmd.Parameters.AddWithValue("@Date", time);
                cmd.Parameters.AddWithValue("@User", user);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Note", note);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Code", newCode);

                // กำหนดค่าประวัติการยืม (ตัวอย่าง)
                string borrowHistory = $"ยืม {amount} ชิ้น เมื่อ {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";
                cmd.Parameters.AddWithValue("@History", borrowHistory);
                if (string.IsNullOrWhiteSpace(note))
                {
                    string upnote = $"ยังขาด ({amount})";
                    cmd.Parameters["@Note"].Value = upnote; // แก้ค่าหาก note ว่าง
                }

                int rowsAffected = cmd.ExecuteNonQuery();


                if (rowsAffected > 0)
                {
                   
                    
                    

                     // แปลงเป็น 10 หลัก เช่น 0000000001

                    string insertReturnQuery = @"INSERT INTO Return 
                    (ลำดับ, ชื่อ, อุปกรณ์, จำนวน, วันที่, ประวัติการคืน, รายละเอียด, รายการ)
                    VALUES 
                    (@Id, @User, @Device, @Amount, @Date, @ReturnHistory, @Detail, @Code)";

                    SQLiteCommand returnCmd = new SQLiteCommand(insertReturnQuery, sqlite_conn);
                    returnCmd.Parameters.AddWithValue("@Id", newId);
                    returnCmd.Parameters.AddWithValue("@User", user);
                    returnCmd.Parameters.AddWithValue("@Device", text1);
                    returnCmd.Parameters.AddWithValue("@Amount", amount);
                    returnCmd.Parameters.AddWithValue("@Date", time);
                    returnCmd.Parameters.AddWithValue("@ReturnHistory", $"0/{amount}");
                    returnCmd.Parameters.AddWithValue("@Detail", note);
                    returnCmd.Parameters.AddWithValue("@Code", newCode);

                    returnCmd.ExecuteNonQuery();

                    ClearForm();
                    LoadData();
                    MessageBox.Show("บันทึกข้อมูลการยืมและรายการคืนสำเร็จ");
                }
                
                

                if (rowsAffected > 0)
                {
                    ClearForm();
                    LoadData();
                    MessageBox.Show("บันทึกข้อมูลการยืมสำเร็จ");

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}\n\nโปรดตรวจสอบข้อมูลและลองอีกครั้ง",
                              "ข้อผิดพลาด",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }


        private void ClearForm()
        {
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear(); // ล้างข้อมูลใน ComboBox
            richTextBox1.Clear();
            textBox2.Clear();
            //textBox3.Clear();
            comboBox2.SelectedIndex = -1;
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

                string selectQuery = "SELECT ลำดับ, ชื่ออุปกรณ์, วันที่, ชื่อผู้ยืม, เบอร์โทร, ที่อยู่, หมายเหตุ, จำนวน FROM Borrow";

                SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, sqlite_conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns["ลำดับ"].Width = 50; 
                dataGridView1.Columns["วันที่"].Width = 150;
                //dataGridView1.Columns["ชนิดอุปกรณ์"].Visible = false;
                //dataGridView1.Columns["เวลา"].Visible = false;
                dataGridView1.Columns["จำนวน"].Visible = false;
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
                                int id = Convert.ToInt32(selectedRow.Cells["ลำดับ"].Value);

                                string deleteQuery = "DELETE FROM Borrow WHERE ลำดับ = @Id";
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
                    int id = Convert.ToInt32(row.Cells["ลำดับ"].Value);

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
                        SQLiteCommand updateEquipmentCmd = new SQLiteCommand(updateEquipmentQuery, sqlite_conn);
                        updateEquipmentCmd.Parameters.AddWithValue("@c", Math.Abs(c)); // ใช้ค่าความต่าง (บวก)
                        updateEquipmentCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", comboBox1.Text);
                        updateEquipmentCmd.ExecuteNonQuery();
                    }

                    // อัปเดตข้อมูลใน Messages
                    string updateQuery = @"
            UPDATE Borrow 
            SET ชื่ออุปกรณ์ = @Text1, 
                 
                 
                วันที่ = @Text4, 
                เวลา = @Time, 
                ชื่อผู้ยืม = @Text6, 
                หมายเหตุ = @Text7,
                จำนวน = @rich1
            WHERE Id = @Id";
                    SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, sqlite_conn);
                    updateCmd.Parameters.AddWithValue("@Text1", comboBox1.Text);
                    //updateCmd.Parameters.AddWithValue("@Text2", comboBox2.Text);
                    //updateCmd.Parameters.AddWithValue("@Text3", combobox2.Text);
                    updateCmd.Parameters.AddWithValue("@Text4", dateTimePicker1.Text);
                    //updateCmd.Parameters.AddWithValue("@Time", $"{hours}:{minutes}"); ;
                    updateCmd.Parameters.AddWithValue("@Text6", comboBox2.Text);
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

                comboBox1.Text = row.Cells["ชื่ออุปกรณ์"].Value?.ToString();
                //comboBox2.Text = row.Cells[2].Value?.ToString();
                //combobox2.Text = row.Cells[3].Value?.ToString();
                dateTimePicker1.Text = row.Cells["วันที่"].Value?.ToString();
                //richTextBox4.Text = row.Cells[5].Value?.ToString();
                comboBox2.Text = row.Cells["ชื่อผู้ยืม"].Value?.ToString();
                textBox2.Text = row.Cells["เบอร์โทร"].Value?.ToString();
                textBox3.Text = row.Cells["ที่อยู่"].Value?.ToString();
                richTextBox6.Text = row.Cells["หมายเหตุ"].Value?.ToString();
                richTextBox1.Text= row.Cells["จำนวน"].Value?.ToString();

                string value = row.Cells[5].Value?.ToString();

               
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
                string searchQuery = "SELECT * FROM Borrow WHERE ชื่ออุปกรณ์  LIKE @SearchValue OR วันที่ LIKE @SearchValue OR ชื่อผู้ยืม LIKE @SearchValue ";
                using (SQLiteCommand searchCmd = new SQLiteCommand(searchQuery, sqlite_conn))
                {
                    searchCmd.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(searchCmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["รายการ"].Visible = false;
                    dataGridView1.Columns["ประวัติการยืม"].Visible = false;
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
            SELECT * FROM Borrow
            WHERE ชื่ออุปกรณ์ LIKE '%{searchValue}%'
           
            
            OR วันที่ LIKE '%{searchValue}%'
            OR ชื่อผู้ยืม LIKE '%{searchValue}%'
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
        SELECT COUNT(*) FROM Borrow
        WHERE ชื่ออุปกรณ์ LIKE '%{searchValue}%'
         
        
        OR วันที่ LIKE '%{searchValue}%'
        OR ชื่อผู้ยืม LIKE '%{searchValue}%'";

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
            string searchText = comboBox1.Text;

            comboBox1.TextChanged -= comboBox1_SelectedIndexChanged;
            int selectionStart = comboBox1.SelectionStart;

            // กรองจาก originalItems ที่มาจากฐานข้อมูล
            var filtered = originalItems
                .Where(item => item.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToArray();

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(filtered);
            comboBox1.DroppedDown = true;

            comboBox1.Text = searchText;
            comboBox1.SelectionStart = selectionStart;
            comboBox1.SelectionLength = 0;

            comboBox1.TextChanged += comboBox1_SelectedIndexChanged;
            // ตรวจสอบว่ามีการเลือกข้อมูลใน ComboBox1 หรือไม่
            if (comboBox1.SelectedItem != null)
            {
                string selectedDeviceName = comboBox1.SelectedItem.ToString(); // ชื่ออุปกรณ์ที่เลือก
                /*LoadDeviceType(selectedDeviceName); // ดึงข้อมูลชนิดอุปกรณ์และเติมใน ComboBox2*/
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchText = comboBox2.Text;

            if (comboBox2.SelectedItem != null)
            {
                string selectedName = comboBox2.SelectedItem.ToString();
                LoadUserDetails(selectedName); // เรียกเมธอดดึงข้อมูลผู้ใช้
            }
        }

        private void LoadUserDetails(string userName)
        {
            try
            {
                // แสดงชื่อที่กำลังค้นหา (สำหรับ debug)
                Debug.WriteLine($"กำลังค้นหาชื่อผู้ใช้: '{userName}'");

                string query = "SELECT เบอร์โทร, ที่อยู่ FROM User WHERE ชื่อ = @UserName";

                using (SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) // ตรวจสอบว่ามีข้อมูลหรือไม่
                        {
                            while (reader.Read())
                            {
                                // Debug ข้อมูลที่ได้
                                Debug.WriteLine($"พบข้อมูล: เบอร์โทร={reader["เบอร์โทร"]}, ที่อยู่={reader["ที่อยู่"]}");

                                // เติมข้อมูลใน TextBox
                                textBox2.Text = reader["เบอร์โทร"]?.ToString() ?? "";
                                textBox3.Text = reader["ที่อยู่"]?.ToString() ?? "";
                            }
                        }
                        else
                        {
                            Debug.WriteLine("ไม่พบข้อมูลผู้ใช้");
                            textBox2.Text = "";
                            textBox3.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"เกิดข้อผิดพลาด: {ex.Message}");
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckUserData()
        {
            try
            {
                string query = "SELECT ชื่อ, เบอร์โทร, ที่อยู่ FROM User";
                SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn);
                DataTable dt = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ข้อมูลในตาราง User:");
                foreach (DataRow row in dt.Rows)
                {
                    sb.AppendLine($"{row["ชื่อ"]} | {row["เบอร์โทร"]} | {row["ที่อยู่"]}");
                }

                MessageBox.Show(sb.ToString(), "ข้อมูลผู้ใช้ทั้งหมด");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ไม่สามารถตรวจสอบข้อมูล: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ใน constructor
        
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
