using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Policy;

namespace lib_captcha
{
    public class CaptchaGenerator
    {
        private static readonly Random rand = new Random();

        public Bitmap GenerateCaptchaImage(out string captchaText)
        {
            // 1. Tạo chuỗi ngẫu nhiên từ 5-7 kí tự (bỏ qua i, l, o, 0 theo yêu cầu trước)
            string chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            int length = rand.Next(4, 7);
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[rand.Next(chars.Length)];
            }
            captchaText = new string(stringChars);

            int width = 450;
            int height = 200;
            Bitmap bitmap = new Bitmap(width, height);

            string[] fontFamilies = { "Arial", "Verdana", "Times New Roman", "Courier New", "Comic Sans MS", "Georgia", "Impact", "Trebuchet MS", "Tahoma", "Lucida Console" };

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                

                // 4. Vẽ từng kí tự với khoảng cách động dựa trên chiều rộng thực tế
                float startX = 30f;
                float baseY = height / 2f;

                foreach (char c in captchaText)
                {
                    string fontName = fontFamilies[rand.Next(fontFamilies.Length)];
                    int fontSize = rand.Next(66, 77); // Size đa dạng
                    float angle = rand.Next(-30, 31);   // Góc nghiêng từ -10 đến 30 độ
                    float offsetY = rand.Next(-20, 21); // Lên xuống ngẫu nhiên trong khoảng -40 đến 40px

                    using (Font font = new Font(fontName, fontSize, FontStyle.Bold))
                    {
                        // Đo kích thước thực tế của ký tự khi render với font và size hiện tại
                        SizeF charSize = g.MeasureString(c.ToString(), font);

                        GraphicsState state = g.Save();

                        g.TranslateTransform(startX, baseY + offsetY);
                        g.RotateTransform(angle);

                        // Màu chữ ngẫu nhiên (tông tối để dễ nhìn)
                        using (Brush brush = new SolidBrush(Color.FromArgb(rand.Next(0, 90), rand.Next(0, 90), rand.Next(0, 90))))
                        {
                            // Vẽ lệch tâm một chút để phép quay quanh tâm hợp lý
                            g.DrawString(c.ToString(), font, brush, -charSize.Width / 2f, -charSize.Height / 2f);
                        }

                        g.Restore(state);

                        // startX dịch chuyển một lượng bằng ~90% đến 95% độ rộng kí tự để chúng dính sát vào nhau
                        float overlapFactor = 0.6f;
                        startX += charSize.Width * overlapFactor;
                    }

                    // 2. Thêm nhiễu hạt
                    for (int i = 0; i < 1500; i++)
                    {
                        int x = rand.Next(width);
                        int y = rand.Next(height);
                        bitmap.SetPixel(x, y, Color.FromArgb(rand.Next(100, 200), rand.Next(100, 200), rand.Next(100, 200)));
                    }

                    // 3. Thêm đường thẳng và đường tròn ngẫu nhiên
                    using (Pen noisePen = new Pen(Color.Gray, 1))
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            g.DrawLine(noisePen, rand.Next(width), rand.Next(height), rand.Next(width), rand.Next(height));
                        }
                        for (int i = 0; i < 20; i++)
                        {
                            int rx = rand.Next(width);
                            int ry = rand.Next(height);
                            int rw = rand.Next(20, 80);
                            g.DrawEllipse(noisePen, rx, ry, rw, rw);
                        }
                    }
                }
            }

            return bitmap;
        }

        public static string BitmapToBase64Jpg(Bitmap bitmap)
        {
            if (bitmap == null)
                return string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                // Lưu Bitmap vào MemoryStream dưới dạng định dạng JPEG
                bitmap.Save(ms, ImageFormat.Jpeg);

                // Chuyển mảng byte của stream thành chuỗi Base64
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}