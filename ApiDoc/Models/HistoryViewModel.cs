using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiDoc.Models
{
    public class HistoryViewModel
    {
        public HistoryViewModel(){}

        public HistoryViewModel(int rev1, int rev2)
        {
            Rev1 = rev1;
            Rev2 = rev2;
        }

        public int Rev1 { get; set; }
        public int Rev2 { get; set; }
    }
}