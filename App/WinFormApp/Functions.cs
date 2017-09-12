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
        /// 清除事件绑定的函数  
        /// </summary>  
        /// <param name="objectHasEvents">拥有事件的实例</param>  
        /// <param name="eventName">事件名称</param>  
        private static void ClearAllEvents(object objectHasEvents, string eventName)
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
            // 先移除事件绑定的函数，防止重复绑定，叠加效果（重复截图）
            ClearAllEvents(w, "DocumentCompleted");
            // 绑定一个lambda表达式函数
            w.DocumentCompleted += (sender, e) => {
                w.DrawToBitmap(bitmap, rectangle);
                // 保存图片
                String Path = GetAssetsPath() + "/" + GetPicName();
                bitmap.Save(Path);
            };
        }
    }
}
