using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Service.AccountHelp
{
    public class OperationResult
    {
        public string Access_token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int Id { get; set; }
        public string Photo { get; set; }

        public bool isSuccesed { get; set; } = true;
    }
}
