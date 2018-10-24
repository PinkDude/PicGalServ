using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class PersonInfo
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        public bool Autor { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
