﻿using System;
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

        /// <summary>
        /// 执行函数当WebBrowser对象加载完毕时
        /// MD，这个Action硬是要填入一个参数，我又不知道什么好，觉得干脆继续返回一个一模一样的WebBrowser吧，没准以后需要对它进行变化。也挺好的
        /// </summary>
        /// <param name="action">待执行的函数</param>
        /// <param name="w">WebBrowser对象</param>
        public static void ExecActionWhenWebBrowserDocumentCompleted(Action<WebBrowser> action, WebBrowser w)
        {
            if (w.ReadyState == WebBrowserReadyState.Complete) {
                action(w);
            }
            else {
                // 先移除事件绑定的函数，防止重复绑定，叠加重复效果
                ClearAllEvents(w, "DocumentCompleted");
                // 绑定一个lambda表达式函数
                w.DocumentCompleted += (sender, e) => {
                    action(w);
                };
            }
        }

        /// <summary>
        /// 截取WebBrowser网页中的图片
        /// 默认的宽高就是验证码图片的size
        /// </summary>
        /// <param name="webBrowser">WebBrowser实例对象</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public static void CutPic(WebBrowser w, int x = 0, int y = 0, int width = 315, int height = 350)
        {
            // 《问题》：由于 WebBrowser.DrawToBitmap 截图时,偏移的参数设置居然是内边距！这完全不是我想要的。
            // 《解决方法》：二次截图
            // 1、先对WebBrowser进行满屏截图，并且生成一个Bitmap。
            // 2、然后再生成一个空白的Bitmap，宽高设置当然是入参的 width 和 height。
            // 3、使用Graphics第一个全屏图片Bitmap（Bitmap类型可以当Image类型使用）进行二次截图，再把结果放入空白的Bitmap中
            // 这时候发现偏移参数正常了。
            int full_width = w.Width;
            int full_height = w.Height;
            Bitmap bitmap = new Bitmap(full_width, full_height);
            Rectangle rectangle = new Rectangle(0, 0, full_width, full_height);

            ExecActionWhenWebBrowserDocumentCompleted(_w => {
                w.DrawToBitmap(bitmap, rectangle);
                Bitmap _bitmap = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(_bitmap);
                g.DrawImage(bitmap, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                // 保存图片
                String Path = GetAssetsPath() + "/" + GetPicName();
                _bitmap.Save(Path);
            }, w);            
        }

        /// <summary>
        /// 执行js代码
        /// </summary>
        /// <param name="jsCode"></param>
        public static void ExecScript(WebBrowser w, string jsCode)
        {
            ExecActionWhenWebBrowserDocumentCompleted(_w => {
                HtmlElement script = w.Document.CreateElement("script");
                script.SetAttribute("type", "text/javascript");
                script.SetAttribute("text", jsCode);
                HtmlElement head = w.Document.Body.AppendChild(script);    
            }, w);
        }

        /// <summary>
        /// 登录
        /// 之所以setTimeout是因为验证码图片容器.geetest_panel_box 动画需要500秒才加载完成。
        /// 而轮训setInterval之所以设置1秒，也是有原因的。如果间隔太短很可能造成截图问题。
        /// </summary>
        /// <param name="w"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Login(WebBrowser w, string username, string password)
        {
            ExecScript(w, @"
                $('#username').val('" + username + @"'); 
                $('#password').val('" + password + @"'); 
                document.getElementById('loginsubmit').click();               
                var s = setInterval(function() {
                    if ($('.geetest_widget').is(':visible') && $('.geetest_item_img').length && $('.geetest_item_img')[0].complete) {  
                        clearInterval(s);
                        var g = $('.geetest_widget')[0].getBoundingClientRect();
                        setTimeout(function () {                            
                            window.external.GetPic(g.left, g.top);
                        }, 500);                        
                    }
                }, 800)
            ");
        }
    }
}
