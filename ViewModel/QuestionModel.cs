using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunStack.ViewModel
{
    public class QuestionModel
    {
       // public string Question { get; set;}
        public int QuestionId { get; set; }
        public string Question1 { get; set; }
        public string Postedby { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}