using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO.Account
{
    public class RegistrationDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? Birthday { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Number { get; set; }
    }
}
