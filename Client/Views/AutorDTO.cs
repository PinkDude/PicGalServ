using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class AutorDTO
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Photo { get; set; }

        public string ShotName
        {
            get
            {
                return $"{LastName} {FirstName?[0]} {MiddleName?[0]}";
            }
        }

        public string FullName { get
            {
                return $"{LastName} {FirstName} {MiddleName}";
            } }
    }
}
