using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSecurityApp.Core.Common
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Authority { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
