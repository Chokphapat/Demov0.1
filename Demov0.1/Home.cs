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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            ShowFormInPanel(new Crud());
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
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
            ShowFormInPanel(new Crud());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Equipment());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Register());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
