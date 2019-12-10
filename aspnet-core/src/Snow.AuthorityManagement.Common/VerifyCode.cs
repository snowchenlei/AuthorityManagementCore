using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Common
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="len">验证码长度</param>
        /// <param name="strCode">验证码</param>
        /// <returns>验证码图片</returns>
        public static byte[] Create(int len, out string strCode)
        {
            MemoryStream stream = new MemoryStream();
            byte[] buffer = null;
            //噪线 噪点
            Color[] colors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.DarkBlue };
            //验证码字体
            string[] fonts = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //验证码内容
            char[] charactars = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'S', 'Y', 'Z' };
            //生成验证码字符串
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < len; i++)
            {
                char codeChar = charactars[r.Next(0, charactars.Length)];
                sb.Append(codeChar);
            }
            strCode = sb.ToString();
            using (Bitmap bitmap = new Bitmap(len * 25, 34))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.WhiteSmoke);
                    for (int i = 0; i < 10; i++)
                    {
                        int x1 = r.Next(100);
                        int y1 = r.Next(40);
                        int x2 = r.Next(100);
                        int y2 = r.Next(40);
                        Color color = colors[r.Next(colors.Length)];
                        g.DrawLine(new Pen(color), x1, y1, x2, y2);
                    }
                    //将字符串画入图片
                    for (int i = 0; i < strCode.Length; i++)
                    {
                        string font = fonts[r.Next(fonts.Length)];
                        Font fnt = new Font(font, 18);
                        Color color = colors[r.Next(colors.Length)];
                        g.DrawString(strCode[i].ToString(), fnt, new SolidBrush(color), (float)i * 20 + 8, (float)8);
                    }
                    //画噪点
                    for (int i = 0; i < 100; i++)
                    {
                        int x = r.Next(bitmap.Width);
                        int y = r.Next(bitmap.Height);
                        Color color = colors[r.Next(colors.Length)];
                        bitmap.SetPixel(x, y, color);
                    }
                }
                bitmap.Save(stream, ImageFormat.Jpeg);
                buffer = stream.ToArray();
            }
            return buffer;
        }
    }
}
