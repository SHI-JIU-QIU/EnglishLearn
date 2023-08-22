using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Options
{
    public class EmailSenderSettings
    {
        public string Host { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
    }
}
