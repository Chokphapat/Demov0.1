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
    

    public partial class Register : Form
    {
       
        private SQLiteConnection sqlite_conn;

        public Register()
        {
            InitializeComponent();
            sqlite_conn = new SQLiteConnection("Data Source=Admin.db;Version=3;");
            sqlite_conn.Open();

            string createTableQuery = @"CREATE TABLE IF NOT EXISTS User (
                                        UserId INTEGER PRIMARY KEY,
                                        Username TEXT NOT NULL UNIQUE,
                                        Password TEXT NOT NULL,
                                        Usertype TEXT NOT NULL)";
            SQLiteCommand createTableCmd = new SQLiteCommand(createTableQuery, sqlite_conn);
            createTableCmd.ExecuteNonQuery();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = richTextBox1.Text.Trim();
            string password = richTextBox2.Text.Trim();
            string usertype = "User";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("โปรดกรอกข้อมูลให้ครบถ้วน");
                return;
            }

            try
            {
                string checkQuery = "SELECT COUNT(*) FROM User WHERE Username = @Username";
                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, sqlite_conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        MessageBox.Show("ชื่อผู้ใช้งานนี้ถูกใช้แล้ว");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO User (Username, Password, Usertype) VALUES (@Username, @Password, @Usertype)";
                using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, sqlite_conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Usertype", usertype);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("สมัครสมาชิกสำเร็จ!");

                    richTextBox1.Clear();
                    richTextBox2.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Username
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            //Password
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
