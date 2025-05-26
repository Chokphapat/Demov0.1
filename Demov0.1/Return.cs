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
            this.Load += Return_Load;
            sqlite_conn = new SQLiteConnection("Data Source=DB.db;Version=3;");
            
            sqlite_conn.Open();


           

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
                    
                }
                
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
                //dataGridView1.Columns["id"].Width = 50;
               // dataGridView1.Columns["วัน_เดือน_ปี"].Width = 130;
                //dataGridView1.Columns["เวลา"].Width = 70;
                //dataGridView1.Columns["จำนวน"].Visible = false;

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
