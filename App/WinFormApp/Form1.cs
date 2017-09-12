using System;
using System.Windows.Forms;
using FastVerCode;
namespace WinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
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
                Functions.CutPic(this.webBrowser1);
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
    }
}
