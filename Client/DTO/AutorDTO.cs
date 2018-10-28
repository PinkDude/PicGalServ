using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class AutorDTO
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Photo { get; set; }

        public string ShotName { get
            {
                return $"{LastName} {FirstName?[0]} {MiddleName?[0]}";
            } }
    }
}
