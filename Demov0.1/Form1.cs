using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Drawing.Imaging;

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
            //กดเเล้วพิมพ์
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static void ExportToPdf(DataGridView dataGridView, string filePath)
        {
            Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

            // เพิ่มหัวข้อตาราง
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                cell.BackgroundColor = new BaseColor(240, 240, 240);
                pdfTable.AddCell(cell);
            }

            // เพิ่มข้อมูล
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdfTable.AddCell(cell.Value?.ToString() ?? string.Empty);
                }
            }

            document.Add(pdfTable);
            document.Close();
        }

        public static void ExportToExcel(DataGridView dataGridView, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // เพิ่มหัวข้อตาราง
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView.Columns[i].HeaderText;
                }

                // เพิ่มข้อมูล
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // บันทึกไฟล์
                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);
            }
        }

        public static void ExportToImage(DataGridView dataGridView, string filePath, ImageFormat format)
        {
            // ปรับขนาด DataGridView เพื่อให้แสดงข้อมูลทั้งหมด
            dataGridView.ScrollBars = ScrollBars.None;
            dataGridView.Height = dataGridView.RowCount * dataGridView.RowTemplate.Height + dataGridView.ColumnHeadersHeight;

            // สร้าง bitmap จาก DataGridView
            Bitmap bitmap = new Bitmap(dataGridView.Width, dataGridView.Height);
            dataGridView.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView.Width, dataGridView.Height));

            // บันทึกเป็นรูปภาพ
            bitmap.Save(filePath, format);

            // คืนค่าการแสดง ScrollBars
            dataGridView.ScrollBars = ScrollBars.Both;
        }
    }
}
