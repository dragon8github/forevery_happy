using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;

namespace FastVerCode
{
    public class VerCode
    {   
        /// <summary>
        /// 联众打码平台获取用户信息
        /// </summary>
        /// <param name="strVcodeUser">平台用户名</param>
        /// <param name="strVcodePass">平台密码</param>
        /// <returns>用户信息</returns>]
        private static string serverAddress = "v1-dll-api.jsdama.com";
        public static string GetUserInfo(string strVcodeUser, string strVcodePass)
        {
            string postdata = Properties.Resources.getUserCode;
            postdata = postdata.Replace("[username]", strVcodeUser);
            postdata = postdata.Replace("[password]", ToBase64(strVcodePass));
            string content = HttpRequest("http://"+serverAddress+"/api.php?mod=yzm&act=point", postdata, "multipart/form-data; boundary=---------------------------7dd3901221176");
            string flag = Between(content, "result\":", ",");
            string message = Between(content, "data\":", "}");
            if (flag.Equals("false"))
            {
                return (message);
           
            }
            else
            {
                return ("亲爱的联众用户,您当前的余额为" + message);
            }

        }
        /// <summary>
        /// 报告错误码
        /// </summary>
        /// <param name="strVcodeUser">平台用户名</param>
        /// <param name="vcodeId">打码id</param>
        public static void ReportError(string strVcodeUser, string vcodeId) {
            string[] infos = vcodeId.Split('&');
            string postdata = Properties.Resources.getErrorCode.Replace("[验证码id]", infos[0]);
            byte[] data = Encoding.UTF8.GetBytes(postdata.ToString());
            HttpRequest("http://"+infos[1]+".hyslt.com/api.php?mod=dmuser&act=yzm_error", postdata, "multipart/form-data; boundary=---------------------------7dd3901221176");

        }
        /// <summary>
        /// 上传byte[]验证码进行识别
        /// </summary>
        /// <param name="vcode">byte[]验证码</param>
        /// <param name="len">验证码长度</param>
        /// <param name="strVcodeUser">平台用户名</param>
        /// <param name="strVcodePass">平台密码</param>
        /// <param name="strSoftkey">作者id</param>
        /// <returns>打码结果信息</returns>
        public static string RecByte_A(byte[] vcode, int len, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            return recCode(vcode, len, 0, 0, 0, strVcodeUser, strVcodePass, strSoftkey);
        }
        /// <summary>
        /// 上传本地图片验证码识别
        /// </summary>
        /// <param name="strYZMPath">本地验证码路径</param>
        /// <param name="strVcodeUser">平台用户名</param>
        /// <param name="strVcodePass">平台密码</param>
        /// <param name="strSoftkey">作者id</param>
        /// <returns>打码结果信息</returns>
        public static string RecYZM_A(string strYZMPath, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = new Bitmap(strYZMPath);
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] photo_byte = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(photo_byte, 0, Convert.ToInt32(ms.Length));
            bmp.Dispose();
            string returnMess= RecByte_A(photo_byte, photo_byte.Length, strVcodeUser, strVcodePass, strSoftkey);
            return returnMess;
        }
        /// <summary>
        /// 平台账号注册
        /// </summary>
        /// <param name="strVcodeUser">平台用户名</param>
        /// <param name="strVcodePass">平台密码</param>
        /// <returns>注册状态</returns>
        public static string Reglz(string strVcodeUser, string strVcodePass)
        {
            string url = "http://"+serverAddress+"/api.php?mod=yzm&act=register";
            string postdata = Properties.Resources.Reglz;
            postdata = postdata.Replace("[username]", strVcodeUser);
            postdata = postdata.Replace("[password]", strVcodePass);
            string content = HttpRequest(url, postdata, "multipart/form-data; boundary=---------------------------7dd3901221176");
            string flag= Between(content,"result\":",",\"");
            if (flag.Equals("true"))
            {
                return "注册成功";
            }
            else {
                return Between(content,  "data\":", "}");
            }
        }
        /// <summary>
        ///通过传送字节获取验证码结果，区别是所有的参数写在函数里面，不用写在配置文件里面
        /// </summary>
        /// <param name="vocde">验证码字节集</param>
        /// <param name="len">验证码长度</param>
        /// <param name="codeType">验证码类型，0代表数字，1代表中文 默认为0</param>
        /// <param name="codeMinLen">验证码最小长度，验证码最小位数，不想设置默认为0</param>
        /// <param name="codeMaxLen">验证码最大长度，验证码最大位数，不想设置默认为0</param>
        /// <param name="strVcodeUser">软件开发者账号</param>
        /// <param name="strVcodePass">软件开发者密码</param>
        /// <param name="strSoftkey">作者id</param>
        /// <returns></returns>
        public static string RecByte_A_2(byte[] vcode, int len, int codeType, int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey) {
            return recCode(vcode,len,codeType,codeMinLen,codeMaxLen,strVcodeUser,strVcodePass,strSoftkey);
        }
        /// <summary>
        ///通过上传本地图片获取验证码结果，区别是所有的参数写在函数里面，不用写在配置文件里面
        /// </summary>
        /// <param name="strYZMPath">本地验证码地址</param>
        /// <param name="codeType">验证码类型，0代表数字，1代表中文 默认为0</param>
        /// <param name="codeMinLen">验证码最小长度，验证码最小位数，不想设置默认为0</param>
        /// <param name="codeMaxLen">验证码最大长度，验证码最大位数，不想设置默认为0</param>
        /// <param name="strVcodeUser">软件开发者账号</param>
        /// <param name="strVcodePass">软件开发者密码</param>
        /// <param name="strSoftkey">作者id</param>
        /// <returns></returns>
        public static string RecYZM_A_2(string strYZMPath, int codeType,int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = new Bitmap(strYZMPath);
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] photo_byte = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(photo_byte, 0, Convert.ToInt32(ms.Length));
            bmp.Dispose();
            return recCode(photo_byte, photo_byte.Length, codeType, codeMinLen, codeMaxLen, strVcodeUser, strVcodePass, strSoftkey);
        }
        
        private static string HttpRequest(string url, string postData, string ContentType) {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = ContentType;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; MALN)";
            request.KeepAlive = true;
            request.Timeout = 15000;
            byte[] data = Encoding.UTF8.GetBytes(postData.ToString());
            request.ContentLength = data.Length;
            Stream outputStream = request.GetRequestStream();
            outputStream.Write(data, 0, data.Length);
            outputStream.Close();
            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            request.Abort();
            return content;
        
        }
        private static string UploadImg(string url,byte[] data, string ContentType)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = ContentType;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; MALN)";
            request.KeepAlive = true;
            request.Timeout = 15000;
            request.ContentLength = data.Length;
            Stream outputStream = request.GetRequestStream();
            outputStream.Write(data, 0, data.Length);
            outputStream.Close();
            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            request.Abort();
            return content;

        }
        private static string Between(string str, string leftstr, string rightstr)
        {
            int i = str.IndexOf(leftstr) + leftstr.Length;
            string temp = str.Substring(i, str.IndexOf(rightstr, i) - i);
            return temp;
        }
        private static string ToBase64(String value)
        {
            string str = Convert.ToBase64String(Encoding.Default.GetBytes(value));
            return str;
        }
        private static String recCode(byte[] vcode, int len, int codeType, int codeMinLen, int codeMaxLen, string strVcodeUser, string strVcodePass, string strSoftkey)
        {
                string postdata = Properties.Resources.getUser;
                postdata = postdata.Replace("[username]", strVcodeUser);
                postdata = postdata.Replace("[password]", ToBase64(strVcodePass));
                string content = "";
                string server = serverAddress;
                string baotou = Properties.Resources.getUpload1;
                baotou = baotou.Replace("[username]", strVcodeUser);
                baotou = baotou.Replace("[password]", ToBase64(strVcodePass));
                byte[] str1 = Encoding.Default.GetBytes(baotou + "\r\n" + "\r\n");
                string baowei = Properties.Resources.getUpload2;
                baowei = baowei.Replace("[strSoftkey]", strSoftkey);
                baowei = baowei.Replace("[yzmtype]", codeType.ToString());
                baowei = baowei.Replace("[yzm_maxlen]", codeMaxLen.ToString());
                baowei = baowei.Replace("[yzm_minlen]", codeMinLen.ToString());
                byte[] str2 = Encoding.Default.GetBytes("\r\n" + baowei);
                byte[] fieldData = new byte[str1.Length + vcode.Length + str2.Length];
                str1.CopyTo(fieldData, 0);
                vcode.CopyTo(fieldData, str1.Length);
                str2.CopyTo(fieldData, str1.Length + vcode.Length);
                content = UploadImg("http://" + server + "/api.php?mod=yzm&act=add", fieldData, "multipart/form-data; boundary=---------------------------7dd165a60630");
                string flag = Between(content, "result\":", ",");
                string message = Between(content, "data\":\"", "\"");
                if (flag.Equals("false")) {
                    return message;
                }
                string codeId = Between(content, "true,\"data\":", "}");
                postdata = Properties.Resources.getResultParameter.Replace("[打码Id]", codeId);
                System.Threading.Thread.Sleep(2000);
                int waitCount = 0;
                while (true)
                {
                    content = HttpRequest("http://" + server + "/api.php?mod=yzm&act=result", postdata, "multipart/form-data; boundary=---------------------------7dd3901221176");
                    if (content.LastIndexOf("wait dama") != -1 || content.LastIndexOf("等待识别") != -1)
                    {
                        waitCount = waitCount + 1;
                        if (waitCount == 15)
                        {
                            return "Error:TimeOut!";
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                    else if (content.LastIndexOf("") != -1)
                    {
                        string[] servers = server.Split('.');
                        string code = Between(content, "true,\"data\":", "}");
                        code = code.Replace("\"", "");
                        return code + "|!|" + codeId + "&" + servers[0];
                    }
                    else
                    {
                        return "Error:TimeOut!";
                    }

                }
        
        }
    }
}
