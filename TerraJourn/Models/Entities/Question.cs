using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TerraJourn.Models.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int OrderNumber { get; set; }
        public string PhotoUrl { get; set; }
        public virtual ICollection<Option> Options { get; set; }
    }
}