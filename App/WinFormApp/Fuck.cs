using System;
using System.Collections.Generic;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using WinFormApp;
using System.Windows.Forms;
using WinFormApp.Models;
using System.IO;

namespace WinFormApp
{
    class Fuck
    {
        HttpHelper http;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Fuck()
        {
            // 实例化HttpHelper
            http = new HttpHelper();
        }

        /// <summary>
        /// 判断是否登陆
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private bool IsLogin(string cookie) {
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/ajax/isLogin",
                Method = "GET",
                Cookie = cookie,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return Convert.ToBoolean(html);
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

        public string GetProvinces(string str)
        {
            // 查看本地缓存是否存在json文件
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Cache/" + "provinces.json";
            // 预定义
            string provinces = "";
            // 如果存在则直接获取
            if (File.Exists(@path)) {
                provinces = File.ReadAllText(@path);
            
            // 如果不存在，则前往url读取并且缓存在本地
            } else {
                HttpItem item = new HttpItem() {
                    URL = "https://www.228.com.cn/ajax/loadRange?type=provinces&typeId=0",
                    Method = "GET",
                    Allowautoredirect = true,
                    AutoRedirectCookie = true
                };
                HttpResult result = http.GetHtml(item);
                provinces = result.Html;

                string currenctDir = AppDomain.CurrentDomain.BaseDirectory + "/Cache/";
                if (!Directory.Exists(currenctDir)) {
                    Directory.CreateDirectory(currenctDir);
                }
                File.WriteAllText(currenctDir + "provinces.json", provinces);
            }

            Provinces p = (Provinces)HttpHelper.JsonToObject<Provinces>(provinces);

            string id = "";
            foreach (var _p in p.rangeList) {
                if (_p.NAME == str) {
                    id = _p.PROVINCEID.ToString();
                    break;
                }
            }

            return id;
        }

        /// <summary>
        ///  获取极验结果
        /// </summary>
        /// <returns></returns>
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
                MessageBox.Show("极验账户积分不足！请充值再尝试！");
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
        /// 必须设置 Allowautoredirect = false 才可以获取redirectUrl(获取302跳转URl)，虽然默认就是为false。
        /// 某些场合必须设置为 true 才合适，但某些场合必须设置为false。
        /// 譬如我登录之后，就要获取该页面的cookie，但由于跳转，到导致获取的是跳转后的Cookie，是错误的。
        /// 所以应该设置为false。可以先获取cookie，再跳转。一开始我手贱设置为true。结果思路是错的，坑了好久！
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Login(string username, string password)
        {
            // 读取本地Cookie
            string mycookie =  Functions.ReadCookie(username);

            // 如果为空或者登陆无效，则重新请求，否则直接返回该cookie。
            if (mycookie == "" || IsLogin(mycookie) == false) {
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
                // 将Cookie保存到本地
                Functions.SaveCookie(username, mycookie);
            }
            return mycookie;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public UserInfo GetUserInfo(string cookie) {
            HttpItem item = new HttpItem() {
                URL = "https://www.228.com.cn/ajax/getUserInfoFact",
                Method = "GET",
                Cookie = cookie,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            UserInfo user = (UserInfo)HttpHelper.JsonToObject<UserInfo>(html);
            return user;
        }
    }
}
