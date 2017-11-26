using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Windows.Forms;
using WinFormApp.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

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

            // 测试是否登录成功
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

        private void button15_Click(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string cookie = _fuck.Login("13713332652", "202063sb");
            string url = textBox11.Text;
            HttpItem item = new HttpItem() {
                URL = url,
                Method = "GET",
                Cookie = cookie,
            };
            HttpResult result = _http.GetHtml(item);
            string html = result.Html;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);
            string xpath = "//*[@id=\"Jprice\"]/li";
            var childNodes = htmlDoc.DocumentNode.SelectNodes(xpath);
            string str = "";
            foreach (var node in childNodes) {
                if (node.NodeType == HtmlNodeType.Element) {
                    // MessageBox.Show(node.Attributes["n"].Value);
                    // MessageBox.Show(node.Attributes["title"].Value);
                    str += node.Attributes["title"].Value + ":" + node.Attributes["n"].Value + "张;";
                }
            }
            textBox5.Text = str;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string cookie = _fuck.Login("13713332652", "202063sb");
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/deliveryAddress/deliveryaddress",
                Method = "GET",
                Cookie = cookie,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            HttpResult result = _http.GetHtml(item);
            string html = result.Html;

            // 获取addressid
            Regex reg = new Regex(@"updateAddress\((.+?)\)");
            MatchCollection match = reg.Matches(html);
            // 如果存在的话，就使用修改接口
            if (match.Count > 0) {
                string addressid = match[0].Groups[1].Value.ToString();
            }
            // 如果不存在的话，使用添加接口
            else {

            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string provinces_id = _fuck.GetProvinces("广东省");

            string cid = _fuck.GetCity(provinces_id, "东莞市");

            string aid = _fuck.GetArea(cid, "南城区");

            string code = _fuck.GetCodeId(aid);
        }
    }
}