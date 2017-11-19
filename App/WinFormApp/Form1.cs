using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Windows.Forms;
using WinFormApp.Models;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        Fuck _fuck;
        HttpHelper _http;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置默认的显示项
            this.tabControl1.SelectedIndex = 3;

            _fuck = new Fuck();

            _http = new HttpHelper();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(_fuck.GetGeetet());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Jiyan jy = _fuck.GetJiyan();
            MessageBox.Show("validate:" + jy.validate + "\r\n challenge:" + jy.challenge + "\r\n status:" + jy.status);
        }

        /// <summary>
        /// 尝试登录并且获取cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            string cookie = _fuck.Login("13713332652", "202063sb");

            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/personorders/myorder.html",
                Method = "GET",
                Timeout = 10000,
                Cookie = cookie,
                ResultType = ResultType.String,
                Allowautoredirect = true
            };
            HttpResult result = _http.GetHtml(item);
            string html = result.Html;
            MessageBox.Show(html);
        }
    }
}