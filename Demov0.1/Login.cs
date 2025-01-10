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
    public partial class Login : Form
    {
        private SQLiteConnection sqlite_conn;
        public Login()
        {
            InitializeComponent();
            sqlite_conn = new SQLiteConnection("Data Source=Admin.db;Version=3;");
            sqlite_conn.Open();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {
            string username = textBox1.Text; // ดึงข้อมูลจาก TextBox สำหรับชื่อผู้ใช้
            string password = textBox2.Text; // ดึงข้อมูลจาก TextBox สำหรับรหัสผ่าน

            if (CheckCredentials(username, password))
            {
                // ซ่อนฟอร์มปัจจุบัน
                this.Hide();

                // แสดงฟอร์ม Home
                Home homeForm = new Home();
                homeForm.ShowDialog();

                // เมื่อปิด Home ฟอร์ม ให้แสดงฟอร์มปัจจุบันกลับมา
                this.Show();
            }
            else
            {
                // แสดงข้อความแจ้งเตือนเมื่อชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง
                MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private bool CheckCredentials(string username, string password)
        {
            try
            {
                // SQL Query สำหรับดึงข้อมูลผู้ใช้ที่ตรงกับ Username และ Password
                string query = "SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password";
                using (SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn))
                {
                    // เพิ่มพารามิเตอร์เพื่อป้องกัน SQL Injection
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    // รันคำสั่ง SQL และตรวจสอบผลลัพธ์
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0; // ถ้ามีข้อมูลที่ตรงกันจะคืนค่า true
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
