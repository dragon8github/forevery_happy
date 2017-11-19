using System;
using System.Collections.Generic;
using System.IO;

namespace WinFormApp
{
    class Functions
    {
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
        public static string DictionaryToPostString(Dictionary<string, string> ht)
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
        public static void SaveCookie(string txtName, string CookieStr)
        {
            string currenctDir = AppDomain.CurrentDomain.BaseDirectory + "/Cookies/";
            if (!Directory.Exists(currenctDir)) {
                Directory.CreateDirectory(currenctDir);
            }
            File.WriteAllText(currenctDir + txtName + ".txt", CookieStr);
        }

        /// <summary>
        /// 读取txt文本信息
        /// </summary>
        /// <param name="path"></param>
        public static string Read(string username)
        {
            string mycookie = AppDomain.CurrentDomain.BaseDirectory + "/Cookies/" + username + ".txt";
            if (!File.Exists(@mycookie)) {
                return "";
            }
            return File.ReadAllText(@mycookie);
        }
    }
}
