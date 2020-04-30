using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunStack.ViewModel
{
    public class AnswerModel
    {
        public int AnswerId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<int> Id { get; set; }
        public string Answer1 { get; set; }

        public int RegisterUser { get; set; }
        public int Question { get; set; }

        public string UserId { get; set; }
    }
}