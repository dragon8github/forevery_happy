using System;
using System.Windows.Forms;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        HttpHelper http;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置默认的显示项
            this.tabControl1.SelectedIndex = 3;

            // 实例化
            http = new HttpHelper();
        }

        /// <summary>
        /// 获取毫秒级的时间戳
        /// </summary>
        /// <returns></returns>
        public static string getTime()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
        }

        /// <summary>
        ///  字典转化为postData 字符串类型
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        static string DictionaryToPostString(Dictionary<string, string> ht)
        {
            string str = "";
            foreach (KeyValuePair<string, string> de in ht) {
                str += de.Key + "=" + de.Value + "&";
            }
            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 储存cookies文本到本地
        /// </summary>
        /// <param name="txtName"></param>
        /// <param name="CookieStr"></param>
        static void SaveCookie(string txtName, string CookieStr) {
            string currenctDir = AppDomain.CurrentDomain.BaseDirectory + "/Cookies/";
            if (!Directory.Exists(currenctDir)) {
                Directory.CreateDirectory(currenctDir);
            }
            File.WriteAllText(currenctDir + txtName + ".txt", CookieStr);
        }

        /// <summary>
        /// 抓包获取极验参数
        /// </summary>
        /// <returns></returns>
        private string GetGeetet() {
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/ajax/registerBind?t=" + getTime(),
                Method = "GET",
                Timeout = 30000,
                ContentType = "text/html",
                Allowautoredirect = true
            };
            HttpResult result = http.GetHtml(item);
            return result.Html;
        }

        private Jiyan GetJiyan() {
            string geetest_code = GetGeetet();
            Geetest geetest = (Geetest)HttpHelper.JsonToObject<Geetest>(geetest_code);
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("gt", geetest.gt);
            d.Add("challenge", geetest.challenge);
            d.Add("model", "3");
            d.Add("supportclick", "jiyan");
            d.Add("referer", "https://www.228.com.cn/auth/login");
            d.Add("return", "json");
            d.Add("user", "dragon8jiyan");
            d.Add("pass", "202063");
            d.Add("devuser", "dragon8jiyan");
            string strHash = DictionaryToPostString(d);
            HttpItem item = new HttpItem() {
                URL = "http://jiyanapi.c2567.com/shibie?" + strHash,
                Method = "GET",
                Timeout = 30000,
                ResultType = ResultType.String
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            Jiyan jy = (Jiyan)HttpHelper.JsonToObject<Jiyan>(html);
            if (jy.status == "stop") {
                throw new Exception("极验账户积分不足！请充值再尝试！");
            }
            if (jy.status != "ok") {
                MessageBox.Show("获取失败，正在重新获取...");
                return GetJiyan();
            }
            return jy;
        }

        /// <summary>
        /// 获取cookie
        /// 神坑记录：http://www.sufeinet.com/forum.php?mod=viewthread&tid=9999
        /// 必须设置Allowautoredirect = false才可以获取redirectUrl(获取302跳转URl)
        /// 某些场合必须设置为true才合适，但某些场合必须设置为false.
        /// 譬如我登录之后，就要获取该页面的cookie，但由于跳转，到导致获取的是跳转后的Cookie，是错误的。
        /// 所以应该设置为false。可以先获取cookie，再跳转。坑死了。
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string Login(string username, string password) {
            Jiyan jy = GetJiyan();
            Dictionary<string, string> d = new Dictionary<string, string> {
                { "username", username },
                { "password", password },
                { "geetest_challenge", jy.challenge },
                { "geetest_validate", jy.validate },
                { "geetest_seccode", jy.validate + "|jordan" }
            };
            string strHash = DictionaryToPostString(d);
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/auth/login",
                Method = "POST",
                Timeout = 30000,
                Postdata = strHash,
                ContentType = "application/x-www-form-urlencoded",
                Allowautoredirect = false,
            };
            HttpResult result = http.GetHtml(item);
            string cookie = result.Cookie;
            SaveCookie(username, cookie);
            return cookie;
        }

        /// <summary>
        /// 读取txt文本信息
        /// </summary>
        /// <param name="path"></param>
        public string Read(string username)
        {
            string mycookie = AppDomain.CurrentDomain.BaseDirectory + "/Cookies/" + username + ".txt";
            if (!File.Exists(@mycookie)) {
                return "";
            }
            return File.ReadAllText(@mycookie);
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(GetGeetet());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Jiyan jy = GetJiyan();
            MessageBox.Show("validate:" + jy.validate + "\r\n challenge:" + jy.challenge + "\r\n status:" + jy.status);
        }

        /// <summary>
        /// 尝试登录并且获取cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            string cookie = Read("13713332652");
            //string cookie = Login("13713332652", "202063sb");
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/personorders/myorder.html",
                Method = "GET",
                Timeout = 10000,
                Cookie = cookie,
                ResultType = ResultType.String,
                Allowautoredirect = true
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            MessageBox.Show(html);
        }


        public class Geetest
        {
            /// <summary>
            /// challenge
            /// </summary>
            public string challenge { get; set; }
            /// <summary>
            /// gt
            /// </summary>
            public string gt { get; set; }
            /// <summary>
            /// success
            /// </summary>
            public string success { get; set; }
        }

        public class Jiyan
        {
            /// <summary>
            /// status
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// challenge
            /// </summary>
            public string challenge { get; set; }

            /// <summary>
            /// validate
            /// </summary>
            public string validate { get; set; }
            
        }       
    }
}