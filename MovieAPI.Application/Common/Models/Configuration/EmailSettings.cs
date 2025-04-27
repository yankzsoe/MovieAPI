using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.Common.Models.Configuration {
    public class EmailSettings {
        public string Address { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string SmtpHost { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public int Timeout { get; set; }
    }
}
