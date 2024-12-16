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
    public partial class History : Form
    {
        private SQLiteConnection sqlite_conn;

        public History()
        {
            InitializeComponent();

            
            sqlite_conn = new SQLiteConnection("Data Source=your_database_v2.db;Version=3;");
            sqlite_conn.Open();
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
    }
}
