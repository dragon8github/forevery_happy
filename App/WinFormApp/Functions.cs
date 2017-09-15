using System;
using System.Drawing;
using System.IO;
using System.Reflection;
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

        public static string GetUUID(string type = "")
        {
            return Guid.NewGuid().ToString(type);
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
        /// 创建一个WebBrowser
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static WebBrowser CreateWebBrowser(string url = "", int width = 1024, int height = 768) {
            WebBrowser w = new WebBrowser() {
                Width = width,
                Height = height
            };
            try {
                w.Url = new Uri(url);
            }
            catch (Exception ex) {
                MessageBox.Show("URL格式有问题，请确保不为空并且包含 http:// 或者 https:// \r\n\r\n" + ex.Message);
            }
            return w;
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
            return GetUUID() + "." + ext;
        }

        /// <summary>  
        /// 清除事件绑定的函数  
        /// </summary>  
        /// <param name="objectHasEvents">拥有事件的实例</param>  
        /// <param name="eventName">事件名称</param>  
        public static void ClearAllEvents(object objectHasEvents, string eventName)
        {
            if (objectHasEvents == null) {
                return;
            }
            try {
                EventInfo[] events = objectHasEvents.GetType().GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (events == null || events.Length < 1) {
                    return;
                }
                for (int i = 0; i < events.Length; i++) {
                    EventInfo ei = events[i];
                    if (ei.Name == eventName) {
                        FieldInfo fi = ei.DeclaringType.GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fi != null) {
                            fi.SetValue(objectHasEvents, null);
                        }
                        break;
                    }
                }
            }
            catch {
            }
        }
    }
}
