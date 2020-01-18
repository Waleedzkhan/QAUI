using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace QAUI.Models
{
    public class Answer
    {
        [DataMember(Name ="Id")]
        public long Id { get; set; }
        [DataMember(Name ="Body")]
        public String Body { get; set; }
        [DataMember(Name ="Accepted")]
        public bool Accepted { get; set; }
    }
}
