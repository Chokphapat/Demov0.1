using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Demov0._1
{
    public class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        // ฟังก์ชันสำหรับส่งข้อมูลไปยัง API
        public async Task SendDataToApiAsync(string text1, string text2, string text3, string text4)
        {
            // URL ของ API ที่ต้องการส่งข้อมูลไป
            var apiUrl = "https://localhost:7104/api/messages"; // ปรับ URL ตามที่ API ของคุณใช้

            // สร้างออบเจ็กต์ข้อมูลที่จะส่ง
            var data = new { Text1 = text1, Text2 = text2, Text3 = text3, Text4 = text4 };

            // แปลงข้อมูลเป็น JSON
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                // ส่ง POST Request ไปยัง API
                var response = await client.PostAsync(apiUrl, content);

                // ตรวจสอบสถานะการตอบสนอง
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data sent successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to send data. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
