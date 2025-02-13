using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Demov0._1
{
    internal class sty
    {
        public class RoundedTextBox : TextBox
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, 20, 20, 180, 90); // มุมซ้ายบน
                path.AddArc(this.Width - 20, 0, 20, 20, 270, 90); // มุมขวาบน
                path.AddArc(this.Width - 20, this.Height - 20, 20, 20, 0, 90); // มุมขวาล่าง
                path.AddArc(0, this.Height - 20, 20, 20, 90, 90); // มุมซ้ายล่าง
                path.CloseFigure();

                this.Region = new Region(path); // กำหนด Region เป็นรูปทรงโค้งมน
            }
            

        }

        public class RoundedRichTextBox : RichTextBox
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr CreateRoundRectRgn(
                int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                IntPtr region = CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20); // ปรับมุมโค้งมน
                SetWindowRgn(this.Handle, region, true);
            }
        }
        public class RoundedPanel : Panel
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, 20, 20, 180, 90); // มุมซ้ายบน
                path.AddArc(this.Width - 20, 0, 20, 20, 270, 90); // มุมขวาบน
                path.AddArc(this.Width - 20, this.Height - 20, 20, 20, 0, 90); // มุมขวาล่าง
                path.AddArc(0, this.Height - 20, 20, 20, 90, 90); // มุมซ้ายล่าง
                path.CloseFigure();

                this.Region = new Region(path); // กำหนด Region เป็นรูปทรงโค้งมน
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillPath(Brushes.LightBlue, path); // เติมสี
            }
        }


    }
    }
