using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace TeamspeakWebAdmin.Models
{
    public class ConnectModel
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Guid { get; set; }
        public bool Error { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string RememberMe { get; set; }

        public bool LoginSufficent()
        {
            return !string.IsNullOrEmpty(IP) && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && Port != 0;
        }

        public bool ConnectSufficent()
        {
            return LoginSufficent() && !string.IsNullOrEmpty(Guid) && !string.IsNullOrEmpty(IP);
        }
    }
}
