using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
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
    }
}
