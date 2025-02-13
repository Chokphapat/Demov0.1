using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Demov0._1
{
    public partial class Report : Form
    {
        private SQLiteConnection sqlite_conn;
        private string sourceForm;
        private int totalPages;
        private int currentPage;
        private int pageSize;
        private string searchQuery;

        public Report(SQLiteConnection connection, string formSource, int totalPages, int currentPage, int pageSize, string searchQuery = "")
        {
            InitializeComponent();
            sqlite_conn = connection;
            sourceForm = formSource;
            this.totalPages = totalPages;
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.searchQuery = searchQuery;
            LoadData(searchQuery);
            // เรียกใช้ฟังก์ชันเพื่อแสดงค่าที่เกี่ยวข้อง
            DisplayReportDetails();
        }
        

        private void DisplayReportDetails()
        {
            // ตัวอย่างการแสดงผล
           
            labelCurrentPage.Text = $"หน้า {currentPage}";
            labeltotalPages.Text = $"จาก {totalPages}";
            //labelPageSize.Text = $" {pageSize}";
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            // แสดงวันที่ปัจจุบัน
            currentPage = 1;
            totalPages = 1;
            if (currentPage <= 0) currentPage = 1;
            if (totalPages <= 0) totalPages = 1;

            label3.Text = DateTime.Now.ToString("dd/MM/yyyy");

            // ตั้งค่าหัวข้อรายงานและโหลดข้อมูล
            if (sourceForm == "CRUD")
            {
                label2.Text = "รายงานบันทึกการยืม-คืน";
                LoadData($"SELECT * FROM Messages WHERE ชื่ออุปกรณ์ LIKE '%{searchQuery}%' LIMIT {pageSize} OFFSET {(currentPage - 1) * pageSize}");
            }
            else if (sourceForm == "Equipment")
            {
                label2.Text = "รายงานบันทึกอุปกรณ์";
                LoadData($"SELECT * FROM Equipment WHERE ชื่ออุปกรณ์ LIKE '%{searchQuery}%' LIMIT {pageSize} OFFSET {(currentPage - 1) * pageSize}");
            }
            else
            {
                label2.Text = "รายงานทั้งหมด";
                LoadData($"SELECT * FROM Messages LIMIT {pageSize} OFFSET {(currentPage - 1) * pageSize}");
            }
        }
        private void LoadData(string searchQuery)
        {
            try
            {
                string query = $"SELECT * FROM Messages WHERE ชื่ออุปกรณ์ LIKE '%{searchQuery}%' LIMIT {pageSize} OFFSET {(currentPage - 1) * pageSize}";

                SQLiteCommand cmd = new SQLiteCommand(query, sqlite_conn);
                cmd.Parameters.AddWithValue("@SearchQuery", searchQuery);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("ไม่พบข้อมูลที่ตรงกับการค้นหา", "ข้อมูลว่าง", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ตรวจสอบว่ามีข้อมูลหรือไม่
                if (dataTable.Rows.Count == 0)
                {
                    //MessageBox.Show("ไม่พบข้อมูลที่ต้องการ", "ข้อมูลว่าง", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // สร้าง DataGridView และตั้งค่าการแสดงผล
                DataGridView dataGridView = new DataGridView
                {
                    DataSource = dataTable,
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToOrderColumns = true // อนุญาตให้ผู้ใช้จัดเรียงคอลัมน์

                };

                // กำหนดค่าคอลัมน์ (กำหนดขนาดและซ่อนบางคอลัมน์ตามความต้องการ)
                if (dataGridView.Columns.Contains("id"))
                    dataGridView.Columns["id"].Width = 50;

                if (dataGridView.Columns.Contains("วัน_เดือน_ปี"))
                    dataGridView.Columns["วัน_เดือน_ปี"].Width = 130;

                if (dataGridView.Columns.Contains("เวลา"))
                    dataGridView.Columns["เวลา"].Width = 70;

                if (dataGridView.Columns.Contains("จำนวน"))
                    dataGridView.Columns["จำนวน"].Visible = false;

                // ล้าง Panel และเพิ่ม DataGridView เข้าไป
                panel1.Controls.Clear();
                panel1.Controls.Add(dataGridView);
                dataGridView.RowTemplate.Height = 30; // กำหนดความสูงของแถวให้เหมาะสม

            }
            catch (SQLiteException )
            {
                //MessageBox.Show($"เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล: {sqlEx.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //แสดงตารางปัจจุบันที่ผู้ใช้งานได้เลือกไว้ ถ้าผู้คนหา เช่น พิมพ์ยา ตารางของเเต่ละหน้าจะขึ้นว่ายา โดยพาเเนลนี้จะนำตารางที่ผู้ใช้พิมค้นหามาอยู่ในพาเเนล
            //หรือก็คือเเสดงหน้าปัจจุบันของตารางสรุปเป็นตารางรายงาน
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //เเสดงการรายงาน เช่นถ้าคลิกมาจากหน้า crud ให้เเสดงบันทึกการคืมคืน ถ้าคลิกพิมรายงานมาจาก หน้า Equiment ให้เป็น บันทึกอุปกรณ์
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //เป็นวันที่ปัจจุบัน
        }

        private void labelPageSize_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // จับภาพหน้าฟอร์ม
                Bitmap formImage = CaptureForm();

                // เลือกที่บันทึกไฟล์ PDF
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF File|*.pdf",
                    Title = "Save Report as PDF",
                    FileName = "FormCapture.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // แปลงภาพเป็น PDF
                    SaveImageToPdf(formImage, saveFileDialog.FileName);

                    MessageBox.Show("บันทึก PDF สำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap CaptureForm()
        {
            // สร้าง Bitmap สำหรับหน้าฟอร์ม
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, this.Width, this.Height)); // ใช้ System.Drawing.Rectangle
            return bitmap;
        }


        private void DrawToBitmap(Bitmap bitmap, iTextSharp.text.Rectangle rectangle)
        {
            throw new NotImplementedException();
        }


        private void SaveImageToPdf(Bitmap image, string outputPath)
        {
            using (FileStream stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // ตั้งค่าเอกสาร PDF
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                // แปลง Bitmap เป็น iTextSharp Image
                using (MemoryStream imageStream = new MemoryStream())
                {
                    image.Save(imageStream, ImageFormat.Png);
                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imageStream.ToArray());
                    pdfImage.ScaleToFit(document.PageSize.Width, document.PageSize.Height); // ปรับขนาดภาพให้เหมาะสม
                    pdfImage.Alignment = Element.ALIGN_CENTER;

                    // เพิ่มภาพใน PDF
                    document.Add(pdfImage);
                }

                document.Close();
                writer.Close();
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
