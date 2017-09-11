﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WinFormApp
{
    class Functions
    {
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp() {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        ///  查看该路径是否存在，如果不存在，则创建这个文件夹，如果存在则什么都不做
        /// </summary>
        /// <param name="Path">要查询的路径</param>
        /// <returns></returns>
        public static string MakePath(string Path) {
            if (!Directory.Exists(Path)) {
                Directory.CreateDirectory(Path);
            }
            return Path;
        }

        /// <summary>
        /// 是否存在资源路径assets，如果不存在则创建，返回路径
        /// </summary>
        /// <returns></returns>
        public static string AssetsPath() {
            string Path = Directory.GetCurrentDirectory() + "/assets";
            return MakePath(Path);
        }

        public static string GetPicExt() {
            return ".jpg";
        }
    }
}