using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Net;
using System.IO;

namespace CsharpHttpHelper_Demo
{
    public partial class Text : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //1.增加IsReset，是否重置request,response的值，默认不重置，当设置为True时request,response将被设置为Null。
            //2.添加byte返回类型，当设置byte时不返回Html，共计设置三种：byte,html,htmlbyte

        }
    }
}