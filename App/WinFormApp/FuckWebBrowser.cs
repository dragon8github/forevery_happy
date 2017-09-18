using FastVerCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinFormApp
{
    public class FuckWebBrowser
    {
        WebBrowser w;

        public FuckWebBrowser(WebBrowser w) {
            w.ObjectForScripting = this;
            w.ScrollBarsEnabled = false;
            this.w = w;
        }

        /// <summary>
        /// 登录
        /// 之所以setTimeout是因为验证码图片容器.geetest_panel_box 动画需要500秒才加载完成。
        /// 而轮训setInterval之所以设置1秒，也是有原因的。如果间隔太短很可能造成截图问题。
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(string username, string password)
        {
            ExecScript(@"     
                $('#username').val('" + username + @"'); 
                $('#password').val('" + password + @"'); 
                document.getElementById('loginsubmit').click(); 
                var s = setInterval(function() {
                    document.getElementById('loginsubmit').click(); 
                    if ($('.geetest_widget').is(':visible') && $('.geetest_item_img').length && $('.geetest_item_img')[0].complete) {  
                        clearInterval(s);
                        $('.geetest_panel_box').css({left:0,top:0,transform:'translate(0, 0)'});
                        setTimeout(function () {   
                           var path = window.external.CutPic();
                           var returnMess = window.external.SendImage(path);
                           var arr = returnMess.split('|');
                           for (var i = 0; i < arr.length - 2; i++) {
                               var _arr = arr[i].split(',');
                               window.alert(_arr[0]+'|'+_arr[1]);
                               //var ev = document.createEvent('HTMLEvents'); 
                               //ev.clientX = _arr[0];
                               //ev.clientY = _arr[1];
                               //ev.initEvent('click', false, true); 
                               //$('.geetest_item.geetest_big_item')[0].dispatchEvent(ev);
                           }    
                           window.external.CutPic();
                        }, 500);
                    }
                }, 100)
            ");
        }

        /// <summary>
        /// 调试js专用交互函数
        /// </summary>
        /// <param name="message"></param>
        public string Msg(string message)
        {
            MessageBox.Show(message, "Fuck You");
            return "123";
        }

        /// <summary>
        /// 发送图片给打码平台
        /// </summary>
        /// <param name="path"></param>
        public string SendImage(string path)
        {
            string username = "dragon8dama";
            string pwd = "202063sbmP";
            string softKey = "dragon8dama";
            return VerCode.RecYZM_A_2(path, 1303, 2, 6, username, pwd, softKey);
        }

        /// <summary>
        /// 创建一个WebBrowser
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static WebBrowser CreateWebBrowser(string url = "", int width = 1024, int height = 768)
        {
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
        /// 执行js代码
        /// </summary>
        /// <param name="jsCode"></param>
        public void ExecScript(string jsCode)
        {
            ExecActionWhenWebBrowserDocumentCompleted(_w => {
                HtmlElement script = _w.Document.CreateElement("script");
                script.SetAttribute("type", "text/javascript");
                script.SetAttribute("text", jsCode);
                HtmlElement head = _w.Document.Body.AppendChild(script);
            });
        }

        /// <summary>
        /// 截取WebBrowser网页中的图片
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public string CutPic(int x = 0, int y = 0, int width = 315, int height = 350)
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
            String Path = Functions.GetAssetsPath() + "/" + Functions.GetPicName();

            ExecActionWhenWebBrowserDocumentCompleted(_w => {
                _w.DrawToBitmap(bitmap, rectangle);
                Bitmap _bitmap = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(_bitmap);
                g.DrawImage(bitmap, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                // 保存图片                
                _bitmap.Save(Path);
            });

            return Path;
        }

        /// <summary>
        /// 执行函数当WebBrowser对象加载完毕时
        /// </summary>
        /// <param name="action"></param>
        private void ExecActionWhenWebBrowserDocumentCompleted(Action<WebBrowser> action)
        {
            if (this.w.ReadyState == WebBrowserReadyState.Complete) {
                action(this.w);
            }
            else {
                // 先移除事件绑定的函数，防止重复绑定，叠加重复效果
                Functions.ClearAllEvents(this.w, "DocumentCompleted");
                // 绑定一个lambda表达式函数
                w.DocumentCompleted += (sender, e) => {
                    action(this.w);
                };
            }
        }
    }
}
