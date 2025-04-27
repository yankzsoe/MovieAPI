using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.Common.Models {
    public class Email {
        public List<string> Addresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
