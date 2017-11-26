﻿using System.Collections.Generic;

namespace WinFormApp.Models
{
    public class RangeList
    {
        public int PROVINCEID { get; set; }
        public int CITYID { get; set; }
        public int AREAID { get; set; }
        public int CODEID { get; set; }
        public string NAME { get; set; }
        
    }

    public class Provinces
    {
        public IList<RangeList> rangeList { get; set; }
        public int ajaxResponse { get; set; }
    }
}
