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

using System.Drawing.Imaging;

namespace Demov0._1
{
    public partial class Form1 : Form
    {
        private DataGridView dataGridView1;
        /*public Form1()
        {
            InitializeComponent();

            // สร้าง panel2 ชั่วคราวหากไม่มีใน Designer
            this.panel2 = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(500, 400),
                BackColor = Color.LightGray
            };
            this.Controls.Add(panel2);

            // สร้าง DataGridView สำหรับทดสอบ
            dataGridView1 = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(460, 360),
                Visible = false // ซ่อนไว้ก่อน
            };
            panel2.Controls.Add(dataGridView1);

            // เพิ่มข้อมูลตัวอย่างใน DataGridView
            InitializeSampleData();
        }
        private void InitializeSampleData()
        {
            // สร้าง DataTable สำหรับข้อมูลตัวอย่าง
            DataTable dt = new DataTable();
            dt.Columns.Add("ลำดับ", typeof(int));
            dt.Columns.Add("ชื่ออุปกรณ์", typeof(string));
            dt.Columns.Add("จำนวน", typeof(int));
            dt.Columns.Add("วันที่ยืม", typeof(string));

            // เพิ่มข้อมูลตัวอย่าง
            dt.Rows.Add(1, "คอมพิวเตอร์", 5, "2023-01-01");
            dt.Rows.Add(2, "โปรเจคเตอร์", 3, "2023-01-02");
            dt.Rows.Add(3, "เครื่องพิมพ์", 2, "2023-01-03");

            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // เรียกใช้ฟังก์ชันพิมพ์
            ExportData();
        }

        private void ExportData()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF File|*.pdf|Excel File|*.xlsx|PNG Image|*.png|JPEG Image|*.jpg";
                saveFileDialog.Title = "บันทึกไฟล์รายงาน";
                saveFileDialog.FileName = "รายงานอุปกรณ์_" + DateTime.Now.ToString("yyyyMMdd");

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        string extension = Path.GetExtension(filePath).ToLower();

                        switch (extension)
                        {
                            case ".pdf":
                                ExportToPdf(dataGridView1, filePath);
                                break;
                            case ".xlsx":
                                ExportToExcel(dataGridView1, filePath);
                                break;
                            case ".png":
                                ExportToImage(dataGridView1, filePath, ImageFormat.Png);
                                break;
                            case ".jpg":
                                ExportToImage(dataGridView1, filePath, ImageFormat.Jpeg);
                                break;
                            default:
                                MessageBox.Show("รูปแบบไฟล์ไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                        }

                        MessageBox.Show("บันทึกไฟล์เรียบร้อยแล้วที่: " + filePath, "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void ExportToPdf(DataGridView dataGridView, string filePath)
        {
            // ตั้งค่าฟอนต์สำหรับภาษาไทย
            string thaiFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "THSarabun.ttf");
            BaseFont thaiFont;

            if (File.Exists(thaiFontPath))
            {
                thaiFont = BaseFont.CreateFont(thaiFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }
            else
            {
                // หากไม่พบฟอนต์ไทย ใช้ฟอนต์มาตรฐาน
                thaiFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            }

            Font font = new Font(thaiFont, 12);

            Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // เพิ่มหัวข้อรายงาน
            Paragraph header = new Paragraph("รายงานข้อมูลอุปกรณ์", new Font(thaiFont, 16, Font.BOLD));
            header.Alignment = Element.ALIGN_CENTER;
            document.Add(header);

            // เพิ่มวันที่พิมพ์
            Paragraph printDate = new Paragraph("พิมพ์เมื่อ: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), font);
            printDate.Alignment = Element.ALIGN_RIGHT;
            document.Add(printDate);

            // เพิ่มช่องว่าง
            document.Add(new Paragraph(" "));

            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

            // เพิ่มหัวข้อตาราง
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, font));
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                pdfTable.AddCell(cell);
            }

            // เพิ่มข้อมูล
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow) // หลีกเลี่ยงการเพิ่มแถวใหม่ที่ว่างเปล่า
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        pdfTable.AddCell(new Phrase(cell.Value?.ToString() ?? string.Empty, font));
                    }
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
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("รายงานอุปกรณ์");

                // เพิ่มหัวข้อรายงาน
                worksheet.Cells[1, 1].Value = "รายงานข้อมูลอุปกรณ์";
                worksheet.Cells[1, 1, 1, dataGridView.Columns.Count].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // เพิ่มวันที่พิมพ์
                worksheet.Cells[2, 1].Value = "พิมพ์เมื่อ: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[2, 1, 2, dataGridView.Columns.Count].Merge = true;
                worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                // เพิ่มหัวข้อตาราง
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[4, i + 1].Value = dataGridView.Columns[i].HeaderText;
                    worksheet.Cells[4, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[4, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[4, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // เพิ่มข้อมูล
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    if (!dataGridView.Rows[i].IsNewRow)
                    {
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 5, j + 1].Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                        }
                    }
                }

                // ปรับความกว้างของคอลัมน์ให้พอดี
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // บันทึกไฟล์
                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);
            }
        }

        public static void ExportToImage(DataGridView dataGridView, string filePath, ImageFormat format)
        {
            // เก็บการตั้งค่าเดิม
            ScrollBars originalScrollBars = dataGridView.ScrollBars;
            int originalHeight = dataGridView.Height;

            try
            {
                // ปรับขนาด DataGridView เพื่อให้แสดงข้อมูลทั้งหมด
                dataGridView.ScrollBars = ScrollBars.None;
                dataGridView.Height = dataGridView.RowCount * dataGridView.RowTemplate.Height + dataGridView.ColumnHeadersHeight;

                // สร้าง bitmap จาก DataGridView
                using (Bitmap bitmap = new Bitmap(dataGridView.Width, dataGridView.Height))
                {
                    dataGridView.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView.Width, dataGridView.Height));
                    bitmap.Save(filePath, format);
                }
            }
            finally
            {
                // คืนค่าการตั้งค่าเดิม
                dataGridView.ScrollBars = originalScrollBars;
                dataGridView.Height = originalHeight;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // โค้ดเมื่อฟอร์มโหลด (ถ้ามี)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }*/
    }
}
