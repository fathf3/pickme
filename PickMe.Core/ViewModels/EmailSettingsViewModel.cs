using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMe.Core.ViewModels
{
    public class EmailSettingsViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Smtp { get; set; } = string.Empty;
        public int Port { get; set; }
        public string From { get; set; } = string.Empty;
    }
}
