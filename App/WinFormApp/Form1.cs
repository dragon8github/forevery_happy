using System;
using System.Windows.Forms;
using FastVerCode;
using System.Diagnostics;
using Microsoft.Win32;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        public enum IE { IE7 = 7000, IE8 = 8000, IE9 = 9999, IE10 = 10001, IE11 = 11001 }

        public Form1()
        {
            /**
            * 解决WebBrowser默认的IE版本太低（IE7）的问题，只需要改变枚举即可
            * https://dotblogs.com.tw/larrynung/archive/2012/10/15/77505.aspx
            * https://msdn.microsoft.com/en-us/library/ee330730(v=vs.85).aspx#browser_emulation
            * InitializeComponent();
            */
            var appName = Process.GetCurrentProcess().MainModule.ModuleName;
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", appName, IE.IE11, RegistryValueKind.DWord);
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string username = "dragon8dama";
            string pwd = "202063sbmP";
            string softKey = "dragon8dama";

            //上传字节集验证码
            //byte[] bytes = { };
            //string returnMess = VerCode.RecByte_A(bytes, bytes.Length, username, pwd, softKey);

            //上传本地验证码(地址，验证码类型，最小验证码字数，最大验证码字数，用户名，密码，推荐人)
            string returnMess = VerCode.RecYZM_A_2("c:\\getimage.png", 1303, 2, 6, username, pwd, softKey);
            //string returnMess = VerCode.RecYZM_A("c:\\getimage.png", username, pwd, softKey);
            Console.WriteLine(returnMess);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置第二个tab为默认的显示项
            this.tabControl1.SelectedIndex = 1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try {
                this.webBrowser1.Url = new Uri(this.textBox11.Text);
            }
            catch (Exception ex) {
                MessageBox.Show("URL格式有问题，请确保不为空并且包含 http:// 或者 https:// \r\n\r\n" + ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try {
                WebBrowser wb = new WebBrowser {
                    Url = new Uri(this.textBox11.Text),
                    Width = 1024,
                    Height = 768,
                };
                Functions.CutPic(wb);
            }
            catch (Exception ex) {
                MessageBox.Show("URL格式有问题，请确保不为空并且包含 http:// 或者 https:// \r\n\r\n" + ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try {
                this.webBrowser1.Url = new Uri(this.textBox11.Text);
                this.webBrowser1.DocumentCompleted += (_sender, _e) => {
                    this.webBrowser1.Document.GetElementById("blog_nav_newpost").InvokeMember("click");
                };
            }
            catch (Exception ex) {
                MessageBox.Show("URL格式有问题，请确保不为空并且包含 http:// 或者 https:// \r\n\r\n" + ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.webBrowser1.ScriptErrorsSuppressed = true;
            Functions.Login(this.webBrowser1);
            Functions.CutPic(this.webBrowser1, 1920, 1080);
        }
    }
}
