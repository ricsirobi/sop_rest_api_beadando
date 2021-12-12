using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC92HL_SOP_BEADANDO_CLIENT.data
{
    public class User
    {
        private int id;
        private string username;
        private string password;
        private string token;
        private int admin;

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Token{ get => token; set => token= value; }

        public int Admin { get => admin; set => admin = value; }

        public bool isAdmin()
        {
            return admin == 1 ? true : false;
        }
    }
}
