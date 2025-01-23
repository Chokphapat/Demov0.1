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

namespace Demov0._1
{
    public partial class Equipment : Form
    {
        private SQLiteConnection sqlite_conn;
        int selectedIndex = -1;

        public Equipment()
        {
            InitializeComponent();

            sqlite_conn = new SQLiteConnection("Data Source=DatabaseAll.db;Version=3;");
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

                string selectQuery = "SELECT * FROM Equipment";
                SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, sqlite_conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView2.DataSource = dataTable;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.Rows.Count)
            {
                selectedIndex = e.RowIndex;
                DataGridViewRow row = dataGridView2.Rows[selectedIndex];

                รหัส.Text = row.Cells["รหัสอุปกรณ์"].Value.ToString();
                ชื่อ.Text = row.Cells["ชื่ออุปกรณ์"].Value.ToString();
                ชนิด.Text = row.Cells["ชนิดอุปกรณ์"].Value.ToString();
                จำนวน.Text = row.Cells["จำนวนทั้งหมด"].Value.ToString();
                ยืม.Text = row.Cells["จำนวนการยืม"].Value.ToString();
                คืน.Text = row.Cells["จำนวนการคืน"].Value.ToString();
                พร้อม.Text = row.Cells["จำนวนพร้อมใช้งาน"].Value.ToString();
                ไม่พร้อม.Text = row.Cells["จำนวนไม่พร้อมใช้งาน"].Value.ToString();
                หาย.Text = row.Cells["จำนวนหาย"].Value.ToString();


                
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
             try
            {
                string insertQuery = @"
                INSERT INTO Equipment (รหัสอุปกรณ์, ชื่ออุปกรณ์, ชนิดอุปกรณ์, จำนวนทั้งหมด, จำนวนการยืม, จำนวนการคืน, จำนวนพร้อมใช้งาน, จำนวนไม่พร้อมใช้งาน, จำนวนหาย)
                VALUES (@รหัสอุปกรณ์, @ชื่ออุปกรณ์, @ชนิดอุปกรณ์, @จำนวนทั้งหมด, @จำนวนการยืม, @จำนวนการคืน, @จำนวนพร้อมใช้งาน, @จำนวนไม่พร้อมใช้งาน, @จำนวนหาย)";

                SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, sqlite_conn);
                insertCmd.Parameters.AddWithValue("@รหัสอุปกรณ์", รหัส.Text);
                insertCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", ชื่อ.Text);
                insertCmd.Parameters.AddWithValue("@ชนิดอุปกรณ์", ชนิด.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนทั้งหมด", จำนวน.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนการยืม", ยืม.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนการคืน", คืน.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนพร้อมใช้งาน", พร้อม.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนไม่พร้อมใช้งาน", ไม่พร้อม.Text);
                insertCmd.Parameters.AddWithValue("@จำนวนหาย", หาย.Text);
                
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells["Id"].Value);

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

        private void button3_Click(object sender, EventArgs e)
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
                        ชนิดอุปกรณ์ = @ชนิดอุปกรณ์,
                        จำนวนทั้งหมด = @จำนวนทั้งหมด,
                        จำนวนการยืม = @จำนวนการยืม,
                        จำนวนการคืน = @จำนวนการคืน,
                        จำนวนพร้อมใช้งาน = @จำนวนพร้อมใช้งาน,
                        จำนวนไม่พร้อมใช้งาน = @จำนวนไม่พร้อมใช้งาน,
                        จำนวนหาย = @จำนวนหาย
                    WHERE Id = @Id";

                    SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, sqlite_conn);
                    updateCmd.Parameters.AddWithValue("@รหัสอุปกรณ์", รหัส.Text);
                    updateCmd.Parameters.AddWithValue("@ชื่ออุปกรณ์", ชื่อ.Text);
                    updateCmd.Parameters.AddWithValue("@ชนิดอุปกรณ์", ชนิด.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนทั้งหมด", จำนวน.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนการยืม", ยืม.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนการคืน", คืน.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนพร้อมใช้งาน", พร้อม.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนไม่พร้อมใช้งาน", ไม่พร้อม.Text);
                    updateCmd.Parameters.AddWithValue("@จำนวนหาย", หาย.Text);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.Message);
            }
        }

        private void ชนิด_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
