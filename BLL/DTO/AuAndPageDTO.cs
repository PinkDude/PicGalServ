using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class AuAndPageDTO
    {
        public int Count { get; set; }

        public IEnumerable<PersonInfo> Persons { get; set; }
    }
}
