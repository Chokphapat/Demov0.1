using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demov0._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // สร้าง panel2 ชั่วคราวหากไม่มีใน Designer
            this.panel2 = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(500, 400),
                BackColor = Color.LightGray // เพิ่มสีพื้นหลังเพื่อดูชัดเจน
            };
            this.Controls.Add(panel2);

            // โหลดฟอร์ม Report
            //ShowFormInPanel(new Report());
        }

        private void ShowFormInPanel(Form form)
        {
            panel2.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panel2.Controls.Add(form);
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
