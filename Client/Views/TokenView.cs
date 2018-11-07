using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class TokenView
    {
        public string Access_token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int Id { get; set; }
        public string Photo { get; set; }

        public bool isSuccesed { get; set; } = true;
    }
}
