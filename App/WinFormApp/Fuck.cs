using System;
using System.Collections.Generic;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using WinFormApp;
using System.Windows.Forms;
using WinFormApp.Models;

namespace WinFormApp
{
    class Fuck
    {
        HttpHelper http;

        public Fuck()
        {
            // 实例化
            http = new HttpHelper();
        }

        /// <summary>
        /// 抓包获取极验参数
        /// </summary>
        /// <returns></returns>
        public string GetGeetet()
        {
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/ajax/registerBind?t=" + Functions.getTime(),
                Method = "GET",
                Timeout = 30000,
                ContentType = "text/html",
                Allowautoredirect = true
            };
            HttpResult result = http.GetHtml(item);
            return result.Html;
        }

        public Jiyan GetJiyan()
        {
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
            string strHash = Functions.DictionaryToPostString(d);
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
        public string Login(string username, string password)
        {
            string mycookie =  Functions.Read(username);
            if (mycookie == "") {
                Jiyan jy = GetJiyan();
                Dictionary<string, string> d = new Dictionary<string, string> {
                    { "username", username },
                    { "password", password },
                    { "geetest_challenge", jy.challenge },
                    { "geetest_validate", jy.validate },
                    { "geetest_seccode", jy.validate + "|jordan" }
                };
                string strHash = Functions.DictionaryToPostString(d);
                HttpItem item = new HttpItem() {
                    URL = "https://www.228.com.cn/auth/login",
                    Method = "POST",
                    Timeout = 30000,
                    Postdata = strHash,
                    ContentType = "application/x-www-form-urlencoded",
                    Allowautoredirect = false,
                };
                HttpResult result = http.GetHtml(item);
                mycookie = result.Cookie;
                Functions.SaveCookie(username, mycookie);
            }
            return mycookie;
        }
    }
}
