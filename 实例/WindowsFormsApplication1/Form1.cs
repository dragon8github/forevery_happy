using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FastVerCode;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = "dragon8dama";
            string pwd = "202063sbmP";
            string softKey = "dragon8dama";
            //获取用户信息 
            string userInfo = VerCode.GetUserInfo(username, pwd);
            byte[] bytes = { };
            //上传字节集验证码
            //string returnMess = VerCode.RecByte_A(bytes, bytes.Length, username, pwd, softKey);
            //上传本地验证码(地址，)
            string returnMess = VerCode.RecYZM_A_2("c:\\getimage.jpg", 1303, 3, 6, username, pwd, softKey);
            //string returnMess = VerCode.RecYZM_A("c:\\getimage.jpg", username, pwd, softKey);
            Console.WriteLine(returnMess);

        }
    }
}
