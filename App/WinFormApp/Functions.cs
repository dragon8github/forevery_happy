using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WinFormApp
{
    class Functions
    {
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp() {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        ///  查看该路径是否存在，如果不存在，则创建这个文件夹，如果存在则什么都不做
        /// </summary>
        /// <param name="Path">要查询的路径</param>
        /// <returns></returns>
        public static string MakePath(string Path) {
            if (!Directory.Exists(Path)) {
                Directory.CreateDirectory(Path);
            }
            return Path;
        }

        /// <summary>
        /// 是否存在资源路径assets，如果不存在则创建，返回路径
        /// </summary>
        /// <returns></returns>
        public static string GetAssetsPath() {
            string Path = Directory.GetCurrentDirectory() + "/assets";
            return MakePath(Path);
        }

        /// <summary>
        /// 生成并且返回图片的名字，其实就是时间戳，默认为jpg
        /// </summary>
        /// <returns></returns>
        public static string GetPicName(string ext = "jpg") {
            return GetTimeStamp() + "." + ext;
        }

        /// <summary>
        /// 截取WebBrowser网页中的图片
        /// </summary>
        /// <param name="webBrowser">WebBrowser实例对象</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public static void CutPic(WebBrowser w, int width = 322, int height = 411)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            w.DrawToBitmap(bitmap, rectangle);

            // 保存图片
            String Path = GetAssetsPath() + "/" + GetPicName();
            bitmap.Save(Path);  
        }
    }
}
