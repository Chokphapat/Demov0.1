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
        private SQLiteConnection equipment_conn;
        int index;
        public Return()
        {
            InitializeComponent();
            this.Load += Return_Load;
            sqlite_conn = new SQLiteConnection("Data Source=your_database_v5.db;Version=3;");
            equipment_conn = new SQLiteConnection("Data Source=DatabaseAll.db;Version=3;");
            sqlite_conn.Open();
            equipment_conn.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Messages (Id INTEGER PRIMARY KEY, ชื่ออุปกรณ์ TEXT, ชนิดอุปกรณ์ TEXT, ประวัติการยืมคืน TEXT, วัน เดือน ปี TEXT, เวลา TEXT, ชื่อผู้ใช้ TEXT, หมายเหตุ TEXT)";
            SQLiteCommand createTableCmd = new SQLiteCommand(createTableQuery, sqlite_conn);
            createTableCmd.ExecuteNonQuery();
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                index = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[index];

                //comboBox1.SelectedItem = row.Cells["ชื่ออุปกรณ์"].Value?.ToString();
                //comboBox2.Text = row.Cells[2].Value?.ToString();
                //combobox2.Text = row.Cells[3].Value?.ToString();
                //dateTimePicker1.Text = row.Cells[4].Value?.ToString();
                //richTextBox4.Text = row.Cells[5].Value?.ToString();
                //richTextBox5.Text = row.Cells[6].Value?.ToString();
                //richTextBox6.Text = row.Cells[7].Value?.ToString();
                //richTextBox1.Text = row.Cells[8].Value?.ToString();

                string value = row.Cells[5].Value?.ToString();

                if (!string.IsNullOrEmpty(value))
                {

                    string[] parts = value.Split(':');

                    // ตรวจสอบว่ามีข้อมูลเพียงพอ
                    if (parts.Length == 2)
                    {
                        textBox2.Text = parts[0]; // รับส่วนแรก เช่น 12
                        textBox3.Text = parts[1]; // รับส่วนหลัง เช่น 16
                    }
                    else
                    {
                        textBox2.Text = ""; // กรณีที่ไม่มีข้อมูลที่ต้องการ
                        textBox3.Text = "";
                    }
                }
                else
                {
                    textBox2.Text = ""; // กรณีค่าเป็น null หรือว่าง
                    textBox3.Text = "";
                }
            }
        
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
                dataGridView1.Columns["id"].Width = 50;
                dataGridView1.Columns["วัน_เดือน_ปี"].Width = 130;
                dataGridView1.Columns["เวลา"].Width = 70;
                dataGridView1.Columns["จำนวน"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        private void Return_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
